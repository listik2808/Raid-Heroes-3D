using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;
using UnityEngine.EventSystems;
using Scripts.Army;
using TMPro;
using Scripts.Logic;
using Scripts.Army.TypesSoldiers.CharacteristicsSoldier;
using Scripts.Army.TypesSoldiers;
using System.Security.Cryptography;
using System.Collections;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.States;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.SaveLoad;
using UnityEngine.SceneManagement;
using Scripts.StaticData;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using UnityEngine.UI;
using System;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Scripts.Infrastructure.UIWindows.ScreensEnemy
{
    public class TroopsScreenOpen : MonoBehaviour, IPointerClickHandler
    {
        public const string TextSquad = "Отряд";
        public const string TextPower = "(Сила";
        [SerializeField] private Power _power;
        [SerializeField] private BookmarkButton _buttonAttack;
        [SerializeField] private TroopsEnemy _troopsEnemy;
        [SerializeField] private BookmarkButton _exitScreenBotton;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private SoldiersIconSpawn _iconSpawn;
        [SerializeField] private TMP_Text _textNumberRaid;
        [SerializeField] private TMP_Text _textPowerSquad;
        [SerializeField] private TMP_Text _textGoldReward;
        [SerializeField] private TMP_Text _textCardReward;
        [SerializeField] private TMP_Text _textSarsReward;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _stars;
        
        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;
        private IPersistenProgressService _persistenProgressService;
        private int _numberRaid;
        private float _powerValue;
        private float _maxPower;
        private int _numberScene = 1;

        private void OnEnable()
        {
            _buttonAttack.ButtonOnClic += Attack;
            _exitScreenBotton.ButtonOnClic += CloseScreen;

        }

        private void OnDisable()
        {
            _buttonAttack.ButtonOnClic -= Attack;
            _exitScreenBotton.ButtonOnClic -= CloseScreen;
        }

        private void Awake()
        {
            if (SceneManager.GetActiveScene().name != AssetPath.SceneMain)
                enabled = false;
        }

        private void Start()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _persistenProgressService = AllServices.Container.Single<IPersistenProgressService>();
        }

        private void Attack()
        {
            _saveLoadService.SaveProgress();
            _stateMachine.Enter<LoadLevelStateBattle, string, int>(AssetPath.SceneBattle, _numberScene);
        }

        private void CloseScreen()
        {
            foreach (var item in _iconSpawn.Enemy)
            {
                item.DiactivateCameraView();
                item.Deactivate();
            }
            _canvas.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _power.Finish += SetPowerText;
            if (_troopsEnemy.Id > 2 || _persistenProgressService.Progress.Training.Finish)
            {
                foreach (var item in _iconSpawn.Enemy)
                {
                    item.ActivateCAmeraView();
                }
                _powerValue = 0;
                _numberRaid = _troopsEnemy.Id;
                ButtonActivatinoEndText();
                _maxPower = _power.GetPower(_numberRaid, false);
                foreach (var item in _troopsEnemy.Soldiers)
                {
                    item.SoldiersStatsLevel.ResetLevel();
                    item.BaseData();
                }
                StartCoroutine(_power.UpLevelEnemy(_troopsEnemy.Soldiers, _maxPower));

            }
        }

        private void SetPowerText()
        {
            _power.Finish -= SetPowerText;
            _powerValue = (float)Math.Round(_power.PowerSoldier, 2);
            _textNumberRaid.text = "РЕЙД" + " #" + _numberRaid.ToString();
            string resultPower = AbbreviationsNumbers.ShortNumber(_powerValue);
            _textPowerSquad.text = $"<color=#46281d>{TextSquad}</color> <color=#92be0f>{TextPower}{resultPower}{")"}</color>";
            _textGoldReward.text = _troopsEnemy.GoldReward.ToString();
            _textCardReward.text = "1";
            if (_troopsEnemy.StarsReward != 0)
            {
                _textSarsReward.gameObject.SetActive(true);
                _stars.gameObject.SetActive(true);
                _textSarsReward.text = _troopsEnemy.StarsReward.ToString();
            }
            else
            {
                _stars.gameObject.SetActive(false);
                _textSarsReward.gameObject.SetActive(false);
            }
            _canvas.gameObject.SetActive(true);
        }

        private void ButtonActivatinoEndText()
        {
            int minId;
            foreach (SpawnPoint item in _troopsEnemy.SpawnPoints)
            {
                if (item._slain == false)
                {
                    minId = item.Id;
                    if (minId < _numberRaid)
                    {
                        _text.gameObject.SetActive(true);
                        _buttonAttack.gameObject.SetActive(false);
                    }
                    else
                    {
                        _text.gameObject.SetActive(false);
                        _buttonAttack.gameObject.SetActive(true);
                    }
                    break;
                }
            }
        }
    }
}