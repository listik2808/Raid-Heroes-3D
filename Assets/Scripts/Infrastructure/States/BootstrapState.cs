using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Factory;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.StaticData;
using System;
using System.Diagnostics;
using TMPro;

namespace Scripts.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Initial,EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            RegisterStaticData();
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            _services.RegisterSingle<IAsset>(new AssetProvider());
            _services.RegisterSingle<IPersistenProgressService>(new PersistenProgressService());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAsset>(),_services.Single<IStaticDataService>(),_services.Single<IPersistenProgressService>()));
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistenProgressService>(), _services.Single<IGameFactory>()));
        }

        private void RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle(staticData);
        }
    }
}