using Scripts.Army.TypesSoldiers;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.StaticData
{
    [CreateAssetMenu(fileName ="MonsterData",menuName ="StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject 
    {
        public MonsterTypeId MonsterTypeId;
        public GameObject Prefab;
    }
}
