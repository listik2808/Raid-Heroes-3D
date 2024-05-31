using Assets.Scripts.Economics;
using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Logic.TaskAchievements;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.UIProgressReid
{
    public class ProgressReaid :MonoBehaviour,ISavedProgress
    {
        public const string TextReaid1 = "Рейд пройден на ";
        public const string TextReaid2 = "(лучш.";

        [SerializeField] private GameObject _menuMoney;
        [SerializeField] private MainScreen _mainScreen;
        [SerializeField] private FightNumber _flightNumber;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CompleteRaidAchievement _completeRaidAchievement;

        private RaidZones _raidZones;
        private float _maxIdEnemy;
        private float _currentRaid;
        private float _bestRaid = 0;

        private void OnEnable()
        {
            ClouseMeney();
        }

        private void OnDisable()
        {
            OpenMoney();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _bestRaid = progress.PointSpawn.BestId;
        }

        private void Start()
        {
            _raidZones = _mainScreen.RaidZones;
            GetMaxReid();
            _currentRaid = _flightNumber.CurrentNumber;
            RenderindSlider();
            _completeRaidAchievement.SetProgressAch(_currentRaid);
        }

        private void OpenMoney()
        {
            _menuMoney.SetActive(true);
        }

        private void ClouseMeney()
        {
            _menuMoney.SetActive(false);
        }

        private void RenderindSlider()
        {
            if(_maxIdEnemy != 0)
            {
                float value = _currentRaid / _maxIdEnemy;
                _progressSlider.value = value;
                double valueRound = Percent(value);

                if (_bestRaid > _currentRaid)
                {
                    float result = _bestRaid / _maxIdEnemy;
                    double resultPercent = Percent(result);
                    ShowBestReid(valueRound,resultPercent);
                }
                else if (_bestRaid <= _currentRaid)
                {
                    ShowReid(valueRound);
                }
            }
        }

        private static double Percent(float value)
        {
            float value1 = value * 100;
            double valueRound = Math.Round(value1);
            return valueRound;
        }

        private void GetMaxReid()
        {
            int countRaid = _raidZones.ListRaids.Count -1;
            int idLastEnemy = _raidZones.ListRaids[countRaid].SpawnMarkers.Count -1;
            _maxIdEnemy = _raidZones.ListRaids[countRaid].SpawnMarkers[idLastEnemy].Id;
        }

        private void ShowBestReid(double value,double valueRound)
        {
            _text.text = TextReaid1 + " " + value.ToString() + "% " + TextReaid2 + valueRound +"% " + ")";
        }

        private void ShowReid(double valueRound)
        {
            _text.text = TextReaid1 + " " + valueRound.ToString() + "%";
        }
    }
}