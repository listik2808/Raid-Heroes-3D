using Scripts.Army.TypesSoldiers;
using System;
using UnityEngine;

namespace Scripts.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHeaith _health;

        public event Action Happened;

        private void Start()
        {
            _health.HealthChanged += HealthChanged;
            Happened += Die;
        }

        private void HealthChanged()
        {
            if (_health.Current <= 0)
                Happened?.Invoke();
        }

        private void Die()
        {
             
        }
    }
}