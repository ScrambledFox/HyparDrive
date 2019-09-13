using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;
using System.Linq;


/// <summary>
/// Manages all the Communication of the client. Connects and subscribes to broker. Publishes commands if Command Sender test scene is active.
/// </summary>
public class MqttHandler : MonoBehaviour
{
    private MqttClient client;

    public void Connect(IPAddress ip, int port)
    {
        // Initialising the Client
        client = new MqttClient(ip, port, false, null);

        client.MqttMsgPublishReceived += MessageReceived;
        client.MqttMsgDisconnected += OnDisconnected;
        //client.MqttMsgDisconnected += Reconnect;

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        client.Subscribe(new string[] { "/interaction/feed" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { "/interaction/timeSync" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        sendTimeEqualizer();
    }

    public void Publish(string topic, string msg)
    {
        client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg));
    }

    void MessageReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received from broker: " + System.Text.Encoding.UTF8.GetString(e.Message));
        Debug.Log(e.Topic);

        switch (e.Topic)
        {
            case "/interaction/feed":
                CommandProcessor.ProcessInteractionCommand(System.Text.Encoding.UTF8.GetString(e.Message));
                break;
            case "/interaction/timeSync":
                if (System.Text.Encoding.UTF8.GetString(e.Message) != "resetTime")
                {
                    CommandProcessor.ProcessTimeCommand(System.Text.Encoding.UTF8.GetString(e.Message));
                }
                break;
            default:
                Debug.Log("ERROR. TOPIC DOES NOT EXIST.");
                break;
        }
    }

    void Reconnect(object sender, MqttMsgDisconnect e)
    {
        client.Connect(Guid.NewGuid().ToString());
    }

    private void OnDisconnected(object sender, EventArgs e)
    {
        Debug.LogError("Disconnected from MQTT broker: " + e.ToString());
        TimeSyncer.happyFam = false;
        Array.Clear(TimeSyncer.unitStatus, 0, TimeSyncer.unitStatus.Length);
    }

    void sendTimeEqualizer()
    {
        //TODO: run elk uur voor reset van millis time op Arduinos
        Publish("/interaction/timeSync", "resetTime");

    }

}
