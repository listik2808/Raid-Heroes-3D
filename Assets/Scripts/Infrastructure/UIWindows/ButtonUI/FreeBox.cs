using Scripts.Army.AllCadsHeroes;
using Scripts.Data;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.UIProgressReid;
using Scripts.Logic;
using Scripts.Logic.TaskAchievements;
using Scripts.Music;
using Scripts.StaticData;
using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using Random = UnityEngine.Random;

namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class FreeBox : MonoBehaviour, ISavedProgress
    {
        public const string InfoText = "Информация";
        public const string MessageText = "Возможная награда";
        public const string InfoTextGet = "Ежедневный набор";
        public const string MessageReward = "Награда";

        [SerializeField] private SoundEffectsUi _soundEffectsUi;
        [SerializeField] private SoundMenu _soundMenu;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private FightNumber _flightNumber;
        [SerializeField] private BookmarkButton _buttonAds;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _textTime;
        [SerializeField] private TMP_Text _textInfo;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private TMP_Text _currentCoin;
        [SerializeField] private TMP_Text _currentCrystal;
        [SerializeField] private TMP_Text _currentCard;
        [SerializeField] private Image _cardIcon;
        [SerializeField] private GameObject _cardContainer;
        [SerializeField] private BookmarkButton _closeButton;
        [SerializeField] private BookmarkButton _buttonReward;
        [SerializeField] private BookmarkButton _buttonInfo;
        [SerializeField] private ListAllHeroes _allHeroes;
        [SerializeField] private TMP_Text _textButton;
        [SerializeField] private GameObject _contayner;
        [SerializeField] private ButtonCloseScreen _closeScreen;
        [SerializeField] private BookmarkButton _buttonOpenInfo;
        [SerializeField] private Image _infoImage;
        [SerializeField] private TMP_Text _textInfoImage;
        [SerializeField] private Image _infoButtonFreeBox;
        [SerializeField] private TMP_Text _infoText;
        [SerializeField] private WatchCommercialsAchievement _watchCommercialsAchievement;

        private float _timerReset = 43200f;
        private float _currentTime;
        private bool _watchedAdvertisement = false;
        private bool _isTimer = false;
        private bool _isLoad = false;
        private string _coutCard = "1";
        private float _goldMin;
        private float _goldMax;
        private int _crystalMin = 1;
        private int _crystalMax = 15;
        private const float _min = 1;
        private const float _max = 10;
        private float _gold;
        private float _crystal;
        private string _hours;
        private string _minutes;
        private string _seconds;
        private string _convertedTime;
        private char p = ':';
        private PlayerProgress _progressService;
        private ISaveLoadService _savedProgress;

        //private event Action OpenCollbeck;
        //private event Action CloseCollbeck;

        private void OnEnable()
        {
            _buttonAds.ButtonOnClic += OnShowVideo;
            _buttonInfo.ButtonOnClic += OpenCanvas;
            _buttonOpenInfo.ButtonOnClic += OpenCanvas;
            //OpenCollbeck += SetComponent;
           //CloseCollbeck += TimerStart;
            //YandexGame.OpenVideoEvent += SetComponent;
            //YandexGame.CloseVideoEvent += TimerStart;
            YandexGame.RewardVideoEvent += SetComponent;
        }

        private void OnDisable()
        {
            _buttonAds.ButtonOnClic -= OnShowVideo;
            _buttonInfo.ButtonOnClic -= OpenCanvas;
            _buttonOpenInfo.ButtonOnClic -= OpenCanvas;
            YandexGame.RewardVideoEvent -= SetComponent;
            //OpenCollbeck -= SetComponent;
            //CloseCollbeck -= TimerStart;
            //YandexGame.OpenVideoEvent += SetComponent;
            //YandexGame.RewardVideoEvent -= SetComponent;
            //YandexGame.CloseVideoEvent += TimerStart;
        }

        private void Awake()
        {
            if(SceneManager.GetActiveScene().name != AssetPath.SceneMain)
            {
                this.enabled = false;
            }
            _savedProgress = AllServices.Container.Single<ISaveLoadService>();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.Shop.WatchedAdvertisement = _watchedAdvertisement;
            progress.Shop.TimeCurrent = _currentTime;
            progress.Shop.IsTimer = _isTimer;
            progress.Shop.ConvertedTime = _convertedTime;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _progressService = progress;
            _watchedAdvertisement = progress.Shop.WatchedAdvertisement;
            _currentTime = progress.Shop.TimeCurrent;
            _isTimer = progress.Shop.IsTimer;
            _convertedTime = progress.Shop.ConvertedTime;
            if (_watchedAdvertisement)
            {
                _contayner.gameObject.SetActive(true);
                _textButton.gameObject.SetActive(false);
            }
            LoadTime();
            _isLoad = true;
        }

        private void Update()
        {
            ShowAccessibilityMarker();

            if (_watchedAdvertisement && _isTimer && _isLoad)
            {
                if(_currentTime <= 0)
                {
                    RestartButton();
                }
                _currentTime -= 1 * Time.deltaTime;
                SaveTime();
                SetDataTimer();
                
            }
        }

        private void ShowAccessibilityMarker()
        {
            if(_watchedAdvertisement == false && _infoImage.gameObject.activeInHierarchy == false)
            {
                _infoImage.gameObject.SetActive(true);
                _textInfoImage.text = 1.ToString();
                _infoButtonFreeBox.gameObject.SetActive(true);
                _infoText.text = 1.ToString();
            }
        }

        private void LoadTime()
        {
            if(_watchedAdvertisement && _isTimer)
            {
                DateTime result = DateTime.ParseExact(_convertedTime,"u", CultureInfo.InvariantCulture);
                TimeSpan timePassed = DateTime.UtcNow - result;
                _currentTime -= (float)timePassed.TotalSeconds;
                TimerStart();
            }
        }

        private void SaveTime()
        {
            _progressService.Shop.ConvertedTime = _convertedTime;
            _progressService.Shop.TimeCurrent = _currentTime;
            _convertedTime = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
        }

        private void ShowTimer(string hours = null, char p = ' ',string minutes = null, char p1 = ' ', string second = null)
        {
            _textTime.text = string.Format($"{hours}{p}{minutes}{p1}{second}");
        }

        private void RestartButton()
        {
            _watchedAdvertisement = false;
            _currentTime = 0;
            _isTimer = false;
            _contayner.gameObject.SetActive(false);
            _textButton.gameObject.SetActive(true);
            _textTime.gameObject.SetActive(false);
        }

        private void SetDataTimer()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_currentTime);
            if (timeSpan.Hours != 0)
            {
                _hours = timeSpan.Hours.ToString();
                _minutes = timeSpan.Minutes.ToString();
                _seconds = timeSpan.Seconds.ToString();
                ShowTimer(_hours,p,_minutes,p ,_seconds);
            }
            else if (timeSpan.Hours == 0)
            {
                _minutes = timeSpan.Minutes.ToString();
                _seconds = timeSpan.Seconds.ToString();
                ShowTimer(null,' ',_minutes,p,_seconds);
            }
            else if(timeSpan.Hours == 0 && timeSpan.Minutes == 0)
            {
                _seconds = timeSpan.Seconds.ToString();
                ShowTimer(null,' ',null,' ',_seconds);
            }
        }

        private void OnShowVideo()
        {
            if (_watchedAdvertisement == false)
            {
                _infoImage.gameObject.SetActive(false);
                _infoButtonFreeBox.gameObject.SetActive(false);
                _watchedAdvertisement = true;
                SetCanvasRewardFree(1);
            }
            else if(_watchedAdvertisement && _isTimer == false)
            {
#if !UNITY_EDITOR
                _soundEffectsUi.PauseEffect();
                _soundMenu.PauseSound();
                YandexGame.RewVideoShow(1);
                _currentTime = _timerReset;
                //SetCanvasRewardFree();
                //TimerStart();
#endif
            }
        }

        public void SetComponent(int id)
        {
            if (id == 1)
            {
                SetCanvasRewardFree(1);
                TimerStart();
                _soundEffectsUi.PlayEffect();
                _soundMenu.PlaySound();
            }
        }

        private void TimerStart()
        {
            _isTimer = true;
            _watchedAdvertisement = true;
            _buttonAds.Button.interactable = false;
            _textTime.gameObject.SetActive(true);
            _contayner.gameObject.SetActive(false);
            _textButton.gameObject.SetActive(false);
        }

        private void SetCanvasRewardFree(int index)
        {
            _closeButton.gameObject.SetActive(false);
            string text;
            GetFreeItemreward();
            text = AbbreviationsNumbers.ShortNumber(_gold);
            text =AbbreviationsNumbers.Value.ToString();
            _currentCoin.text = text + AbbreviationsNumbers.Chars[AbbreviationsNumbers._maxChar];
            text = AbbreviationsNumbers.ShortNumber(_crystal);
            text = AbbreviationsNumbers.Value.ToString();
            _currentCrystal.text = text + AbbreviationsNumbers.Chars[AbbreviationsNumbers._maxChar];
            _textInfo.text = InfoTextGet;
            _message.text = MessageReward;
            _buttonReward.gameObject.SetActive(true);
            _closeScreen.SetButton(_buttonReward);
            _buttonReward.ButtonOnClic += GetReward;
            _canvas.gameObject.SetActive(true);
            _audioSource.Play();
            _contayner.gameObject.SetActive(true);
            _textButton.gameObject.SetActive(false);
        }

        private void GetReward()
        {
            _progressService.Wallet.Coins.Add(_gold);
            _progressService.Wallet.Diamonds.Add(_crystal);
            _savedProgress.SaveProgress();
            _canvas.gameObject.SetActive(false);
        }

        private void OpenCanvas()
        {
            _watchCommercialsAchievement.SetCountShowCommercial();
            _currentCoin.text = Price.GetLastPVERewardGold(_flightNumber,_max).ToString();
            _currentCrystal.text = _crystalMax.ToString();
            _textInfo.text = InfoText;
            _message.text = MessageText;
            _buttonReward.gameObject.SetActive(false);
            _cardIcon.gameObject.SetActive(true);
            _cardContainer.gameObject.SetActive(true);
            _currentCard.text = _coutCard;
            _closeButton.gameObject.SetActive(true);
            _canvas.gameObject.SetActive(true);
            _audioSource.Play();
            _closeScreen.SetButton(_closeButton);
            _closeButton.ButtonOnClic += CloseCanvas;
        }

        private void CloseCanvas()
        {
            _closeButton.ButtonOnClic -= CloseCanvas;
            _canvas.gameObject.SetActive(false);
        }

        private void GetFreeItemreward(bool possible = false)
        {
            float value = Random.Range(0.1f, 1.1f);
            _goldMin = Price.GetLastPVERewardGold(_flightNumber, _min);
            _goldMax = Price.GetLastPVERewardGold(_flightNumber, _max);
            _gold = possible ? _goldMax : (_goldMin + (float)Math.Round(value * (_goldMax - _goldMin)));
            _crystal = possible ? _crystalMax : (_crystalMin + (Random.Range(1, _crystalMax)));
            _cardIcon.gameObject.SetActive(false);
            _cardContainer.gameObject.SetActive(false);
            float result = Random.Range(0.1f, 1.1f);
            if(_progressService.Training.Tutor == false && _progressService.Training.CountCard < 5)
            {
                result = 1;
            }
            if (possible || result < 0.5f)
            {
                int res = _allHeroes.EnrollRandomHeroCard(true,0f, 1, 1,null,_buttonReward);
                if (res >= 0)
                {
                    _cardIcon.gameObject.SetActive(true);
                    _cardContainer.gameObject.SetActive(true);
                    _currentCard.text = _coutCard;
                }
            }
        }
    }
}