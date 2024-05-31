using Scripts.StaticData;
using UnityEngine;

namespace Scripts.Logic
{
    public class HeroView : MonoBehaviour
    {
        [SerializeField] private HeroTypeId _heroTypeId;

        public HeroTypeId HeroTypeId => _heroTypeId;
    }
}