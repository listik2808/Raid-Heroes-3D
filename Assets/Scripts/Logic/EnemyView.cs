using Scripts.StaticData;
using UnityEngine;

namespace Scripts.Logic
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private MonsterTypeId _monsterTypeId;

        public MonsterTypeId MonsterTypeId => _monsterTypeId;
    }
}