using Scripts.StaticData;
using System;
using UnityEngine;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public abstract class DataLevelSkill
    {
        public HeroTypeId TypeId;
        public int MaxLevelHero = 5;
        public int CountCardHiring = 5;
        public int CurrentLevelHero = -1;
        public int CurrentCountCard = 0;
        public float BestSurvivalRateLevel = 1;
        public float BestMeleerateLevel = 1;
        public float BestLevelSpecialSkill = 1;
        public float BestLevelMobility = 1;
        public float CurrenSurvivabilityLevel = 1;
        public float CurrenMeleeLevel = 1;
        public float CurrenSpecialAttackLevel = 1;
        public float CurrenSpeedLevel = 1;
        public float CurrentStepSurvivability = 0;
        public float CurrentStepMelee = 0;
        public float CurrentStepMobility = 0;
        public float CurrentStepSpecialAttack = 0;
        public float ValueSpeed =-1;
        public bool UnitOpened = false;
        public bool Hired = false;

        public void Reset()
        {
            CurrenSpecialAttackLevel = 1;
            CurrenSurvivabilityLevel = 1;
            CurrenMeleeLevel = 1;
            CurrenSpeedLevel = 1;
            CurrentStepSpecialAttack = 0;
            CurrentStepSurvivability = 0;
            CurrentStepMelee = 0;
            CurrentStepMobility = 0;
        }

        public float GetCurrentHealth()
        {
            float value = ((CurrenSurvivabilityLevel - 1) * 10) + CurrentStepSurvivability;
            if (CurrenSurvivabilityLevel > 1)
                value += CurrenSurvivabilityLevel - 1;
            return value;
        }

        public float GetCurrentSpecialSkill()
        {
            float value = ((CurrenSpecialAttackLevel * 10) - 10) + CurrentStepSpecialAttack;
            if (CurrenSpecialAttackLevel > 1)
                value += CurrenSpecialAttackLevel - 1;
            return value;
        }

        public float GetCurrentMeleeSkill()
        {
            float value = ((CurrenMeleeLevel * 10) - 10) + CurrentStepMelee;
            if (CurrenMeleeLevel > 1)
                value += CurrenMeleeLevel - 1;
            return value;
        }

        public float GetCurrentMobilitySkill()
        {
            float value = ((CurrenSpeedLevel * 10) - 10) + CurrentStepMobility;
            if (CurrenSpeedLevel > 1)
                value += CurrenSpeedLevel - 1;
            return value;
        }

        public int GetMaxCountCard()
        {
            int n = CountCardHiring;
            int level = MaxLevelHero + 1;
            for (int i = 0; i < level; i++)
            {
                n += 10 * i;
            }
            return n;
        }

        public int GetCurrentMaxCountCard()
        {
            int maxCard = GetMaxCountCard(); 
            int n = CountCardHiring;
            int level = CurrentLevelHero;
            if( level == -1)
            {
                return n;
            }
            else
            {
                level += 2;
                for (int i = 0; i < level; i++)
                {
                    n += 10 * i;
                }
                if(n > maxCard)
                {
                    n = maxCard;
                }
                return n;
            }
        }
    }
}
