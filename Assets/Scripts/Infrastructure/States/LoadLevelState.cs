using Scripts.Army.TypesSoldiers;
using Scripts.CameraMuve;
using Scripts.Infrastructure.Factory;
using Scripts.Infrastructure.Player;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.Logic;
using Scripts.RaidScreen;
using Scripts.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace Scripts.Infrastructure.States
{
    public class LoadLevelState : IPayloadedStateInt<string, int>
    {
        private const string EnemySpawner = "EnemySpawner";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistenProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly ISaveLoadService _saveLoadService;
        private readonly LoadingCurtain _loadingCurtain;
        private int _numberRaid;
        private MainScreen _screen;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory,IPersistenProgressService progressService,IStaticDataService staticData, ISaveLoadService saveLoadService,LoadingCurtain curtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _saveLoadService = saveLoadService;
            _loadingCurtain = curtain;
        }

        public void Enter(string sceneName,int numberRaid)
        {
            _loadingCurtain.Show();
            _numberRaid = numberRaid;
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName,OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReader();
            _stateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            if (_numberRaid == 0)
            {
                _numberRaid = 1;
                int index = 0;
                //_progressService.Progress.KillData.ClearedSpawners.Clear();
                foreach (int id in _progressService.Progress.KillData.ClearedSpawners)
                {
                    index = id;
                }
                _progressService.Progress.PointSpawn.IdSpawnerEnemy = index + 1;
            }

            
            _progressService.Progress.WorldData.Activiti = true;
            RaidZones raidZones = _gameFactory.CreateRaidZones();
            GameObject raid = _gameFactory.CreateRaid(_numberRaid);
            GameObject hud = _gameFactory.CreateHud();
            TransferObjects(raid, hud,raidZones,_numberRaid);
            InitSpawners(raid);
        }

        private void TransferObjects(GameObject raid, GameObject hud,RaidZones raidZones,int number)
        {
            RaidsObject target = raid.GetComponent<RaidsObject>();
            _screen = hud.GetComponentInChildren<MainScreen>();
            _screen.SetComponetRaid(target, raidZones,number,_saveLoadService,_gameFactory,_progressService);
        }

        private void InitSpawners(GameObject raid)
        {
            LevelStaticData levelData = _staticData.ForLevel(_numberRaid.ToString());
            if(levelData != null)
            {
                foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
                {
                    SpawnPoint pointer = _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId,spawnerData.IdRaid, _screen);
                    pointer.transform.parent = raid.transform;
                }
            }
        }

        private void InformProgressReader()
        {
            foreach (ISavedProgessReader progessReader in _gameFactory.ProgessReaders)
                progessReader.LoadProgress(_progressService.Progress);
        }
    }
}