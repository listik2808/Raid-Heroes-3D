using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Logic;
using Scripts.StaticData;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Scripts.BattleLogic.GameResult
{
    public class NextFight : MonoBehaviour
    {
        public const string MassagerReward = "Награда";
        public const string MassegerVictory = "ПОБЕДА!";
        public const string MassegerDefeat = "ПОРАЖЕНИЕ";
        public const string NextBattleText = "и начать бой #";
        [SerializeField] private PreparingBattle _preparingBattle;
        [SerializeField] private GameResultsWatcher _gameResultsWatcher;
        [SerializeField] private BookmarkButton _collectReward;
        [SerializeField] private BookmarkButton _nextBattle;
        [SerializeField] private BookmarkButton _RestartBattle;
        [SerializeField] private GameObject _buttonSpeed;
        [SerializeField] private TMP_Text _textNumberFight;
        [SerializeField] private TMP_Text _textMassegerReward;
        [SerializeField] private TMP_Text _textVictoryEndDefeat;
        [SerializeField] private TMP_Text _textRewardGold;
        [SerializeField] private TMP_Text _textStars;
        [SerializeField] private Image _imageStar;
        [SerializeField] private Image _iconCard;
        [SerializeField] private GameObject _stars;
        [SerializeField] private GameObject _card;
        [SerializeField] private ButtonAds _buttonAds;
        [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
        [SerializeField] private BookmarkButton _tutorButton;
        private float _rewardSars = 0;
        private bool _enemyDead;
        private float _price;
        private int _idMaxEnemy = 0;
        private bool _activeScreen = false;
        private IPersistenProgressService _progressService;
        public PreparingBattle PreparingBattle => _preparingBattle;
        public bool EnemyDead => _enemyDead;
        public TMP_Text Text => _textRewardGold;

        private void OnEnable()
        {
            _gameResultsWatcher.ChangetEnemyDaed += GettingResultBattle;
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }

        private void OnDisable()
        {
            _gameResultsWatcher.ChangetEnemyDaed -= GettingResultBattle;
        }

        public void SetPrice(float price,float starsReward)
        {
            _price = price;
            _rewardSars = starsReward;
            _preparingBattle.SetPrice(price,starsReward);
        }

        public void GettingResultBattle(bool value)
        {
            _enemyDead = value;
            if (_enemyDead)
                Victory();
            else
                Defeat();
            _buttonSpeed.gameObject.SetActive(false);
            if (_preparingBattle.IdEnemy <= 2 && _progressService.Progress.Training.Tutor == false 
                || _preparingBattle.IdEnemy == 9 && _progressService.Progress.Training.Tutor == false)
            {
                _collectReward.gameObject.SetActive(false);
            }
            else
            {
                _collectReward.gameObject.SetActive(true);
            }
        }

        private void Defeat()
        {
            _textMassegerReward.text = MassagerReward + "<color=#59181b>(10%)</color>";
            _textVictoryEndDefeat.text = MassegerDefeat;
            SetRewardPercent();
            _preparingBattle.SetPrice(_price,_rewardSars);
            _iconCard.gameObject.SetActive(false);
            _card.SetActive(false);
            _imageStar.gameObject.SetActive(false);
            _stars.SetActive(false);
            _nextBattle.gameObject.SetActive(false);
            _RestartBattle.gameObject.SetActive(true);
            //_buttonAds.ButtonAdsLose.gameObject.SetActive(true);
            _buttonAds.ButtonAdsVictory.gameObject.SetActive(true);
            _gameResultsWatcher.ShowResults();
        }

        private void Victory()
        {
            _idMaxEnemy = SetMaxEnemyId();
            if (_idMaxEnemy == _preparingBattle.IdEnemy)
            {
                _rewardSars = _preparingBattle.IdEnemy / 10;
                _rewardSars = MathF.Round(_rewardSars);
                string _rewardSarsStr = AbbreviationsNumbers.ShortNumber(_rewardSars);
                _textStars.text = _rewardSarsStr;
                _imageStar.gameObject.SetActive(true);
                _stars.SetActive(true);
            }
            else
            {
                _imageStar.gameObject.SetActive(false);
                _stars.SetActive(false);
            }
            int valueRandom = Random.Range(0, 101);
            if(valueRandom >= 50)
            {
                _buttonAds.ButtonAdsVictory.gameObject.SetActive(true);
            }
            int numberCard;
            _textMassegerReward.text = MassagerReward;
            _textVictoryEndDefeat.text = MassegerVictory;
            _progressService.Progress.Achievements.AllCountFightNumber += 1;
            _progressService.Progress.OptionData.WonRaids += 1;
            numberCard = _preparingBattle.TryGetIdNextFighter(_enemyDead);
            int nextNumberFight = _preparingBattle.IdEnemy + 1;
            SetRewardGold(nextNumberFight);
            _preparingBattle.SetPrice(_price,_rewardSars);
            _textNumberFight.text = NextBattleText + nextNumberFight;
            SetNumberTypeCard(numberCard);
            if(_preparingBattle.IdEnemy <= 2 && _progressService.Progress.Training.Tutor == false 
                || _preparingBattle.IdEnemy == 9 && _progressService.Progress.Training.Tutor == false)
            {
                _tutorButton.gameObject.SetActive(true);
                _nextBattle.gameObject.SetActive(false);
            }
            else
            {
                _tutorButton.gameObject.SetActive(false);
                _nextBattle.gameObject.SetActive(true);
            }
            
            _RestartBattle.gameObject.SetActive(false);
            if (numberCard < 0)
            {
                _gameResultsWatcher.ShowResults();
            }
            else
            {
                _gameResultsWatcher.ShowResults();
                //_preparingBattle.RandomChanceCard.ScreenCardShow.BookmarkButton.Button.onClick.AddListener(SetActivateScreen);
                //StartCoroutine(ActivateScreen());
            }
        }

        private IEnumerator ActivateScreen()
        {
            while (_activeScreen == false)
            {
                yield return null;
                if(_activeScreen == true)
                {
                    _preparingBattle.RandomChanceCard.ScreenCardShow.BookmarkButton.Button.onClick.RemoveListener(SetActivateScreen);
                    _gameResultsWatcher.ShowResults();
                    _activeScreen = false;
                }
            }
        }

        private void SetActivateScreen()
        {
            _activeScreen = true;
        }

        private void SetRewardPercent()
        {
            _price = CountingRewards.GetPveRewardGold(_preparingBattle.IdEnemy);
            _price = _price / 10;
            _price = MathF.Round(_price);
            string value = AbbreviationsNumbers.ShortNumber(_price);
            //value = AbbreviationsNumbers.Value.ToString();
            _textRewardGold.text = value; //+ AbbreviationsNumbers.Chars[AbbreviationsNumbers.Number];
        }

        private void SetRewardGold(int nextNumberFight)
        {
            int curentId = nextNumberFight - 1;
            _price = CountingRewards.GetPveRewardGold(curentId);
            _price = MathF.Round(_price);
            string value1 = AbbreviationsNumbers.ShortNumber(_price);
            //value1 = AbbreviationsNumbers.Value.ToString();
            _textRewardGold.text = value1; //+ AbbreviationsNumbers.Chars[AbbreviationsNumbers.Number];
        }

        private void SetNumberTypeCard(int numberCard)
        {
            if (numberCard >= 0)
            {
                _iconCard.sprite = _sprites[numberCard];
                _iconCard.gameObject.SetActive(true);
                _card.gameObject.SetActive(true);
            }
        }

        private int SetMaxEnemyId()
        {
            foreach (var item in _preparingBattle.RaidZones.ListRaids)
            {
                if (item.Id == _preparingBattle.IdRaid)
                {
                    _idMaxEnemy = item.MaxId;
                    return _idMaxEnemy;
                }
            }
            return -1;
        }
    }
}