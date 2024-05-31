using Scripts.BattleLogic.GameResult;
using Scripts.Logic;
using Scripts.Music;
using Scripts.StaticData;
using System;
using UnityEngine;
using YG;

namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class ButtonAds : MonoBehaviour
    {
        //[SerializeField] private BookmarkButton _buttonAdsLose;
        [SerializeField] private BookmarkButton _buttonAdsVictory;
        [SerializeField] private NextFight _nextFight;
        [SerializeField] private SoundBattle _soundbattle;
        [SerializeField] private SoundEffectUi _soundEffectUi;

        string _result;
        float _price;
        //public BookmarkButton ButtonAdsLose => _buttonAdsLose;
        public BookmarkButton ButtonAdsVictory => _buttonAdsVictory;

        private void OnEnable()
        {
            //_buttonAdsLose.ButtonOnClic += OnShowVideoButtonClickLose;
            _buttonAdsVictory.ButtonOnClic += OnShowVideoButtonClickVictory;
            YandexGame.RewardVideoEvent += CalculateReward;
        }

        private void OnDisable()
        {
            //_buttonAdsLose.ButtonOnClic -= OnShowVideoButtonClickLose;
            _buttonAdsVictory.ButtonOnClic -= OnShowVideoButtonClickVictory;
            YandexGame.RewardVideoEvent -= CalculateReward;
        }

        private void OnShowVideoButtonClickLose()
        {
            _soundbattle.PuseSound();
            _soundEffectUi.EffectPause();
            YandexGame.RewVideoShow(2);
        }

        private void OnShowVideoButtonClickVictory()
        {
            _soundbattle.PlaySound();
            _soundEffectUi.EffectPause();
            YandexGame.RewVideoShow(2);
        }

        private void SetReward()
        {
            _nextFight.Text.text = _result;
            //_buttonAdsLose.gameObject.SetActive(false);
            _buttonAdsVictory.gameObject.SetActive(false);
        }

        private void CalculateReward(int index)
        {
            YandexGame.timerShowAd = 0;
            _soundEffectUi.EffectPlay();
            _soundbattle.PlaySound();
            int id = _nextFight.PreparingBattle.ProgressService.Progress.PointSpawn.IdSpawnerEnemy;
            _price = CountingRewards.GetPveRewardGold(id);
            if (index == 2 && _nextFight.EnemyDead)
            {
                _price *= 10;
                _result = AbbreviationsNumbers.ShortNumber(_price);
            }
            if (index == 2 && _nextFight.EnemyDead == false)
            {
                _price = (_price / 10) * 10;
                _result = AbbreviationsNumbers.ShortNumber(_price);
            }
            Reward();
        }

        private void Reward()
        {
            _nextFight.SetPrice(_price,0);
            SetReward();
        }
    }
}