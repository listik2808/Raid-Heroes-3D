using System;
using System.Collections.Generic;

namespace Scripts.Economics.Buildings
{
    [Serializable]
    public class Building
    {
        public Castle Castle;
        public Market Market;
        public GoldMine GoldMine;
        public CrystalMine CrystalMine;
        public Arena Arena;
        public Barracks Barracks;
        public Tavern Tavern;
        public HeroesHall HeroesHall;
        public MagicTower MagicTower;
        public Lab Lab;
        public Forge Forge;
        public Palace Palace;

        public Building() 
        {
            Castle = new Castle();
            Market = new Market();
            GoldMine = new GoldMine();
            CrystalMine = new CrystalMine();
            Arena = new Arena();
            Barracks = new Barracks();
            Tavern = new Tavern();
            HeroesHall = new HeroesHall();
            MagicTower = new MagicTower();
            Lab = new Lab();
            Forge = new Forge();
            Palace = new Palace();
        }
    }
}
