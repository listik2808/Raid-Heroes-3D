using Scripts.Army.TypesSoldiers;
using Scripts.BattleLogic.GameResult;
using Scripts.Logic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Enemy
{
    public class EnemyHeaith : MonoBehaviour
    {
         public float DamageMultiplayer = 1;

        [SerializeField] private Soldier _soldier;
        [SerializeField] private float _current;
        [SerializeField] private float _max;

        public Soldier Soldier => _soldier;

        public event Action HealthChanged;
        public event Action<float> OnHit;
        public event Action<Soldier> ChangedMaxHp;

        private void OnEnable()
        {
            _soldier.ChangedHp += SetHp;
            _soldier.ChangedHpBar += SetHp;
        }

        private void OnDisable()
        {
            _soldier.ChangedHp -= SetHp;
            _soldier.ChangedHpBar -= SetHp;
        }

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        public float Damage(float damage)
        {
            float currentDamage = damage / DamageMultiplayer; //* damage;
            if(currentDamage < 0)
            {
                currentDamage = Treatment(currentDamage);
            }
            else
            {
                currentDamage = Hit(currentDamage, damage);
            }
            
            
            if(_current < 0)
                _current = 0;

            if(_current > _max)
                _current = _max;
            OnHit?.Invoke(currentDamage);
            HealthChanged?.Invoke();
            if (currentDamage < 0)
                currentDamage *= -1;
            return currentDamage;
        }

        public void SetHp()
        {
            _max = Mathf.Ceil(_soldier.CurrentHealth);
            if(_current > _max)
            {
                _current = _max;
            }
            _current = Mathf.Ceil(_soldier.CurrentHealth);
            HealthChanged?.Invoke();
            ChangedMaxHp?.Invoke(_soldier);
        }

        private float Hit(float currentDamage,float damage)
        {
            if (_current < currentDamage)
            {
                currentDamage = _current;
                _current -= currentDamage;
                return currentDamage;
            }
            else
            {
                _current -= currentDamage;//DamageMultiplayer * damage;
            }
            return damage;
        }

        private float Treatment(float damage)
        {
            float currentHP = _current;
            currentHP -= DamageMultiplayer * damage;
            if (currentHP > _max)
            {
                float newDamage = _max - currentHP;
                damage-= newDamage;
                _current = _max;
                return damage;
            }
            else
            {
                _current -= DamageMultiplayer * damage;
            }
            return damage;
        }
    }
}