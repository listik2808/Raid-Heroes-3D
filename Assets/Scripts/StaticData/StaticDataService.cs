using Scripts.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<int, RaidStaticData> _raidLevel;
        private Dictionary<int, BattleStaticData> _battle;
        public void Load()
        {
            _monsters = Resources.LoadAll<MonsterStaticData>("StaticData/Monsters").ToDictionary(x => x.MonsterTypeId, x => x);
                
            _levels = Resources.LoadAll<LevelStaticData>("StaticData/Levels").ToDictionary(x => x.LevelKey, x => x);

            _raidLevel = Resources.LoadAll<RaidStaticData>("StaticData/LevelRaid").ToDictionary(x => x.Number, x => x);

            _battle = Resources.LoadAll<BattleStaticData>("StaticData/LevelBattle").ToDictionary(x => x.Number, x => x);
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
            _monsters.TryGetValue(typeId, out MonsterStaticData staticData) ? staticData : null;

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;

        public RaidStaticData ForRaidLevel(int number) =>
            _raidLevel.TryGetValue(number, out RaidStaticData staticData) ? staticData : null;

        public BattleStaticData ForBattleLevel(int number) =>
            _battle.TryGetValue(number, out BattleStaticData staticData) ? staticData : null;
    }
}
