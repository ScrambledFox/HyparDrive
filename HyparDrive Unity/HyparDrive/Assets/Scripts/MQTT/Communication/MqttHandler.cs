using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;


/// <summary>
/// Manages all the Communication of the client. Connects and subscribes to broker. Publishes commands if Command Sender test scene is active.
/// </summary>
public class MqttHandler : MonoBehaviour {
    private MqttClient client;

    public void Connect (IPAddress ip, int port) {
        // Initialising the Client
        client = new MqttClient(ip, port, false, null);

        client.MqttMsgPublishReceived += MessageReceived;
        client.MqttMsgDisconnected += OnDisconnected;
        //client.MqttMsgDisconnected += Reconnect;

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        client.Subscribe(new string[] { "/interaction/feed" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE});
        sendTimeEqualizer();
    }

    public void Publish (string topic, string msg) {
        client.Publish( topic, System.Text.Encoding.UTF8.GetBytes(msg) );
    }

    void MessageReceived ( object sender, MqttMsgPublishEventArgs e ) {
        Debug.Log("Received from broker: " + System.Text.Encoding.UTF8.GetString(e.Message));
        Debug.Log(e.Topic);

        switch (e.Topic)
        {
            case "/interaction/feed":
                CommandProcessor.ProcessInteractionCommand(System.Text.Encoding.UTF8.GetString(e.Message));
                break;
            case "/interaction/timeSync":
                CommandProcessor.ProcessTimeCommand(System.Text.Encoding.UTF8.GetString(e.Message));
                // if all arduinos ok top stuur ok.
                // anders nog een poging
                break;

            default:
                Debug.Log("ERROR TOPIC DOES NOT EXIST FUCK U");
                break;
        }
    }

    void Reconnect (object sender, MqttMsgDisconnect e) {
        client.Connect(Guid.NewGuid().ToString());
    }

    private void OnDisconnected (object sender, EventArgs e) {
        Debug.LogError("Disconnected from MQTT broker: " + e.ToString());
    }

    void sendTimeEqualizer()
    {
        Publish("/interaction/feed", "resetTime");
    }

}
