using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

using UnityEngine.UI;

public class mqttCommandSender : MonoBehaviour {

    private MqttClient client;

    public string ip = "127.0.0.1";
    public int port = 1337;

    private void Awake () {
        // Initialising the Client
        client = new MqttClient(IPAddress.Parse(ip), port, false, null);

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);
    }

    public void SendCommand (string msg) {
        client.Publish("blackout", System.Text.Encoding.UTF8.GetBytes(msg));
    }

}