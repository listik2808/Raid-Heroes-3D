using Scripts.Army;
using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Player;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Logic;
using Scripts.StaticData;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Profiling;

namespace Scripts.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAsset _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistenProgressService _progressService;
        private MonsterStaticData _monsterData;
        private GameObject _monster;
        public RaidZones RaidZones;

        public List<SpawnPoint> SpawnPoints { get; } = new List<SpawnPoint>();
        public List<ISavedProgessReader> ProgessReaders { get; } = new List<ISavedProgessReader>();
        public List<ISavedProgress> ProgessWriters { get; } = new List<ISavedProgress>();

        public GameFactory(IAsset assets, IStaticDataService staticData,IPersistenProgressService persistenProgressService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = persistenProgressService;
        }

        public GameObject CreateHud()
        {
            GameObject gameObject = InstantiateRegistered(AssetPath.HudPath);
            return gameObject;
        }

        public GameObject CreateHudBattle()
        {
            GameObject gameObject = InstantiateRegistered(AssetPath.HudBattle);
            return gameObject;
        }

        public RaidZones CreateRaidZones()
        {
            GameObject gameObject = InstantiateRegistered(AssetPath.ReideZonePath);
            RaidZones = gameObject.GetComponent<RaidZones>();
            return RaidZones;
        }

        public GameObject CreateRaid(int number)
        {
            RaidStaticData raidData = _staticData.ForRaidLevel(number);
            GameObject raid = Object.Instantiate(raidData.Prefab);
            return raid;
        }

        public GameObject CreateBattle(int number)
        {
            BattleStaticData battleData = _staticData.ForBattleLevel(number);
            GameObject battle = Object.Instantiate(battleData.Prefab);
            RegisterProgressWatchers(battle);
            return battle;
        }

        public void Cleanup()
        {
            ProgessReaders.Clear();
            ProgessWriters.Clear();
            SpawnPoints.Clear();
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgessReader progessReader in gameObject.GetComponentsInChildren<ISavedProgessReader>())
                Register(progessReader);
        }

        public void Register(ISavedProgessReader progessReader)
        {
            if (progessReader is ISavedProgress progressWrite)
                ProgessWriters.Add(progressWrite);

            ProgessReaders.Add(progessReader);
        }

        public GameObject CreateModelMonster(MonsterTypeId typeId, Transform parent,int id,int idRaid, MainScreen mainScreen)
        {
            _monster = Instantiate(typeId, parent, out _monsterData, out _monster);
            TroopsEnemy troops = _monster.GetComponentInChildren<TroopsEnemy>();
            foreach (var item in RaidZones.ListRaids)
            {
                foreach (var item2 in item.SpawnMarkers)
                {
                    if(item2.Id == id)
                    {
                        troops.AddSolder(item2.ArmyEnemySoldiers, id, idRaid,SpawnPoints,mainScreen,RaidZones.CameraParentEnemies);
                    }
                }
            }
            return _monster;
        }
        public SpawnPoint CreateSpawner(Vector3 at, int spawnerId, MonsterTypeId monsterTypeId, int idRaid, MainScreen mainScreen)
        {
            SpawnPoint spawnPoint = InstantiateRegistered(AssetPath.Spawner, at).GetComponent<SpawnPoint>();
            spawnPoint.Construct(this,mainScreen);
            spawnPoint.Id = spawnerId;
            spawnPoint.IdRaid = idRaid;
            spawnPoint.MonsterTypeId = monsterTypeId;
            SetCountSpawners(spawnPoint);
            return spawnPoint;
        }

        public void SetCountSpawners(SpawnPoint pointer)
        {
            SpawnPoints.Add(pointer);
        }

        private GameObject Instantiate(MonsterTypeId typeId, Transform parent, out MonsterStaticData monsterData, out GameObject monster)
        {
            monsterData = _staticData.ForMonster(typeId);
            monster = Object.Instantiate(monsterData.Prefab);
            monster.transform.position = parent.transform.position;
            monster.transform.SetParent(parent.transform);
            return monster;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath, at: at);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }
    }
}