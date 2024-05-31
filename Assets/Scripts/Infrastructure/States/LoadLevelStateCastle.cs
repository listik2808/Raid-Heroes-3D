using Scripts.Infrastructure.Factory;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Infrastructure.States
{
    public class LoadLevelStateCastle : IPayloadedStateInt<string, int>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistenProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly LoadingCurtain _curtain;
        private int _namberBattle;

        public LoadLevelStateCastle(GameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory, IPersistenProgressService progressService, IStaticDataService staticData,LoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _curtain = loadingCurtain;
        }

        public void Enter(string sceneName, int numberRaid)
        {
            _curtain.Show();
            _namberBattle = numberRaid;
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName,OnLoaded);
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            //InitGameWorld();
            InformProgressReader();
            _stateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
        }


        private void InformProgressReader()
        {
            foreach (ISavedProgessReader progessReader in _gameFactory.ProgessReaders)
                progessReader.LoadProgress(_progressService.Progress);
        }
    }
}