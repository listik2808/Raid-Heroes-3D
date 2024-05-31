using Scripts.Data;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Factory;
using Scripts.Infrastructure.Player;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.NextScene;
using Scripts.RaidScreen;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Infrastructure.UIWindows.ScreenNavigation
{
    public class MainScreen : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private BattleScene _battleScene;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ListEnemyUnits _listEnemyUnits;
        [SerializeField] private CanvasRaid _canvasRaid;
        [SerializeField] private CanvasGroup _canvasGroupScreensMain;
        [SerializeField] private OptionsTab _optionsTab;
        [SerializeField] private CanvasGroup _screenRaidCanvasGroup;
        [SerializeField] private ScreenTask _screenTask;
        [SerializeField] private UiWallet _uiWallet;
        [SerializeField] private WarSelectionScreen _warSelectionScreen;
        private RaidZones _raidZones;
        private RaidsObject _raidsObject;
        private int _numberRaid;
        private bool _diactivate =false;
        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;
        private IPersistenProgressService _persistenProgressService;

        public IPersistenProgressService PersistenProgressService => _persistenProgressService;
        public ISaveLoadService SaveLoadService => _saveLoadService;
        public bool Diactivate => _diactivate;
        public BattleScene BattleScene => _battleScene;
        public RaidsObject RaidObject => _raidsObject;
        public RaidZones RaidZones => _raidZones;
        public CanvasRaid CanvasRaid => _canvasRaid;
        public CanvasGroup CanvasGroup => _canvasGroupScreensMain;
        public CanvasGroup ScreenRaidCanvasGroup => _screenRaidCanvasGroup;
        public OptionsTab CanvasOption => _optionsTab;


        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.IsMainScren = _diactivate;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _diactivate = progress.WorldData.IsMainScren;
            if (_diactivate && progress.WorldData.Activiti == false)
            {
                _canvas.gameObject.SetActive(false);
            }
            else if (progress.WorldData.Activiti == true && _diactivate == false)
            {
                _canvas.gameObject.SetActive(true);
                
            }
        }

        private void OnEnable()
        {
            _battleScene.ChangedActivateScreen += OffMainScreen;
            _canvasRaid.Open += OpenRaid;
            _canvasRaid.Close += CloseRaid;
        }

        private void OnDisable()
        {
            _battleScene.ChangedActivateScreen -= OffMainScreen;
            _canvasRaid.Open -= OpenRaid;
            _canvasRaid.Close -= CloseRaid;
        }

        public void SetComponetRaid(RaidsObject raidsObject,RaidZones raidZones,int numberRaid,ISaveLoadService saveLoadService,IGameFactory gameFactory,IPersistenProgressService persistenProgressService)
        {
            _raidsObject = raidsObject;
            _raidZones = raidZones;
            _numberRaid = numberRaid;
            _gameFactory = gameFactory;
            _listEnemyUnits.Construct(_gameFactory);
            _saveLoadService = saveLoadService;
            _persistenProgressService = persistenProgressService;
            _warSelectionScreen.Construct(saveLoadService, _persistenProgressService);
            _uiWallet.Construct(_persistenProgressService);
            _screenTask.Initialize(this,_persistenProgressService);
        }

        private void CloseRaid()
        {
            if(_raidsObject != null)
                _raidsObject.gameObject.SetActive(false);
        }

        private void OpenRaid()
        {
            if (_raidsObject !=  null)
                _raidsObject.gameObject.SetActive(true);
        }

        private void OffMainScreen()
        {
            if(PersistenProgressService.Progress.PointSpawn.IdRaid == 0)
            {
                PersistenProgressService.Progress.PointSpawn.IdRaid = 1;
            }
            _diactivate = true;
            _canvas.gameObject.SetActive(false);
            _saveLoadService.SaveProgress();
        }
    }
}