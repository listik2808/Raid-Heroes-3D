using UnityEngine;

namespace Scripts.StaticData
{
    [CreateAssetMenu(fileName = "BattleData", menuName = "StaticData/BattleLevel")]
    public class BattleStaticData : ScriptableObject
    {
        public int Number;
        public GameObject Prefab;
    }
}
