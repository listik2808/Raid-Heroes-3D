using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using YG;

namespace Scripts.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private const string MainScene = "Main";
        private const int NumberRaidStart = 1;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistenProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistenProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            if(_saveLoadService.Chek() == true)
            {
                YandexGame.ResetSaveProgress();
                YandexGame.savesData.CorectSave = true;
            }
            else
            {
                LoadProgressOrInitNew();
            }
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();
            _gameStateMachine.Enter<LoadLevelState, string, int>
            (_progressService.Progress.WorldData.PositionOnLevel.Level, _progressService.Progress.PointSpawn.IdRaid);
        }

        private PlayerProgress NewProgress() =>
            new PlayerProgress(initialLevel: MainScene, NumberRaidStart);
    }
}