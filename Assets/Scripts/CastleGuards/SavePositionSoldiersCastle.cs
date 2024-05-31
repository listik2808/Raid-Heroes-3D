using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.CastleGuards
{
    public class SavePositionSoldiersCastle : MonoBehaviour
    {
        [SerializeField] private BookmarkButton _saveButtonProgress;
        [SerializeField] private Image _homecomingScreen;
        [SerializeField] private BookmarkButton _buttonNo;

        private ISaveLoadService _saveLoadService;

        private void OnEnable()
        {
            _saveButtonProgress.ButtonOnClic += Save;
            _buttonNo.ButtonOnClic += CloseScreenHome;
        }

        private void OnDisable()
        {
            _saveButtonProgress.ButtonOnClic -= Save;
            _buttonNo.ButtonOnClic -= CloseScreenHome;
        }

        private void Awake()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void Save()
        {
            //_saveLoadService.SaveProgress();
            _homecomingScreen.gameObject.SetActive(true);
        }

        private void CloseScreenHome()
        {
            _homecomingScreen.gameObject.SetActive(false);
        }
    }
}
