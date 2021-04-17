using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

using UnityEngine;

namespace HauteTechnique.Dmx
{
    public class ArtNetDmxNode : DmxNode
    {
        private Socket socket; 

        bool toggle = true;

        public ArtNetDmxNode(string name, string ip, int port = ArtNetClient.DefaultPort) : base(name)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
	        
            try {
                socket.Connect(IPAddress.Broadcast, 6454);
            }
            catch(Exception e) {
                Debug.LogWarning("DMX: Not connected? " + e.Message);    
            }
        }

        public override void Send()
        {
            int failed = 0;
            
            DmxUniverse universeZero = null;
            
       
            // Send all universes
            foreach (var universe in universes)
            {  
                if(universe.Value.universe == 0)
                {
                    universeZero = universe.Value;
                }
                else 
                {
                    int bytes = SendData(universe.Value.universe, universe.Value.artnetPacket);
                    if(bytes < 0) {
                        failed++;
                    }
                 }
            }

            toggle = !toggle;

            if(universeZero != null)
            {  
                // Notify the node that all packets are sent
                int bytes2 = SendData(universeZero.universe, universeZero.artnetPacket);
                if(bytes2 < 0) {
                    failed++;
                }
            }

            if(failed > 0) {
                 Debug.LogWarningFormat("DMX: Failed to send {0} packets in this run.", failed);    
            }
        }

        public int SendData(int universeId, byte[] _data) 
        {
            int bytesSend = -1;
            //Debug.Log("Sending from ArtNetDMXNode" + _data.Length);
            try
            {          
                bytesSend = socket.Send(_data);

                if(bytesSend != _data.Length) {
                    Debug.LogWarningFormat("DMX: Only send: {0} bytes of the {1} total", bytesSend, _data.Length);
                }
            }
            catch(SocketException e) {
                Debug.LogWarningFormat("DMX: SocketExcpetion when sending universe: {0} message '{1}'", universeId, e.Message);
            }
            catch(Exception e) {
                Debug.LogWarningFormat("DMX: Exception when sending universe: {0} message: '{1}'", universeId, e.Message);
            }

            return bytesSend;
        }
        
        public override void Close()
        {
            socket.Close();
        }
    }
}