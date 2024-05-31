using Scripts.Army.TypesSoldiers;
using UnityEngine;

namespace Scripts.BattleTactics
{
    public class EnemyCell :MonoBehaviour
    {
        private bool _isBusy;
        private Soldier _soldierEnemy;
        public bool IsBusy => _isBusy;

        public Soldier Spawn(Soldier soldier)
        {
            _isBusy = true;
            Soldier enemy = Instantiate(soldier, transform);
            //enemy.transform.SetParent(null);
            _soldierEnemy = enemy;
            HeroEnemyList.Enemies.Add(enemy);
            return enemy;
        }
    }
}