using Scripts.Logic.TaskAchievements;
using Scripts.StaticData;
using System;
using System.Collections.Generic;

namespace Scripts.Data
{
    [Serializable]
    public class Achievements
    {
        public float AchievementRaidWinIndexId = 0;
        public float AllCountFightNumber = 0;

        public float AchievementGetCardHeroIndexId = 0;
        public int AchievementAllCountCard = 0;

        public float AchievementHireHeroCount = 9;
        public List<HeroTypeId> HeroTypeIds = new List<HeroTypeId>();

        public float AchievementGetCoinsIndexId = 0;
        public float AllCoinsAch = 0;

        public float AchievementGetDiamondsIndexId = 0;
        public float AllDiamonds = 0;

        public float AchievementGetStarsIndexId = 0;
        public float AllStars = 0;

        public float AllUpHeroSkill =0;
        public float AchievementGetUpSkillHero = 0;

        public float AllUpEvolutionSkill = 0;
        public float AchievementGetUpEvolutionHero = 48;

        public float AllValueRankAch = 0;
        public float AchievementGetUpRank = 9;

        public float AllValueClearTerritoriesAch = 0;
        public float AchievementGetCountClierTerritories = 1;

        public float AllValueBuildBuildingAch = 0;
        public float AhievementGetCountBuilding = 0;

        public float AllValueImpruvementBuilding = 0;
        public float AchievementGetCountImpruvementBuilding = 77;

        public float AllRaidCountReachRaidAch = 0;
        public float AhievementGetCountRaidCountReachRaid = 0;

        public float AllNewCountRaid = 0;
        public float AchievementGetNewNamberRaid = 95;

        public float ProgressRaid = 0;

        public float AchievementGetCountWatchCommercials = 0;
        public float AllCountShowCommercials = 0;

        public float AchievementGetCountNumberPurchases = 0;
        public float NumberPurchasesAchievement = 0;
    }
}