using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public  class TypeHero
    {
        public AllHerosType AllHerosType;
        public BarbarianHero BarbarianHero;
        public KonungHero KonungHero;
        public ArcherHero ArcherHero;
        public AssassinHero AssassinHero;
        public BerserkHero BerserkHero;
        public CharmerHero CharmerHero;
        public CommanderHero CommanderHero;
        public ElfHero ElfHero;
        public FairyHero FairyHero;
        public GreenArrowHero GreenArrowHero;
        public KingHero KingHero;
        public KnightHero KnightHero;
        public PoisonerHero PoisonerHero;
        public PriestHero PriestHero;
        public PrincessHero PrincessHero;
        public StalkerHero StalkerHero;
        public SubzeroHero SubzeroHero;
        public VikingHero VikingHero;
        public WarriorHero WarriorHero;
        public WitcherBlueHero WitcherBlueHero;
        public WitcherGreenHero WitcherGreenHero;
        public WitcherRedHero WitcherRedHero;
        public TeleportSoldierHero TeleportSoldierHero;
        public HailArrowsSoldierHero HailArrowsSoldierHero;
        public FrostArrowSoldierHero FrostArrowSoldierHero;
        public InvisibilitySoldierHero InvisibilitySoldierHero;
        public AuraSoldierHero AuraSoldierHero;
        public BreakingSoldierHero BreakingSoldierHero;

        public TypeHero()
        {
            AllHerosType = new AllHerosType();
            BarbarianHero = new BarbarianHero(AllHerosType);
            KonungHero = new KonungHero(AllHerosType);
            ArcherHero = new ArcherHero(AllHerosType);
            AssassinHero = new AssassinHero(AllHerosType);
            BerserkHero = new BerserkHero(AllHerosType);
            CharmerHero = new CharmerHero(AllHerosType);
            CommanderHero = new CommanderHero(AllHerosType);
            ElfHero = new ElfHero(AllHerosType);
            FairyHero = new FairyHero(AllHerosType);
            GreenArrowHero = new GreenArrowHero(AllHerosType);
            KingHero = new KingHero(AllHerosType);
            KnightHero = new KnightHero(AllHerosType);
            PoisonerHero = new PoisonerHero(AllHerosType);
            PriestHero = new PriestHero(AllHerosType);
            PrincessHero = new PrincessHero(AllHerosType);
            StalkerHero = new StalkerHero(AllHerosType);
            SubzeroHero = new SubzeroHero(AllHerosType);
            VikingHero = new VikingHero(AllHerosType);
            WarriorHero = new WarriorHero(AllHerosType);
            WitcherBlueHero = new WitcherBlueHero(AllHerosType);
            WitcherGreenHero = new WitcherGreenHero(AllHerosType);
            WitcherRedHero = new WitcherRedHero(AllHerosType);
            TeleportSoldierHero = new TeleportSoldierHero();
            HailArrowsSoldierHero = new HailArrowsSoldierHero();
            FrostArrowSoldierHero = new FrostArrowSoldierHero();
            InvisibilitySoldierHero = new InvisibilitySoldierHero();
            AuraSoldierHero = new AuraSoldierHero();
            BreakingSoldierHero = new BreakingSoldierHero();
        }
    }
}
