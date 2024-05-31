using Agava.WebUtility;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;
using YG;

namespace Scripts.Music
{
    public class SoundEffectsUi : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private BookmarkButton _audioSoundOff;
        [SerializeField] private BookmarkButton _audioSoundOn;
        private bool _isSoundEffects = true;
        private bool _isVideoAds = false;


        private void OnEnable()
        {
            YandexGame.GetDataEvent += DownloadSettingsSoundMenu;
            _audioSoundOff.ButtonOnClic += MenySoundOff;
            _audioSoundOn.ButtonOnClic += MenuSoundOn;
        }

        private void OnDisable()
        {
            YandexGame.GetDataEvent -= DownloadSettingsSoundMenu;
            _audioSoundOff.ButtonOnClic -= MenySoundOff;
            _audioSoundOn.ButtonOnClic -= MenuSoundOn;
        }

        private void Awake()
        {
            DownloadSettingsSoundMenu();
        }

        public void PauseEffect()
        {
            if(_isSoundEffects)
            {
                MenySoundOff();
                _isVideoAds = true;
            }
        }

        public void PlayEffect()
        {
            if (_isVideoAds)
            {
                _isVideoAds = false;
                MenuSoundOn();
            }
            
        }

        private void DownloadSettingsSoundMenu()
        {
            if (YandexGame.SDKEnabled == true)
            {
                _isSoundEffects = YandexGame.savesData.IsSoundEffects;
                _audioSource.enabled = _isSoundEffects;
                ActivateButtonMenuSoundEffects();
            }
        }

        private void MenuSoundOn()
        {
            if (_isSoundEffects == false)
            {
                _isSoundEffects = true;
                _audioSource.enabled = true;
               //_audioSource.Play();
                ActivateButtonMenuSoundEffects();
            }
        }

        private void MenySoundOff()
        {
            if (_isSoundEffects)
            {
                _isSoundEffects = false;
                _audioSource.enabled = false;
                ActivateButtonMenuSoundEffects();
            }
        }

        private void ActivateButtonMenuSoundEffects()
        {
            if (_isSoundEffects == false)
            {
                _audioSoundOff.gameObject.SetActive(false);
                _audioSoundOn.gameObject.SetActive(true);
            }
            else
            {
                _audioSoundOff.gameObject.SetActive(true);
                _audioSoundOn.gameObject.SetActive(false);
            }
            YandexGame.savesData.IsSoundEffects = _isSoundEffects;
        }
    }
}