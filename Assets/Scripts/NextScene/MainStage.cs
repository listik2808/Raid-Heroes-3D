using Scripts.BattleLogic.GameResult;
using Scripts.Data;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.States;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.Screens;
using System;
using UnityEngine;
using YG;

namespace Scripts.NextScene
{
    public class MainStage : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private BookmarkButton _button;
        [SerializeField] private BookmarkButton _buttonMainScene;
        [SerializeField] private BookmarkButton _nextBattle;
        [SerializeField] private BookmarkButton _restartBattle;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameResultsWatcher _gameResultsWatcher;
        [SerializeField] private BookmarkButton _tutorButton;
        private ScreenCardShow _screenCardShow;
        private int _namberZoneBattle =1;
        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;
        private IPersistenProgressService _persistenProgressService;
        private bool _returnSceneMain = false;
        private bool _isOpenCard = false;
        private bool _isRestart = false;
        private bool _isNext = false;
        private bool _isReturnMain = false;
        public CanvasGroup CanvasGroupMainStage => _canvasGroup;

        public event Action NextIdBattle;
        public event Action RestartLevelBattle;
        public event Action ReturnMainScene;
        public event Action ScreenClose;

        private void OnEnable()
        {
            _gameResultsWatcher.ChangetEnemyDaed += AddListenerButtons;
            _button.ButtonOnClic += LoadSceneBattle;
        }

        private void OnDisable()
        {
            _gameResultsWatcher.ChangetEnemyDaed -= AddListenerButtons;
            _button.ButtonOnClic -= LoadSceneBattle;
            _buttonMainScene.ButtonOnClic -= ReturnMain;
            _tutorButton.ButtonOnClic -= ReturnMain;
            _nextBattle.ButtonOnClic -= NextBattle;
            _restartBattle.ButtonOnClic -= RestartBattle;
        }

        private void AddListenerButtons(bool value)
        {
            
            _buttonMainScene.ButtonOnClic += ReturnMain;
            _tutorButton.ButtonOnClic += ReturnMain;
            _nextBattle.ButtonOnClic += NextBattle;
            _restartBattle.ButtonOnClic += RestartBattle;
        }


        public void LoadProgress(PlayerProgress progress)
        {
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_returnSceneMain)
                progress.WorldData.IsMainScren = false;
        }

        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _persistenProgressService = AllServices.Container.Single<IPersistenProgressService>();
        }

        public void CardIssued(int count)
        {
            if(count >= 0)
            {
                _isOpenCard = true;
            }
            else
            {
                _isOpenCard = false;
            }
        }

        public void LoadScene(ScreenCardShow screenCardShow)
        {
            _screenCardShow = screenCardShow;
            _screenCardShow.gameObject.SetActive(false);
            if (_isNext || _isRestart)
            {
                _saveLoadService.SaveProgress();
                _stateMachine.Enter<LoadLevelStateBattle, string, int>(AssetPath.SceneBattle, _namberZoneBattle);
            }
            else if (_isReturnMain)
            {
                _saveLoadService.SaveProgress();
                _stateMachine.Enter<LoadLevelState, string, int>(AssetPath.SceneMain, _persistenProgressService.Progress.PointSpawn.IdRaid);
            }
        }

        private void LoadSceneBattle()
        {
            Time.timeScale = 1.0f;
            _returnSceneMain = true;
            MainSceneLoad();
        }

        private void ReturnMain()
        {
            _isReturnMain = true;
            ReturnMainScene?.Invoke();
            MainSceneLoad();
        }

        private void NextBattle()
        {
            _isNext = true;
            NextIdBattle?.Invoke();
            Invoke("BattleScene", 0.2f);
            //BattleScene();
        }

        private void RestartBattle()
        {
            _isRestart = true;
            RestartLevelBattle?.Invoke();
            Invoke("BattleScene", 0.2f);
            //BattleScene();
        }

        private void BattleScene()
        {
            ClearListSoldiers();
            if (_returnSceneMain == false && _isOpenCard == false)
            {
                _saveLoadService.SaveProgress();
                _stateMachine.Enter<LoadLevelStateBattle, string, int>(AssetPath.SceneBattle, _namberZoneBattle);
            }
            else
            {
                ScreenClose?.Invoke();
                _gameResultsWatcher.Screen.gameObject.SetActive(false);
            }
        }

        private void MainSceneLoad()
        {
            ClearListSoldiers();
            if (_isOpenCard == false)
            {
                _saveLoadService.SaveProgress();
                _stateMachine.Enter<LoadLevelState, string, int>(AssetPath.SceneMain, _persistenProgressService.Progress.PointSpawn.IdRaid);
            }
            else
            {
                ScreenClose?.Invoke();
                _gameResultsWatcher.Screen.gameObject.SetActive(false);
            }
        }


        private void ClearListSoldiers()
        {
            YandexGame.FullscreenShow();
            HeroEnemyList.Heroes.Clear();
            HeroEnemyList.Enemies.Clear();
        }
    }
}
