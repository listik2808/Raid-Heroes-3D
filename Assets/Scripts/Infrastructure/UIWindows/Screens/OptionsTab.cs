using Scripts.Data;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.StaticData;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static System.Net.WebRequestMethods;
using Random = UnityEngine.Random;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class OptionsTab : Tab, ISavedProgessReader
    {
        public const string TextNewRaid = "Рейдов начато:";
        public const string TextVictoryBattleraid = "Одержано побед в рейде:";
        public const string TextGold = "Золота получено:";
        public const string TextDimond = "Кристаллов получено:";
        public const string TextStars = "Звезд получено:";
        public const string TextSupport = "support@persona.games";
        public const string HOME_URL = "https://personagame.ru/tw/vk/";

        [SerializeField] private BookmarkButton _offSoundButton;
        [SerializeField] private BookmarkButton _onSoundButton;
        [SerializeField] private BookmarkButton _offMusicButton;
        [SerializeField] private BookmarkButton _onMusicButton;
        [SerializeField] private BookmarkButton _language;
        [SerializeField] private BookmarkButton _support;
        [SerializeField] private TMP_Text _supportText;
        [SerializeField] private TMP_Text _idText;
        [SerializeField] private TMP_Text _textRaidNew;
        [SerializeField] private TMP_Text _textRaidVictory;
        [SerializeField] private TMP_Text _textGold;
        [SerializeField] private TMP_Text _textDaimond;
        [SerializeField] private TMP_Text _textStars;
        [SerializeField] private TMP_Text _textVersion;
        [SerializeField] private BookmarkButton _openCheatPanel;
        [SerializeField] private Canvas _option;
        [SerializeField] private BookmarkButton _exitButtonScreenCheatPanel;
        private int _numberCharacters = 10;
        private int[] _idNumber = new int[] {1, 2, 3, 4, 5, 6, 7, 8,9,0};
        private char[] _idChar = new char[] { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'n', 'm' };
        private int _raidCount = 0;
        private int _countVictoryRaidBattle = 0;
        private float _currentGolg = 0;
        private float _currentDaimond = 0;
        private float _currentStars = 0;
        private IPersistenProgressService _progressService;
        private string _gold;
        private string _stars;
        private string _daimond;
        private bool _isActivatedSound;
        private bool _isActivatedMusic;
        private string _currentId;

        public event Action<bool> ISActivatedSound;
        public event Action<bool> IsActivatedMusic;

        private void OnEnable()
        {
            SetCurrencyValue += ShowDataTextOption;
            _bookmarkButton.ButtonOnClic += OpenScreen;
            _exitButtonScreen.ButtonOnClic += CloseScreen;
            _openCheatPanel.ButtonOnClic += OpenChetPanelCanvas;
            _exitButtonScreenCheatPanel.ButtonOnClic += ScreenCloseCheatPanel;
            _support.ButtonOnClic += OpenSupportSite;
            //_offMusicButton.ButtonOnClic += OffMusic;
            //_offSoundButton.ButtonOnClic += OffSound;
            //_onMusicButton.ButtonOnClic += OnMusic;
            //_onSoundButton.ButtonOnClic += OnSound;
        }


        private void OnDisable()
        {
            SetCurrencyValue -= ShowDataTextOption;
            _bookmarkButton.ButtonOnClic -= OpenScreen;
            _exitButtonScreen.ButtonOnClic -= CloseScreen;
            _openCheatPanel.ButtonOnClic -= OpenChetPanelCanvas;
            _exitButtonScreenCheatPanel.ButtonOnClic -= ScreenCloseCheatPanel;
            //_onMusicButton.ButtonOnClic -= OffMusic;
            //_offSoundButton.ButtonOnClic -= OffSound;
            //_offMusicButton.ButtonOnClic -= OnMusic;
            //_onSoundButton.ButtonOnClic -= OnSound;
        }

        private void Awake()
        {
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }

        private void OffMusic()
        {
            _isActivatedMusic = false;
            _offMusicButton.gameObject.SetActive(false);
            _onMusicButton.gameObject.SetActive(true);
            IsActivatedMusic?.Invoke(_isActivatedMusic);
        }

        private void OffSound()
        {
            _isActivatedSound = false;
            _offSoundButton.gameObject.SetActive(false);
            _onSoundButton.gameObject.SetActive(true);
            ISActivatedSound?.Invoke(_isActivatedSound);
        }

        private void OnMusic()
        {
            _isActivatedMusic = true;
            _onMusicButton.gameObject.SetActive(false);
            _offMusicButton.gameObject.SetActive(true);
            IsActivatedMusic?.Invoke(_isActivatedMusic);
        }

        private void OnSound()
        {
            _isActivatedSound = true;
            _onSoundButton.gameObject.SetActive(false);
            _offSoundButton.gameObject.SetActive(true);
            ISActivatedSound?.Invoke(_isActivatedSound);
        }

        private void SetNewRaidCount(PlayerProgress playerProgress)
        {
            _raidCount = playerProgress.OptionData.CountRaid; 
        }

        private void SetCountBattleVictoryRaid(PlayerProgress playerProgress)
        {
            _countVictoryRaidBattle = playerProgress.OptionData.WonRaids;
        }

        private void SetCurrency(PlayerProgress playerProgress)
        {
            _currentGolg = (float)Math.Round(playerProgress.Wallet.Coins.AllCount);
            _gold = AbbreviationsNumbers.ShortNumber(_currentGolg);
            _currentDaimond = (float)Math.Round(playerProgress.Wallet.Diamonds.AllCount);
            _daimond = AbbreviationsNumbers.ShortNumber(_currentDaimond);
            _currentStars = (float)Math.Round(playerProgress.Wallet.Stars.AllCount);
            _stars = AbbreviationsNumbers.ShortNumber(_currentStars);
        }

        private void ShowDataTextOption()
        {
            SetCurrency(_progressService.Progress);
            _idText.text = _currentId;
            _textRaidNew.text = TextNewRaid + _raidCount.ToString();
            _textRaidVictory.text = TextVictoryBattleraid + _countVictoryRaidBattle.ToString();
            _textGold.text = TextGold + _gold;
            _textDaimond.text = TextDimond + _daimond;
            _textStars.text = TextStars + _stars;
            _supportText.text = TextSupport;
            _textVersion.text = "v." + Application.version;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            //if(progress.OptionData.ID == null)
            //{
            //    SetIdRandom(progress);
            //}
            //else
            //{
            //    _currentId = progress.OptionData.ID;
            //    _idText.text = _currentId;
            //}
            SetNewRaidCount(progress);
            SetCountBattleVictoryRaid(progress);
            SetCurrency(progress);
            ShowDataTextOption();
        }

        private void OpenChetPanelCanvas()
        {
            _option.gameObject.SetActive(true);
            _canvas.gameObject.SetActive(false);
        }

        private void ScreenCloseCheatPanel()
        {
            _option.gameObject.SetActive(false);
        }

        private void OpenSupportSite()
        {
            //string url = "https://personagame.ru";
            string urlMail = "mailto://support@persona.games";
            Application.OpenURL(urlMail);
        }

        private void SetIdRandom(PlayerProgress progress)
        {
            StartCoroutine(SetRandomChar(progress));
        }

        private IEnumerator SetRandomChar(PlayerProgress progress)
        {
            while (_numberCharacters > 0)
            {
                int result = Random.Range(0, _idNumber.Length);
                _numberCharacters--;
                _currentId += _idNumber[result];
                int resultChar = Random.Range(0, _idChar.Length);
                _numberCharacters--;
                _currentId += _idChar[resultChar];
                yield return null;
            }

            progress.OptionData.ID = _currentId;
        }
    }
}
