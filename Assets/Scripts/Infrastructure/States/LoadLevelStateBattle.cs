using Scripts.Army.AllCadsHeroes;
using Scripts.Army.PlayerSquad;
using Scripts.BattleLogic.GameResult;
using Scripts.BattleTactics;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Factory;
using Scripts.Infrastructure.Player;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.Logic;
using Scripts.NextScene;
using UnityEngine;

namespace Scripts.Infrastructure.States
{
    public class LoadLevelStateBattle : IPayloadedStateInt<string, int>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistenProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly LoadingCurtain _loadingCurtain;
        private int _namberBattle;

        public LoadLevelStateBattle(GameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory, IPersistenProgressService progressService, IStaticDataService staticData, LoadingCurtain curtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _loadingCurtain = curtain;
        }

        public void Enter(string sceneName, int numberRaid)
        {
            _loadingCurtain.Show();
            _namberBattle = numberRaid;
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName, OnLoaded);
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
            _progressService.Progress.WorldData.Activiti = false;
            GameObject battle = _gameFactory.CreateBattle(_namberBattle);
            GameObject hud = _gameFactory.CreateHud();
            GameObject canvas = _gameFactory.CreateHudBattle();
            GameResultsWatcher resultWotcher = canvas.GetComponentInChildren<GameResultsWatcher>();
            HerosCards herosCards = hud.GetComponentInChildren<HerosCards>();
            ActivateSpawnerEnemySoldier zone = battle.GetComponentInChildren<ActivateSpawnerEnemySoldier>();
            ZoneCell zoneCell = battle.GetComponentInChildren<ZoneCell>();
            RaidZones raidZone = _gameFactory.CreateRaidZones();
            zone.Construct(raidZone, _gameFactory, resultWotcher,_progressService);
            RandomChanceCard randomChanceCard = hud.GetComponentInChildren<RandomChanceCard>();
            zoneCell.Constructor(resultWotcher, herosCards);
            MainStage stage = canvas.GetComponent<MainStage>();
            StartBattleButton startbattle = canvas.GetComponent<StartBattleButton>();
            startbattle.Construct(zoneCell, zone,_progressService);
            PreparingBattle hudBattle = canvas.GetComponent<PreparingBattle>();
            hudBattle.Construct(raidZone, stage, randomChanceCard,_progressService);
            UiWallet uiWallet = hud.GetComponentInChildren<UiWallet>();
            uiWallet.Construct(_progressService);
        }


        private void InformProgressReader()
        {
            foreach (ISavedProgessReader progessReader in _gameFactory.ProgessReaders)
                progessReader.LoadProgress(_progressService.Progress);
        }
    }
}