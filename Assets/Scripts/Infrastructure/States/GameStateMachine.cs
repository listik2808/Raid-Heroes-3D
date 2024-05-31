using Scripts.Infrastructure.Factory;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, AllServices services , LoadingCurtain Curtain)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, services.Single<IGameFactory>(), services.Single<IPersistenProgressService>(),services.Single<IStaticDataService>(),services.Single<ISaveLoadService>(),Curtain),
                [typeof(LoadLevelStateBattle)] = new LoadLevelStateBattle(this, sceneLoader, services.Single<IGameFactory>(), services.Single<IPersistenProgressService>(), services.Single<IStaticDataService>(),Curtain),
                [typeof(LoadLevelStateCastle)] = new LoadLevelStateCastle(this, sceneLoader, services.Single<IGameFactory>(), services.Single<IPersistenProgressService>(), services.Single<IStaticDataService>(),Curtain),
                [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IPersistenProgressService>(), services.Single<ISaveLoadService>()),
                [typeof(GameLoopState)] = new GameLoopState(this),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void Enter<TState, TPayload, TPayLoadInt>(TPayload payload, TPayLoadInt payLoadInt) where TState : class, IPayloadedStateInt<TPayload,TPayLoadInt>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload,payLoadInt);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }


        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}