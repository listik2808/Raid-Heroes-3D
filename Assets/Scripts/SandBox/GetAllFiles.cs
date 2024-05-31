using Scripts.Army.TypesSoldiers;
using Scripts.BattleTactics;
using Scripts.Logic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GetAllFiles : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private RectTransform _heroesButtonsParent;
    [SerializeField] private ZoneCell _heroesCells;
    [SerializeField] private ActivateSpawnerEnemySoldier _enemiesCells;
    private GameObject[] _heroes;
    private GameObject[] _enemies;
    //private bool _isActiveAchki = false;
    private int _levelMeleeDamage;
    private int _levelSpecAttack;
    private int _levelHp;
    private int _stepMeleedamage;
    private int _stepSpecAttack;
    private int _stepHp;
    private float _maxLevel;
    private void Start()
    {
        Construct();
        ShowHeroes();
    }
    public void Construct()
    {
        _heroes = Resources.LoadAll<GameObject>("Enemies/Heroes");
        _enemies = Resources.LoadAll<GameObject>("Enemies/Battle");
    }

    public void ShowHeroes()
    {
        ClearList();
        Show(isHero: true);
    }

    public void ShowEnemies()
    {
        ClearList();
        Show(isHero: false);
    }

    private void Show(bool isHero)
    {
        var go = isHero ? _heroes : _enemies;
        foreach (GameObject hero in go)
        {
            if (hero.name == "HeroVariant" || hero.name == "EnemyVariant")
                continue;

            var item = Instantiate(_itemPrefab, _heroesButtonsParent).GetComponent<HeroItem>();
            item.Name.text = hero.name.Substring(0, hero.name.Length - "Variant".Length - 1);
            item.name = item.Name.text;

            UnityAction action = isHero ? () => ButtonClickHero(hero, item) : () => ButtonClickEnemy(hero, item);
            item.Spawn.onClick.AddListener(action);
        }

        _heroesButtonsParent.sizeDelta = new Vector2(_heroesButtonsParent.sizeDelta.x, 100 * go.Length *2.3f);
    }

    private void ButtonClickHero(GameObject prefab, HeroItem item)
    {
        var soldier = prefab.GetComponent<Soldier>();
        var cells = _heroesCells.PlayerCells;
        var i = Random.Range(0, cells.Count);
        var tries = 0;
        while (cells[i].IsBusy == true && tries++ < 15)
        {
            i = Random.Range(0, cells.Count);
        }

        if (cells[i].IsBusy)
        {
            return;
        }

        var sol = cells[i].Spawn(soldier);
        InitHeroLevelSkills(item, sol); // transfer input values (level, step) to PlayerData.Heroes

        //var soldier = prefab.GetComponent<Soldier>();
        soldier.SetInstalled();
    }

    private void InitHeroLevelSkills(HeroItem item,Soldier soldier ,bool isHero = true)
    {
        _maxLevel = soldier.SoldiersStatsLevel.MaxLevelParam(soldier.Rank);
        _levelMeleeDamage = (int.TryParse(item.DamagelLevel.text, out int value) ? value : 1);
        _levelMeleeDamage = CheckingMaximum(_levelMeleeDamage);
        _levelSpecAttack = (int.TryParse(item.SpecDamageLevel.text, out int value2) ? value2 : 1);
        _levelSpecAttack = CheckingMaximum(_levelSpecAttack);
        _levelHp = (int.TryParse(item.SurvivalLevel.text, out int value3) ? value3 : 1);
        _levelHp = CheckingMaximum(_levelHp);

        _stepMeleedamage = (int.TryParse(item.DamageStep.text, out int value4) ? value4 : 0);
        _stepSpecAttack = (int.TryParse(item.SpecDamageStep.text, out int value5) ? value5 : 0);
        _stepHp = (int.TryParse(item.SurvivalStep.text, out int value6) ? value6 : 0);

        soldier.SoldiersStatsLevel.SetMeleeLevel(_levelMeleeDamage);
        soldier.SoldiersStatsLevel.SetSpecAttac(_levelSpecAttack); 
        soldier.SoldiersStatsLevel.SetHealthLevel(_levelHp);

        soldier.MeleeDamageLevelData.SetStepSkill(_stepMeleedamage);
        soldier.SpecialSkillLevelData.SetStepSkill(_stepSpecAttack);
        soldier.SurvivabilityLevelData.SetStepSkill(_stepHp);
        soldier.BaseData();
        soldier.SetMeleeDamage(soldier.MeleeDamageLevelData.ValueUpDamage);
        soldier.SpecialSkillUpgrade();
        soldier.SetCurrenHealth(soldier.SurvivabilityLevelData.ValueUpHealth);
    }

    private int CheckingMaximum(int value)
    {
        if(_maxLevel > value)
        {
            return value;
        }
        else
        {
            value = (int)_maxLevel;
            return value;
        }
    }

    private void ButtonClickEnemy(GameObject prefab, HeroItem item)
    {
        var soldier = prefab.GetComponent<Soldier>();
        var cells = _enemiesCells.EnemyCells;
        var i = Random.Range(0, cells.Count);
        var tries = 0;
        while (cells[i].IsBusy == true && tries++ < 15)
        {
            i = Random.Range(0, cells.Count);
        }

        if (cells[i].IsBusy)
        {
            return;
        }

        var sol = cells[i].Spawn(soldier);
        InitHeroLevelSkills(item, sol, false);

        soldier.SetInstalled();
    }

    private void ClearList()
    {
        ClearChildren(_heroesButtonsParent.transform);
    }

    public void ClearChildren(Transform t)
    {
        var children = t.Cast<Transform>().ToArray();

        foreach (var child in children)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}