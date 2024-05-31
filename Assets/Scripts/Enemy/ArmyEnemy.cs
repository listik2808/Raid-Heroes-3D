using Scripts.Army.TypesSoldiers;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemy
{
    public class ArmyEnemy : MonoBehaviour
    {
        private List<Soldier> _soldiersEnemy = new List<Soldier>();

        public void AddArmy(List<Soldier> soldiers)
        {
            _soldiersEnemy = soldiers;
        }

        public void RemoveArmy()
        {
            _soldiersEnemy.Clear();
        }
    }
}