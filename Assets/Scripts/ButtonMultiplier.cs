using Scripts.Infrastructure.UIWindows.Screens;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Source.Scripts.Logic
{
    public class ButtonMultiplier : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private ScreenUprageSoldier _screenUprageSoldier;
        [SerializeField] private AudioSource _audioSource;
        private int _multiplierX1 = 1;
        private int _multiplierX10 = 10;
        private string _max = "MAX";
        private int _currentMultiplier;
        private int _index = 1;
        private bool _isMax =false;
        private bool _isTen = false;

        public Button Button => _button;
        public int CurrentMultiplier => _currentMultiplier;
        public bool IsMax => _isMax;
        public bool IsTen => _isTen;

        public event Action LoadMeltiplier;

        private void OnEnable()
        {
            _button.onClick.AddListener(ActivateMultiplier);
           
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ActivateMultiplier);
        }

        private void Start()
        {
            LoadNumberButton();
        }

        private void LoadNumberButton()
        {
            if (YandexGame.SDKEnabled == true)
            {
                _index = YandexGame.savesData.NumberButtonMultiplyer;
                SetTxetOrMultiplier();
                LoadMeltiplier?.Invoke();
            }
        }

        private void SetTxetOrMultiplier()
        {
            YandexGame.savesData.NumberButtonMultiplyer = _index;
            switch (_index)
            {
                case 1:
                    _text.text = "x" + _multiplierX1.ToString();
                    _isMax = false;
                    _isTen = false;
                    _currentMultiplier = _multiplierX1;
                    break;
                case 2:
                    _text.text = "x" + _multiplierX10.ToString();
                    _isMax = false;
                    _isTen = true;
                    _currentMultiplier = _multiplierX10;
                    break;
                case 3:
                    _text.text = _max;
                    _isMax = true;
                    _isTen = false;
                    _currentMultiplier = _screenUprageSoldier.Soldier.SoldiersStatsLevel.GetMaxLevelOrCurrentLevel(_screenUprageSoldier.Soldier.Rank.CurrentLevelHero) * 10;
                    break;
            }
        }

        private void ActivateMultiplier()
        {
            _audioSource.Play();
            if (_index == 3)
            {
                _index = 1;
            }
            else
            {
                _index++;
            }
            SetTxetOrMultiplier();
        }
    }
}
