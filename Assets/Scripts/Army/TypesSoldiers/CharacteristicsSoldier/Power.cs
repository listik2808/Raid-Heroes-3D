using Scripts.StaticData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.CharacteristicsSoldier
{
    public class Power : MonoBehaviour
    {
        private float _damage;
        private float _damageDelay;
        private float _spec;
        private float _specDelay;
        private float _specTime;
        private float _health;
        private float _rangeTarget;
        private float _maxHealth;
        private float _dps;
        private float _dpsSpec;
        private float _time;
        private float _power;

        private float _pvePowerA = 1000;
        private float _pvePowerB = 1.105f;

        public float PowerSoldier => _power;

        public event Action Finish;

        public float GetUnitDPS (Soldier soldier)
	    {
            _damage = soldier.CurrentMeleeDamage;
            _damageDelay = soldier.DataSoldier.DurationRecoveryMeleeDamage;
            _spec = soldier.CurrenValueSpecAttack;
            _specDelay = soldier.DurationRecoverySpecAttack;
            _specTime = soldier.TimeSpecialSkill;
            _health = soldier.DataSoldier.BaseHealthValue;
            _maxHealth = soldier.CurrentHealth;
            _rangeTarget = soldier.DataSoldier.RangeAttack;// было в оригенале 60 у всех

            _dps = _damage / _damageDelay;
            _dpsSpec = _spec / _specDelay;
            _time = _specTime / _specDelay;

            if (soldier.SpecialAttack == SpecialAttack.Fireball || soldier.SpecialAttack == SpecialAttack.Lightning || soldier.SpecialAttack == SpecialAttack.Shoot)
                _dps += _dpsSpec * 20 / 95 / 95 * _rangeTarget;

            if (soldier.SpecialAttack == SpecialAttack.HailArrows)
                _dps += _dpsSpec * 40 / 95 / 95 * _rangeTarget;

            if (soldier.SpecialAttack == SpecialAttack.WildArrow)
                _dps += _dpsSpec * 40 / 95 / 95 * _rangeTarget;

            if (soldier.SpecialAttack == SpecialAttack.Stun|| soldier.SpecialAttack == SpecialAttack.Freeze || soldier.SpecialAttack == SpecialAttack.Teleportation)
            {
                _dps += _dpsSpec;
                _dps /= (1 - _time);
            }

            if (soldier.SpecialAttack == SpecialAttack.StunCircle || soldier.SpecialAttack == SpecialAttack.Breakthrough)
            {
                _dps += 2 * _dpsSpec;
                _dps /= (1 - _time);
            }

            if (soldier.SpecialAttack == SpecialAttack.Heal || soldier.SpecialAttack == SpecialAttack.Aura)
                _dps += _dpsSpec;

            if (soldier.SpecialAttack == SpecialAttack.DoubleAttack)
                _dps += 2 * _dpsSpec;

            if (soldier.SpecialAttack == SpecialAttack.Swap)
                _dps += 4 * _dpsSpec;

            if (soldier.SpecialAttack == SpecialAttack.Hypnosis)
            {
                _dps *= (1 + 1800 / _health / soldier.CurrentMeleeDamage * soldier.DataSoldier.DurationRecoveryMeleeDamage * _time) / (1 - _time);
            }

            if (soldier.SpecialAttack == SpecialAttack.Toxic)
            {
                var dpsSpec = 3 * _spec / _specDelay;
                _dps += dpsSpec * (1 + 20 / _health / _health * _rangeTarget);
            }

            if (soldier.SpecialAttack == SpecialAttack.Motivation|| soldier.SpecialAttack == SpecialAttack.MotivationAll|| soldier.SpecialAttack == SpecialAttack.Rage)
            {
                var m = _spec - 1;
                _dps *= (1 + m * _time) / (1 - m / (1 + m) * _time);
            }

            if (soldier.SpecialAttack == SpecialAttack.BattleCry)
                _dps *= (1 + 4 * _spec * _spec / (_specDelay / _specTime - 1));

            if (soldier.SpecialAttack == SpecialAttack.Revealing || soldier.SpecialAttack == SpecialAttack.Invisibility)
                _dps *= (1 + _spec * _time);

            if (soldier.SpecialAttack == SpecialAttack.Raise)
            {
                float h = 0;
                float d = 0;

                if (soldier.TryGetComponent(out Priest priest))
                {
                    h = priest.CurrentHealthSkeleta;
                    d = priest.CurrenValueSpecAttack;
                }

                _dps *= (1 + d / _dps) * (1 + h / _maxHealth);
            }

            if (soldier.SpecialAttack == SpecialAttack.IceArrow)
                _dps *= (float)(1 + 3.2 * _spec * _rangeTarget / _maxHealth / 90 / (_specDelay - _specTime));

            _dps = soldier.CurrentHealth * _dps;
            _dps = soldier.RoundUp(_dps, 2);
            _dps = Math.Abs(_dps);

            return _dps;
        }

        public float GetSquadPower(List<Soldier> soldiers)
        {
            float valuePower = 0;
            float squadLength = soldiers.Count;
            foreach (var soldier in soldiers)
            {
                valuePower += soldier.Power.GetUnitDPS(soldier);
            }

            float coefficient = 1;
            if (squadLength == 2)
                coefficient = 0.75f;
            if (squadLength == 3)
                coefficient = 0.66f;
            if (squadLength == 4)
                coefficient = 0.625f;
            if (squadLength == 5)
                coefficient = 0.6f;
            if (squadLength == 6)
                coefficient = 0.583f;
            if (squadLength == 7)
                coefficient = 0.57f;
            if (squadLength == 8)
                coefficient = 0.56f;
            if (squadLength == 9)
                coefficient = 0.553f;
            if (squadLength == 10)
                coefficient = 0.55f;

            return valuePower * squadLength * coefficient;
        }

        public float GetPower(int number,bool value = true)
        {
            float s = 1.125f;
            if (value) s = number < 10 ? 1 : (1 + 0.25f * (number % 10) / 9);
            return _pvePowerA * s * (float)(Math.Pow(_pvePowerB, number + 1));
        }

        public IEnumerator UpLevelEnemy(List<Soldier> soldiers,float maxPower)
        {
            float power = 0;
            int index = 0;
            int count = 0;
            int valueLevel = 0;

            power = GetSquadPower(soldiers);
            if (power < maxPower)
            {
                while (power < maxPower)
                {
                    if (count == 0)
                    {
                        valueLevel = soldiers[index].SpecialSkillLevelData.AddStepEnemy();
                        soldiers[index].SoldiersStatsLevel.AddEnemyLevelSpecialSkill(valueLevel);
                        soldiers[index].SpecialSkillUpgrade();
                    }
                    if (count == 1)
                    {
                        valueLevel = soldiers[index].SurvivabilityLevelData.AddStepEnemy();
                        soldiers[index].SoldiersStatsLevel.AddenemyHealthLevel(valueLevel);
                        soldiers[index].SurvivabilityLevelData.SkillSurvivabikityUpgrade(soldiers[index]);
                    }
                    if (count == 2)
                    {
                        valueLevel = soldiers[index].MeleeDamageLevelData.AddStepEnemy();
                        soldiers[index].SoldiersStatsLevel.AddEnemyMeleeLevel(valueLevel);
                        soldiers[index].MeleeDamageLevelData.SkillMeleeDamageUpgrade(soldiers[index]);
                    }

                    index++;
                    if (index == soldiers.Count)
                    {
                        index = 0;
                        count++;
                        if (count == 3) count = 0;
                    }

                    power = GetSquadPower(soldiers);
                    _power = power;
                    yield return null;
                }
            }
            _power = power;
            Finish?.Invoke();
        }
    }
}
