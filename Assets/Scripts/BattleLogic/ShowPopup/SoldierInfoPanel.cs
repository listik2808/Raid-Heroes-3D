using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoldierInfoPanel : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ShowPopup _showPopup;
    [SerializeField] private Soldier _soldier;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Sprite _spriteStarsActiv;
    [SerializeField] private Sprite _spriteStarsDiactiveta;
    [SerializeField] private List<Image> _imagesStars;
    [SerializeField] private Image _imageSpecAttac;
    [SerializeField] private TMP_Text _textSpecAttac;
    [SerializeField] private TMP_Text _textHp;
    [SerializeField] private TMP_Text _textDamage;
    [SerializeField] private TMP_Text _textPower;
    [SerializeField] private BoxCollider _boxCollider;

    private Camera _camera;
    private string _nameObject;
    private IPersistenProgressService _progressService;

    private void OnEnable()
    {
        if(_progressService == null)
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        _showPopup.OnShow += OnShow;
        _showPopup.OnHide += OnHide;
    }
    private void OnDisable()
    {
        if(_boxCollider != null)
            _boxCollider.enabled = true;

        _showPopup.OnShow -= OnShow;
        _showPopup.OnHide -= OnHide;
    }
    private void OnShow()
    {
        if(SceneManager.GetActiveScene().name != "Sandbox")
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
            _canvas.worldCamera = _camera;
        }
        //_nameObject = _soldier.AnimationSwitch.gameObject.name;
        //_nameObject = Lean.Localization.LeanLocalization.GetTranslationText(_nameObject);
        SetRang();
        _imageSpecAttac.sprite = _soldier.IconSpecAttack;
        if (_soldier.TypeSoldier == HeroType.Hero)
        {
            _nameObject = CustomRuLocalization.GetRuHeroTypeId((int)_soldier.HeroTypeId);
            _boxCollider.enabled = false;
            SetLevelEndStepHero();
        }
        else
        {
            _nameObject = CustomRuLocalization.GetRuMonster((int)_soldier.HeroTypeId);
            _soldier.BaseData();
            SetLevelEndStepEnemy();
        }
        _name.text = _nameObject;
        float value = _soldier.Power.GetUnitDPS(_soldier);
        string result = AbbreviationsNumbers.ShortNumber(value);
        _textPower.text = "Сила " + result;
    }

    private void SetLevelEndStepEnemy()
    {
        _textSpecAttac.text = _soldier.SoldiersStatsLevel.CurrentLevelSpecialSkill.ToString() + "." + _soldier.SpecialSkillLevelData.CurrentStepSkill.ToString();
        _textHp.text = _soldier.SoldiersStatsLevel.CurrentSurvivabilityLevel.ToString() + "." + _soldier.SurvivabilityLevelData.CurrentStepSkill.ToString();
        _textDamage.text = _soldier.SoldiersStatsLevel.CurrentMeleelevel.ToString() + "." + _soldier.MeleeDamageLevelData.CurrentStepSkill.ToString();
    }

    private void OnHide()
    {
    }

    private void SetLevelEndStepHero()
    {
        //foreach (Scripts.Data.TypeHeroSoldier.DataLevelSkill item in _progressService.Progress.PlayerData.TypeHero.AllHerosType.ListTypsHeros)
        //{
        //    if (item.TypeId == _soldier.HeroTypeId)
        //    {
        //        _textSpecAttac.text = item.CurrenSpecialAttackLevel.ToString() + "." + item.CurrentStepSpecialAttack.ToString();
        //        _textHp.text = item.CurrenSurvivabilityLevel.ToString() + "." + item.CurrentStepSurvivability.ToString();
        //        _textDamage.text = item.CurrenMeleeLevel.ToString() + "." + item.CurrentStepMelee.ToString();
        //        break;
        //    }
        //}
        _textSpecAttac.text = _soldier.SoldiersStatsLevel.CurrentLevelSpecialSkill.ToString() + "." + _soldier.SpecialSkillLevelData.CurrentStepSkill.ToString();
        _textHp.text = _soldier.SoldiersStatsLevel.CurrentSurvivabilityLevel.ToString() + "." + _soldier.SurvivabilityLevelData.CurrentStepSkill.ToString();
        _textDamage.text = _soldier.SoldiersStatsLevel.CurrentMeleelevel.ToString() + "." + _soldier.MeleeDamageLevelData.CurrentStepSkill.ToString();
    }

    private void SetRang()
    {
        int count = _soldier.Rank.CurrentLevelHero;
        if (count > 0)
        {
            for (int i = 0; i < _imagesStars.Count; i++)
            {
                if (count > 0)
                {
                    _imagesStars[i].sprite = _spriteStarsActiv;
                    count--;
                }
            }
        }
        else
        {
            foreach (var item in _imagesStars)
            {
                item.sprite = _spriteStarsDiactiveta;
            }
        }
    }
}