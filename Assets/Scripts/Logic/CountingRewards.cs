using Assets.Scripts.Economics;
using System;
using UnityEngine;

namespace Scripts.Logic
{
    public static class CountingRewards
    {
        public const int RewardCoef1 = 10;
        public const float RewardCoef2 = 0.1f;

        public static float GetPveRewardGold(int namber)
        {
            float value = RewardCoef1 * (float)Math.Exp(RewardCoef2 * namber);
            return Mathf.Round(value);
        }
    }
}
