using Scripts.StaticData;

namespace Scripts.Infrastructure.Services
{
    public interface IStaticDataService : IService
    {
        MonsterStaticData ForMonster(MonsterTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
        RaidStaticData ForRaidLevel(int number);
        BattleStaticData ForBattleLevel(int number);
        void Load();
    }
}