using Agava.WebUtility;
using UnityEngine;
using YG;

namespace Scripts.Music
{
    public class SoundBattle : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private BattleSpeed _battleSpeed;

        private bool _isSoundMenu = true;
        private bool _isVideoAds = false;
       // private bool _isFocus = false;


        public bool IsVideoAds => _isVideoAds;

        private void OnEnable()
        {
            WebApplication.InBackgroundChangeEvent += OnBackgroundChange;
            YandexGame.GetDataEvent += DownloadSettingsSoundMenu;
        }

        private void OnDisable()
        {
            WebApplication.InBackgroundChangeEvent -= OnBackgroundChange;
            YandexGame.GetDataEvent -= DownloadSettingsSoundMenu;
        }

        private void Awake()
        {
            DownloadSettingsSoundMenu();
        }

        private void DownloadSettingsSoundMenu()
        {
            if (YandexGame.SDKEnabled == true)
            {
                _isSoundMenu = YandexGame.savesData.IsSoundMenu;
            }
        }

        public void PlayMusicBasedOnCondition()
        {
            if (_isSoundMenu == false)
                _musicSource.Pause();
            else
                _musicSource.Play();
        }

        public void PuseSound()
        {
            if (_isSoundMenu)
            {
                _isVideoAds = true;
                _musicSource.Pause();
            }
        }

        public void PlaySound()
        {
            if (_isVideoAds)
            {
                _isVideoAds = false;
                _musicSource.Play();
            }
        }

        private void OnBackgroundChange(bool inBackground)
        {
            AudioListener.pause = inBackground;
            AudioListener.volume = inBackground ? 0f : 1f;
            Time.timeScale = inBackground ? 0f : _battleSpeed.CurrentSpeed;
        }
    }
}
