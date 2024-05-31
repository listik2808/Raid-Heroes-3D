using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Logic.TaskAchievements;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ScreenTask :Screen
    {
        [SerializeField] private Image _infoTaskMarker;
        [SerializeField] private List<AchievementsAll> _all = new List<AchievementsAll>();
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private TMP_Text _text;
        private int _count = 0;
        private AchievementsAll _achieve;
        private List<AchievementsAll> _achievements = new List<AchievementsAll>();
        private IPersistenProgressService _progressService;
        //[SerializeField] private BookmarkButton _dayTime;
        //[SerializeField] private GameObject _scroll;
        //[SerializeField] private BookmarkButton _buttonAll;
        //[SerializeField] private Slider _slider;

        protected override void OnEnable()
        {
            _bookmark.ButtonOnClic += ActivateCard;
        }

        protected override void OnDisable()
        {
            _bookmark.ButtonOnClic -= ActivateCard;
            foreach (var item in _all)
            {
                item.ChangetMarker -= SetMarker;
            }
            foreach (var card in _all)
            {
                if (card.TryGetComponent(out GetCoinsAchievement getCoinsAchievement))
                {
                    if(getCoinsAchievement.PersistenProgressService != null)
                        getCoinsAchievement.PersistenProgressService.Progress.Wallet.Coins.Value -= getCoinsAchievement.SetCoins;
                }
                if(card.TryGetComponent(out GetDiamondsAchievement getDiamondsAchievement))
                {
                    if(getDiamondsAchievement.PersistenProgressService != null)
                        getDiamondsAchievement.PersistenProgressService.Progress.Wallet.Diamonds.Value -= getDiamondsAchievement.SetDiamonds;
                }
                if (card.TryGetComponent(out GetStarsAchievement getStarsAchievement))
                {
                    if(getStarsAchievement.PersistenProgressService != null)
                        getStarsAchievement.PersistenProgressService.Progress.Wallet.Stars.Value-= getStarsAchievement.SetStars;
                }
            }
        }


        public void Initialize(MainScreen mainScreen, IPersistenProgressService persistenProgress)
        {
            _progressService = persistenProgress;
            if(_progressService.Progress.WorldData.Activiti)
            {
                foreach (var card in _all)
                {
                    card.ChangetMarker += SetMarker;
                    card.Construct(mainScreen,this);
                    if(card.TryGetComponent(out GetCoinsAchievement getCoinsAchievement))
                    {
                        getCoinsAchievement.PersistenProgressService.Progress.Wallet.Coins.Value += getCoinsAchievement.SetCoins;
                    }
                    if(card.TryGetComponent(out GetDiamondsAchievement getDiamondsAchievement))
                    {
                        getDiamondsAchievement.PersistenProgressService.Progress.Wallet.Diamonds.Value += getDiamondsAchievement.SetDiamonds;
                    }
                    if(card.TryGetComponent(out GetStarsAchievement getStarsAchievement))
                    {
                        getStarsAchievement.PersistenProgressService.Progress.Wallet.Stars.Value += getStarsAchievement.SetStars;
                    }
                    card.ActivatedCard();
                }
                SetMarker();
            }
        }

        public void SortCard()
        {
            SortProgressCard();
            ActivateListCardProgress();
            OpenScreen();
        }

        private void SortProgressCard()
        {
            for (int i = 0; i < _all.Count; i++)
            {
                for (int j = 0; j + 1 < _all.Count; j++)
                {
                    if (_all[i].CurrentFillSlider > _all[j].CurrentFillSlider)
                    {
                        _achieve = _all[i];
                        _all[i] = _all[j];
                        _all[j] = _achieve;
                    }
                }
            }
        }

        private void ActivateListCardProgress()
        {
            for (int i = 0; i < _all.Count; i++)
            {
                _all[i].transform.SetSiblingIndex(i);
            }
        }

        private void ActivateCard()
        {
            foreach (var card in _all)
            {
                card.ActivatedCard();
            }
            SortCard();
        }

        private void SetMarker()
        {
            _count = 0;
            foreach (var item in _all)
            {
                _count += item.MArkers;
            }

            if(_count > 0)
            {
                _infoTaskMarker.gameObject.SetActive(true);
                _text.text = _count.ToString();
            }
            else
            {
                _infoTaskMarker.gameObject.SetActive(false);
            }
        }

        //private void OpenProgressDaily()
        //{
        //    DiactivateAllProgress(_dayTime, _scroll);
        //    ActivateAllProgress(_buttonAll, _slider.gameObject);
        //}

        //private void CloseProgressDaily()
        //{
        //    DiactivateAllProgress(_buttonAll, _slider.gameObject);
        //    ActivateAllProgress(_dayTime, _scroll);
        //}

        //private void ActivateAllProgress(BookmarkButton button, GameObject view)
        //{
        //    button.gameObject.SetActive(true);
        //    view.gameObject.SetActive(true);
        //}

        //private void DiactivateAllProgress(BookmarkButton button, GameObject view)
        //{
        //    button.gameObject.SetActive(false);
        //    view.gameObject.SetActive(false);
        //}
    }
}
