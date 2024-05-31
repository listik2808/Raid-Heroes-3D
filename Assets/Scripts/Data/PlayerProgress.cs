using Assets.Scripts.Economics.ArenaPasses;
using Assets.Scripts.Economics.Portals;
using System;

namespace Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public KillData KillData;
        public SquadPlayer SquadPlayer;
        public Training Training;
        public PlayerCellData PlayerCellData;
        public PriceLevelHeroCard PriceLevelHeroCard;
        public HeroesSquad HeroesSquad;
        public PointSpawn PointSpawn;
        public Wallet Wallet;
        public Gems Gems;
        public Portals Portals;
        public Passes ArenaPasses;
        public Shop Shop;
        public PlayerData PlayerData;
        public OptionData OptionData;
        public Achievements Achievements;

        public PlayerProgress(string initialLevel,int numberRaid)
        {
            WorldData = new WorldData(initialLevel,numberRaid);
            KillData = new KillData();
            SquadPlayer = new SquadPlayer();
            Training = new Training();
            PlayerCellData = new PlayerCellData();
            PriceLevelHeroCard = new PriceLevelHeroCard();
            HeroesSquad = new HeroesSquad();
            PointSpawn = new PointSpawn();
            Wallet = new Wallet();
            Gems = new Gems();
            ArenaPasses = new Passes();
            Portals = new Portals();
            Shop = new Shop();
            PlayerData = new PlayerData();
            OptionData = new OptionData();
            Achievements = new Achievements();
        }
    }
}
