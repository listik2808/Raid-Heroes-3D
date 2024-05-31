using Scripts.Infrastructure.UIWindows.UIProgressReid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Logic
{
    public static class Price
    {
        public static int GetUpgradeCostDiamonds(float currentLevelStap,float bestLevel)
        {
            if(bestLevel <= currentLevelStap)
            {
                var value = Math.Round(1 + 2 * Math.Pow(currentLevelStap - 1, 2));
                return (int)value;
            }
            return 0;
        }

        public static float GetUpgradeCostCoin(float currentLevelSkill, float currentStep, float maxStep, int upgradeMultiplier, bool round)
        {
            float value = GetPowCurwe(currentLevelSkill, currentStep, maxStep, 10 * upgradeMultiplier, 1.3, round);
            return value;
        }

        public static float GetLastPVERewardGold(FightNumber flightNumber, float multiplicatator = 1, bool withRunes = false)
        {
            float value = (float)Math.Round(Math.Max(1, multiplicatator * GetPVERewardGold(flightNumber.CurrentNumber, withRunes)));
            return value;
        }

        private static float GetPowCurwe(float currentLevelSkill, float currentStep, float maxStep, int value, double coefficient, bool round)
        {
            var result = value * Math.Pow(coefficient, Gets(currentLevelSkill, currentStep, maxStep));
            if (round)
            {
                result = Math.Round(result);
                return (float)result;
            }
            return (float)result;
        }

        private static float Gets(float currentLevelSkill, float currentStep, float maxStep,float lowCoef = 0.25f)
        {
            return ((currentLevelSkill - 1) * (maxStep + 1) + currentStep) * lowCoef;
        }

        private static float GetPVERewardGold(int number, bool withRunes = false)
        {
            float value = 10 * (float)Math.Exp(0.1 * number);
            return value;
        }
    }
}
