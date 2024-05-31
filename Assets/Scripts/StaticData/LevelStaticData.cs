using Scripts.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.StaticData
{
    [CreateAssetMenu(fileName ="LevelData",menuName ="StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;
        public List<EnemySpawnerData> EnemySpawners;
    }
}