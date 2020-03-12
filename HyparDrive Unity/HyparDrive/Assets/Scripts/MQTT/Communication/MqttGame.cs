using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MqttGame : MonoBehaviour {

    public static MqttGame INSTANCE;

    public MqttHandler netHandler;

    // Defaults to localhost
    public string ip = "127.0.0.1";
    // Best port ever
    public int port = 1883;


    private void Start () {
        INSTANCE = this;

        netHandler = FindObjectOfType<MqttHandler>();
        netHandler.Connect(IPAddress.Parse(ip), port);
    }

}
