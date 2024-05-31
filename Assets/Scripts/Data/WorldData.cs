using System;

namespace Scripts.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public bool IsMainScren = false;
        public int BaseRaibProc = 0;
        public bool Activiti = true;
        public WorldData(string initialLevel,int numberRaid) 
        {
            PositionOnLevel = new PositionOnLevel(initialLevel, numberRaid);
        }
    }
}
