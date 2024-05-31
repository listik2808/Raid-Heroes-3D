using Scripts.Army.PlayerSquad;
using Scripts.Data;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Player;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Logic.TaskAchievements;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Infrastructure.UIWindows.UIProgressReid
{
    public class FightNumber : MonoBehaviour
    {
        [SerializeField] private ReachRaidAchievement _reachRaidAcievement;
        [SerializeField] private TMP_Text _CurrentNumberAttak;
        [SerializeField] private ListEnemyUnits _listEnemyUnits;
        [SerializeField] private BookmarkButton _buttonNewRaid;
        private int _numberOpenNewraid = 15;
        private int _currentNumber;

        public int CurrentNumber => _currentNumber;

        private void OnEnable()
        {
            _listEnemyUnits.NumberBattlesChanged += SetNumberButtle;
        }

        private void OnDisable()
        {
            _listEnemyUnits.NumberBattlesChanged -= SetNumberButtle;
        }

        private void Start()
        {
            Show();
        }

        private void Show()
        {
            _CurrentNumberAttak.text =  "#" + _currentNumber;
        }

        private void SetNumberButtle(int value)
        {
            _currentNumber = value;
            if(SceneManager.GetActiveScene().name == AssetPath.SceneMain)
            {
                _reachRaidAcievement.SetNamberRaid();
            }

            Show();
            if(_currentNumber >= _numberOpenNewraid)
            {
                _buttonNewRaid.gameObject.SetActive(true);
            }
            else
            {
                _buttonNewRaid.gameObject.SetActive(false);
            }
        }
    }
}