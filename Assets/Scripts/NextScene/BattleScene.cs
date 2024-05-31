using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.States;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.UIProgressReid;
using Source.Scripts.Logic;
using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Scripts.NextScene
{
    public class BattleScene : MonoBehaviour
    {
        public const string PlayFinger = "Finger";

        [SerializeField] private BookmarkButton _attakButton;
        [SerializeField] private FightNumber _flightNumber;
        [SerializeField] private Tutor _tutor;
        [SerializeField] private Image _image;

        private int _numberScene = 1;

        private IGameStateMachine _stateMachine;

        public event Action ChangedActivateScreen;

        public FightNumber FlightNumber => _flightNumber;

        private void OnEnable()
        {
            _attakButton.ButtonOnClic += LoadSceneBattle;
        }

        private void OnDisable()
        {
            _attakButton.ButtonOnClic -= LoadSceneBattle;
        }

        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
        }

        private void Start()
        {
            if (YandexGame.savesData.SelectionWindow)
            {
                ActivateAnimationFinger();
            }
        }

        public void ActivateAnimationFinger()
        {
            if (_tutor.IsActiveTutor == false && _flightNumber.CurrentNumber == 1)
            {
                _image.gameObject.SetActive(true);
            }
        }

        private void LoadSceneBattle()
        {
            Invoke("SceneLoad", 0.2f);
        }

        private void SceneLoad()
        {
            ChangedActivateScreen?.Invoke();
            _stateMachine.Enter<LoadLevelStateBattle, string, int>(AssetPath.SceneBattle, _numberScene);
        }

    }
}
