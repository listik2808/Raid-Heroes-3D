using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.States;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;

namespace Scripts.CastleGuards
{
    public class SceneMenu : MonoBehaviour
    {
        private const string SceneMain = "Main";

        [SerializeField] private BookmarkButton _buttonGoHom;
        [SerializeField] private BookmarkButton _buttonYes;
        private int _numberScene = 1;

        private IGameStateMachine _stateMachine;

        private void OnEnable()
        {
            _buttonGoHom.ButtonOnClic += LoadSceneBattle;
            _buttonYes.ButtonOnClic += LoadSceneBattle;
        }

        private void OnDisable()
        {
            _buttonGoHom.ButtonOnClic -= LoadSceneBattle;
            _buttonYes.ButtonOnClic -= LoadSceneBattle;
        }

        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
        }

        private void LoadSceneBattle()
        {
            _stateMachine.Enter<LoadLevelState, string, int>(SceneMain, _numberScene);
        }
    }
}
