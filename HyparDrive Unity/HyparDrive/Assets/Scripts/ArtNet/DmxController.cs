using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HauteTechnique.Dmx
{
    public class DmxController
    {
        private List<DmxNode> nodes = new List<DmxNode>();

        public DmxController()
        {

        }

        public void Send()
        {
            foreach (DmxNode node in nodes)
                node.Send();
        }

        public void AddNode(DmxNode dmxNode)
        {
            nodes.Add(dmxNode);
        }

        public DmxNode GetNode(int i)
        {
            return nodes.ElementAt(i);
        }
    }
}