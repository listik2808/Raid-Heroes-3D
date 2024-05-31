using Scripts.Logic.ShowingSoldierData;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Army.PlayerSquad
{
    public class ActivUpgradeSoldier : MonoBehaviour
    {
        [SerializeField] private List<UpgradeData> _upgradeDatas = new List<UpgradeData>();

        public List<UpgradeData> UpgradeDatas => _upgradeDatas;
    }
}
