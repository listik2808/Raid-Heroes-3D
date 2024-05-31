using UnityEngine;
using YG;

namespace Scripts.Music
{
    public class SoundEffectUi : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private bool _isSoundEffects = true;
        private bool _isVideoAds = false;

        private void OnEnable()
        {
            YandexGame.GetDataEvent += DownloadSettingsSoundMenu;
        }

        private void OnDisable()
        {
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
                _isSoundEffects = YandexGame.savesData.IsSoundEffects;
                if(_audioSource != null)
                {
                    _audioSource.enabled = _isSoundEffects;
                }
            }
        }

        public void EffectPause()
        {
            if (_isSoundEffects)
            {
                _isVideoAds = true;
                _audioSource.enabled = false;
            }
        }

        public void EffectPlay()
        {
            if(_isVideoAds)
            {
                _isVideoAds = false;
                _audioSource.enabled = true;
            }
        }
    }
}
