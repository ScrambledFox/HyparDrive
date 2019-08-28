using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MqttGame : MonoBehaviour {

    MqttHandler netHandler;

    // Defaults to localhost
    public string ip = "127.0.0.1";
    // Best port ever
    public int port = 1337;


    private void Start () {
        netHandler = FindObjectOfType<MqttHandler>();
        netHandler.Connect(IPAddress.Parse(ip), port);
    }

}
