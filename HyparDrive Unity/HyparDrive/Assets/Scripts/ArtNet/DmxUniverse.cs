using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace HauteTechnique.Dmx
{
    public class DmxUniverse
    {
        private const int ArtnetHeaderSize = 18;
        private const int DmxDataSize = 512;

        private int _universe;
        private byte[] _data;// = new byte[ArtnetHeaderSize + DmxDataSize];

        public DmxUniverse(int universe)
        {
            _universe = universe;
            _data = ArtNetClient.CreateUniverseBuffer(_universe);
        }

        public void SetValue(int index, byte value)
        {
            if(index >= 0 && index < DmxDataSize)  {
                _data[ArtnetHeaderSize + index] = value;
            }
            else {
                Debug.LogWarningFormat("DmxUniverse{0}: Channel: {1} out of range", _universe, index);
            }
        }

        public byte GetValue(int index)
        {
            byte value = 0;
            
            if(index >= 0 && index < DmxDataSize) {
                value = _data[ArtnetHeaderSize + index];
            }
            else {
                Debug.LogWarningFormat("DmxUniverse{0}: Channel: {1} out of range", _universe, index);
            }

            return value;
        }

        // Getters and Setter

        public int universe { get {return _universe; }}
        public byte[] artnetPacket {get {return _data; }}
    }
}