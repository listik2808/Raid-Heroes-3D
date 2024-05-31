using UnityEngine;

namespace Scripts.StaticData
{
    [CreateAssetMenu(fileName = "RaidData", menuName = "StaticData/RaidLevel")]
    public class RaidStaticData : ScriptableObject
    {
        public int Number;
        public GameObject Prefab;
    }
}
