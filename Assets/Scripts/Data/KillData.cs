using System;
using System.Collections.Generic;

namespace Scripts.Data
{
    [Serializable]
    public class KillData
    {
        public List<int> ClearedSpawners = new List<int>();
        public List<int> ClireRaidZoneId = new List<int>();
        public bool SetClireZoneRaidId(int id)
        {
            foreach (var cl in ClireRaidZoneId)
            {
                if(cl == id)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckingRecordPassing(int index)
        {
            foreach(int point in ClearedSpawners)
            {
                if(point == index)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
