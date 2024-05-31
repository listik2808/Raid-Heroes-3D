using Agava.WebUtility;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;
using YG;

namespace Scripts.Music
{
    public class SoundMenu : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private BookmarkButton _audioMenuPause;
        [SerializeField] private BookmarkButton _audioMenuPlau;
        private bool _isSoundMenu = true;
        //private bool _isFocus = true;
        private bool _isPause = false;
        private bool _isVideoAds = false;
        public bool IsSoundMenu => _isSoundMenu;

        private void OnEnable()
        {
            WebApplication.InBackgroundChangeEvent += OnBackgroundChange;
            YandexGame.GetDataEvent += DownloadSettingsSoundMenu;
            _audioMenuPause.ButtonOnClic += MenyPauseAudio;
            _audioMenuPlau.ButtonOnClic += MenuPlayAudio;
        }

        private void OnDisable()
        {
            WebApplication.InBackgroundChangeEvent -= OnBackgroundChange;
            YandexGame.GetDataEvent -= DownloadSettingsSoundMenu;
            _audioMenuPause.ButtonOnClic -= MenyPauseAudio;
            _audioMenuPlau.ButtonOnClic -= MenuPlayAudio;
        }

        private void  Awake()
        {
            DownloadSettingsSoundMenu();
        }

        public void PauseSound()
        {
            if (_isSoundMenu)
            {
                _isVideoAds = true;
                MenyPauseAudio();
            }
        }

        public void PlaySound()
        {
            if (_isVideoAds)
            {
                _isVideoAds = false;
                MenuPlayAudio();
            }
        }

        private void DownloadSettingsSoundMenu()
        {
            if (YandexGame.SDKEnabled == true)
            {
                _isSoundMenu = YandexGame.savesData.IsSoundMenu;
                if (_isSoundMenu)
                    _musicSource.Play();
                else
                    _musicSource.Stop();

                ActivateButtonMenuSound();
            }
        }

        private void MenuPlayAudio()
        {
            if(_isSoundMenu == false)
            {
                _isSoundMenu = true;
                _musicSource.Play();
                //AudioListener.volume = 1;
                //AudioListener.pause = false;
                ActivateButtonMenuSound();
            }
        }

        private void MenyPauseAudio()
        {
            if(_isSoundMenu)
            {
                _isSoundMenu = false;
                _musicSource.Stop();
                //AudioListener.volume = 0;
                //AudioListener.pause = true;
                ActivateButtonMenuSound();
            }
        }

        private void ActivateButtonMenuSound()
        {
            if(_isSoundMenu == false)
            {
                _audioMenuPause.gameObject.SetActive(false);
                _audioMenuPlau.gameObject.SetActive(true);
            }
            else
            {
                _audioMenuPause.gameObject.SetActive(true);
                _audioMenuPlau.gameObject.SetActive(false);
            }
            YandexGame.savesData.IsSoundMenu = _isSoundMenu;
        }

        private void OnBackgroundChange(bool inBackground)
        {
            AudioListener.pause = inBackground;
            AudioListener.volume = inBackground ? 0f : 1f;
            Time.timeScale = inBackground ? 0f : 1f;
        }
    }
}
