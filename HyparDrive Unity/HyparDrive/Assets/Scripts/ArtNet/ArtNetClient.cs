using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace HauteTechnique.Dmx
{
   public class ArtNetClient
    {
        public const int DefaultPort = 6454;

		private static byte[] artnetHeader = ASCIIEncoding.ASCII.GetBytes("Art-Net");
        
        private UdpClient client;
        private Socket socket; 
      	private IPEndPoint remote;
		private MemoryStream stream;
		private BinaryWriter packetWriter;

		private enum OpCodes
		{
			OpDmx = 0x5000,
		}
			
        public ArtNetClient(string ip, int port = DefaultPort)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            client = new UdpClient();
            remote = new IPEndPoint(ipAddress, port);

			stream = new MemoryStream (1000);
			packetWriter = new BinaryWriter(stream, Encoding.UTF8);

            socket = new Socket(remote.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            //socket.DontFragment = true;
	        socket.EnableBroadcast = true;
            socket.SendBufferSize = 100000;
        }
			
        public void Send(int universe, byte[] dmxData)
        {
			stream.Position = 0;
			WriteArtnetHeader (packetWriter, (UInt16)OpCodes.OpDmx);
			WriteArtnetDmx (packetWriter, (byte)universe, dmxData);

			//UnityEngine.Debug.Log ("Sending: " + stream.Length + " bytes");
			client.Send (stream.GetBuffer (), (int)stream.Length, remote);
        }
			
        public int Send(byte[] packet)
        {
            try
            {          
                Debug.Log("ArnetClient: Sending: " + packet.Length + " bytes");
                //client.Send(packet, packet.Length, remote);
                int bytesSend = socket.SendTo(packet, remote);

                if(bytesSend != packet.Length)
                {
                    Debug.LogWarningFormat("ArtNet: Only send {0} bytes of the {1} total", bytesSend, packet.Length);
                    bytesSend = -1;
                }

                return bytesSend;
            }
            catch(Exception e)
            {
                Debug.LogWarning("Excpetion when sending: " + e.Message);
                Debug.LogWarning(e.StackTrace);
            }

            return -1;
        }

        public void Close()
        {
            client.Close();
            socket.Close();
            stream.Dispose();
        }

        public static byte[] CreateUniverseBuffer(int universe)
        {
            using(MemoryStream stream = new MemoryStream(512 + 18))
            {
                BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);
                stream.Position = 0;
                writer.Write (artnetHeader);	            // header	
                writer.Write ((byte)0);			            // null from null terminted string
                writer.Write ((UInt16)OpCodes.OpDmx);		// opCode
                writer.Write (Swap(0x5000));	            // version
                writer.Write ((byte)0);						// sequnece
                writer.Write((byte)0);						// physical
                writer.Write((byte)universe);				// universe
                writer.Write((byte)0);						// net
                writer.Write(Swap((ushort)512));            // length
                return stream.GetBuffer();
            }
        }

		public static UInt16 Swap(UInt16 value)
		{
			return (UInt16)(value >> 8 | value << 8);
		}

		private static void WriteArtnetHeader(BinaryWriter writer, UInt16 opCode)
		{
			writer.Write (artnetHeader);	// header	
			writer.Write ((byte)0);			// null from null terminted string
			writer.Write (opCode);			// opCode
			writer.Write (Swap(0x5000));	// version
		}

		private static void WriteArtnetDmx(BinaryWriter writer, byte universe, byte[] dmxData)
		{
			writer.Write ((byte)0);						// sequnece
			writer.Write((byte)0);						// physical
			writer.Write(universe);						// universe
			writer.Write((byte)0);						// net
			writer.Write(Swap((UInt16)dmxData.Length));	// length
			writer.Write(dmxData);						// dmx data
		}
    }
}