using Scripts.Army;
using Scripts.Army.TypesSoldiers;
using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.StaticData;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Logic.EnemySpawners
{
    public class SpawnMarker : MonoBehaviour
    {
        public MonsterTypeId MonsterTypeId;
        public int IdRaid;
        public int Id;
        public bool Slain = false;
        public List<Soldier> ArmyEnemySoldiers = new List<Soldier>();
    }
}