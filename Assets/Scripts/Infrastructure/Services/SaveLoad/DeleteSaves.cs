using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.Services.PersistentProgress;
using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Scripts.Infrastructure.Services.SaveLoad
{
    public class DeleteSaves : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private IPersistenProgressService _progressService;

        public event Action DellEvent;

        private void OnEnable()
        {
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
            _button.onClick.AddListener(Dell);
            DellEvent += DelYandexSave;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Dell);
            DellEvent -= DelYandexSave;
        }

        private void Dell()
        {
            _progressService.Progress.SquadPlayer.SoldiersPlayer.Clear();
            _progressService.Progress.SquadPlayer.HeroTypeIds.Clear();
            _progressService.Progress.HeroesSquad.SoldierHerosSquad.Clear();
            _progressService.Progress.HeroesSquad.HeroTypeIds.Clear();
            DellEvent?.Invoke();
        }

        private void DelYandexSave()
        {
            YandexGame.ResetSaveProgress();
            YandexGame.SaveProgress();
        }
    }
}
