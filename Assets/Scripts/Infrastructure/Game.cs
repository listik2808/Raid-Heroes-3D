using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.States;
using Scripts.Infrastructure.UIWindows.Screens;
using UnityEngine;

namespace Scripts.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;
        public Game(ICoroutineRunner coroutineRunner,LoadingCurtain Curtain)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner),AllServices.Container,Curtain);
        }
    }
}