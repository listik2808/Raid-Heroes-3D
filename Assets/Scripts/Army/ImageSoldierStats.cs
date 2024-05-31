using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Logic;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Army
{
    public class ImageSoldierStats : MonoBehaviour
    {
        [SerializeField] private BookmarkButton _buttonOpenStats;
        [SerializeField] private GameObject _background;
        [SerializeField] private Image _iconSpecAttack;
        [SerializeField] private TMP_Text _textLevelSpecAttack;
        [SerializeField] private TMP_Text _textLevelHealth;
        [SerializeField] private TMP_Text _textLevelDamage;
        [SerializeField] private TMP_Text _classSoldiers;
        [SerializeField] private TMP_Text _powerSoldier;
        [SerializeField] private List<Image> _iconStars = new List<Image>();
        [SerializeField] private Sprite _defaultStars;
        [SerializeField] private Sprite _upStars;
        [SerializeField] private BookmarkButton _closeBak;
        [SerializeField] private RawImage _iconEnemy;
        private CameraParentEnemy _parentEnemy;
        private int _number = 0;
        private Soldier _soldier;
        private bool _isOpen = false;
        private float _powerSquad = 0;
        private string _resulrPower;
        private string _name;

        public int Number => _number;

        public event Action <int> OpenStatsSoldier;

        private void OnEnable()
        {
            _buttonOpenStats.ButtonOnClic += OpenCanvasStats;
            //_closeBak.ButtonOnClic += Deactivate;
        }

        private void OnDisable()
        {
            _buttonOpenStats.ButtonOnClic -= OpenCanvasStats;
            //_closeBak.ButtonOnClic -= Deactivate;
        }

        public void SetSpriteEnemy(Soldier soldier,int number,List<CameraParentEnemy> cameraParentEnemies,AudioSource audioSource)
        {
            _soldier = soldier;
            _number = number;
            _buttonOpenStats.SetAudioSource(audioSource);
            foreach (var item in cameraParentEnemies)
            {
                if (item.EnemyView.MonsterTypeId == soldier.MonsterTypeId) 
                {
                    _parentEnemy = item;
                    _iconEnemy.texture = _parentEnemy.RenderTexture;
                    break;
                }
            }
        }

        public void Activate()
        {
            _isOpen = true;
            _background.gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _isOpen = false;
            _background.gameObject.SetActive(false);
        }

        public void ActivateCAmeraView()
        {
            _parentEnemy.gameObject.SetActive(true);
        }

        public void DiactivateCameraView()
        {
            _parentEnemy.gameObject.SetActive(false);
        }

        private void OpenCanvasStats()
        {
            if (_isOpen == false)
            {
                _powerSquad = _soldier.Power.GetUnitDPS(_soldier);
                _powerSquad = (float)Math.Round(_powerSquad, 2);
                OpenStatsSoldier?.Invoke(_number);
                _iconSpecAttack.sprite = _soldier.IconSpecAttack;
                //_name = _soldier.AnimationSwitch.gameObject.name;
                //_name = Lean.Localization.LeanLocalization.GetTranslationText(_name);
                _name = CustomRuLocalization.GetRuMonster((int)_soldier.MonsterTypeId);
                _classSoldiers.text = _name;
                SetLevelEndStep();
                _resulrPower = AbbreviationsNumbers.ShortNumber(_powerSquad);
                _powerSoldier.text = "Сила " + _resulrPower;
                Activate();
                _powerSquad = 0;
            }
        }

        private void SetLevelEndStep()
        {
            _textLevelSpecAttack.text = _soldier.SoldiersStatsLevel.CurrentLevelSpecialSkill + "." + _soldier.SpecialSkillLevelData.CurrentStepSkill.ToString();
            _textLevelHealth.text = _soldier.SoldiersStatsLevel.CurrentSurvivabilityLevel + "." + _soldier.SurvivabilityLevelData.CurrentStepSkill.ToString();
            _textLevelDamage.text = _soldier.SoldiersStatsLevel.CurrentMeleelevel + "." + _soldier.MeleeDamageLevelData.CurrentStepSkill.ToString();
        }
    }
}
