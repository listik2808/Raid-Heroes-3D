using Scripts.Army.TypesSoldiers;
using Scripts.BattleLogic.GameResult;
using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;
using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Scripts.BattleTactics
{
    public class PlayerCell : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        private Soldier _soldier;
        private bool _isbusy = false;
        private int _id;

        public bool IsBusy => _isbusy;
        public int ID => _id;
        public Soldier Soldier => _soldier;
        public Collider Collider=> _collider;

        public void SetId(int id)
        {
            _id = id;
        }

        public void SetSoldier(Soldier soldier)
        {
            _soldier = soldier;
        }

        public Soldier Spawn(Soldier soldier)
        {
            Soldier hero = Instantiate(soldier, transform);
            HeroEnemyList.Heroes.Add(hero);
            _soldier = hero;
            _isbusy = true;
            _soldier.SetCell(this);
            return hero;
        }

        public void SetTransformPoint(Soldier soldier)
        {
            _soldier = soldier;
            _soldier.transform.position = new Vector3(transform.position.x,1f,transform.position.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_isbusy == false && _soldier == null)
            {
                if (other.TryGetComponent(out Soldier player))
                {
                    _soldier = player;
                    _isbusy = true;
                    _soldier.SetCell(this);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Soldier player))
            {
                if(_soldier != null)
                {
                    if(player == _soldier)//if (player.TypeSoldier == HeroType.Hero && player.HeroTypeId == _soldier.HeroTypeId)
                    {
                        _soldier = null;
                        _isbusy = false;
                    }
                }
            }
        }
    }
}