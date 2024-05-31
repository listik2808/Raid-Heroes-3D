using Scripts.Logic;
using System;

namespace Scripts.Data
{
    [Serializable]
    public class PointSpawn
    {
        public int IdSpawnerEnemy = 1;
        public int NextIdEnemy;
        public int IdRaid = 1;
        public int BestId = 0;
        public bool Reset = false;
        public void SetBestIdEnemy(int value)
        {
            if(value > BestId)
            {
                BestId = value;
            }
        }
    }
}