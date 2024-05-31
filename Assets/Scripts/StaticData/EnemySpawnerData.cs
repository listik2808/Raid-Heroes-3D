using Scripts.Army.TypesSoldiers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.StaticData
{
    [Serializable]
    public class EnemySpawnerData
    {
        public int Id;
        public MonsterTypeId MonsterTypeId;
        public Vector3 Position;
        public int IdRaid;

        public EnemySpawnerData(int id, MonsterTypeId monsterTypeId, Vector3 position, int idraid)
        {
            Id = id;
            MonsterTypeId = monsterTypeId;
            Position = position;
            IdRaid = idraid;
        }
    }
}