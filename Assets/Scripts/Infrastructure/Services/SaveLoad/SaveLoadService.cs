using Newtonsoft.Json;
using Scripts.Data;
using Scripts.Infrastructure.Factory;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using YG;

namespace Scripts.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        public const string PlayerDataKey = "PlayerData";

        private readonly IPersistenProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        private string _json;

        public SaveLoadService(IPersistenProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
            //SaveOrLoad.Init();
        }

        public PlayerProgress LoadProgress()
        {

            _json = YandexGame.savesData.JsSave;

            _progressService.Progress = JsonUtility.FromJson<PlayerProgress>(_json);
            return _progressService.Progress;
        }

        public void SaveProgress()
        {
            foreach(ISavedProgress progressWriter in _gameFactory.ProgessWriters)
                progressWriter.UpdateProgress(_progressService.Progress);

            _json = JsonUtility.ToJson(_progressService.Progress);
            YandexGame.savesData.JsSave = _json;
            YandexGame.SaveProgress();
        }

        public bool Chek()
        {
            if (YandexGame.savesData.SelectionWindow == false && YandexGame.savesData.CorectSave == false)
            {
                _json = null;
                return true;
            }
            return false;
        }
    }
}
