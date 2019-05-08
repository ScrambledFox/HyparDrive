using System.Collections.Generic;

namespace HauteTechnique.Dmx
{
    abstract public class DmxNode
    {
        public Dictionary<int, DmxUniverse> universes;
        public int maxUniverse = 0;

        private string Name;

        public DmxNode(string name)
        {
            Name = name;
            universes = new Dictionary<int, DmxUniverse>();
        }

        public void AddUniverse(DmxUniverse dmxUniverse)
        {
            if(dmxUniverse.universe > maxUniverse) {
                maxUniverse = dmxUniverse.universe;
            }   

            universes.Add(dmxUniverse.universe, dmxUniverse);
        }

        public void Sort() 
        {
            
        }

        public DmxUniverse GetUniverse(int universeId)
        {
            DmxUniverse universe;

            if(universes.TryGetValue(universeId, out universe)) {
                return universe;
            }
    
            return null;
        }

        public string GetName()
        {
            return Name;
        }

        public virtual void Close()
        {
            
        }

        abstract public void Send();
    }
}