using Scripts.BattleTactics;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Logic;
using Scripts.Music;
using Scripts.NextScene;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StartBattleButton : MonoBehaviour
{
    [SerializeField] private SoundBattle _soundBattle;
    [SerializeField] private BookmarkButton _buttonRemuve;
    [SerializeField] private BookmarkButton _buttonStart;
    [SerializeField] private TimerBattle _timerBattle;
    [SerializeField] private BattleSpeed _battleSpeed;
    [SerializeField] private SoldierClick _soldierClick;
    [SerializeField] private Image _image;
    [SerializeField] private BookmarkButton _backScene;
    private IPersistenProgressService _progressService;

    private ZoneCell _zoneCell;
    private ActivateSpawnerEnemySoldier _activateSpawnerEnemySoldier;

    private void OnEnable()
    {
        _buttonStart.ButtonOnClic += Battle;
    }

    public void Construct(ZoneCell zoneCell, ActivateSpawnerEnemySoldier activateSpawnerEnemySoldier,IPersistenProgressService persistenProgress)
    {
        _zoneCell = zoneCell;
        _activateSpawnerEnemySoldier = activateSpawnerEnemySoldier;
        _progressService = persistenProgress;
        if(_progressService != null )
        {
            ActivateOrDiactivateTutor();
        }
        else
        {
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
            ActivateOrDiactivateTutor();
        }
    }

    private void ActivateOrDiactivateTutor()
    {
        if (_progressService.Progress.PointSpawn.IdSpawnerEnemy <= 2 && _progressService.Progress.Training.Tutor == false)
        {
            _image.gameObject.SetActive(true);
            _battleSpeed.gameObject.SetActive(false);
            _backScene.gameObject.SetActive(false);
        }
        else
        {
            _backScene.gameObject.SetActive(true);
            _battleSpeed.gameObject.SetActive(true);
            _image.gameObject.SetActive(false);
        }
    }

    public void Battle()
    {
        _soundBattle.PlayMusicBasedOnCondition();
        DiacivateCollider(_zoneCell);
        //_activateSpawnerEnemySoldier.SetTutorDamage();
        _soldierClick.gameObject.SetActive(false);
        _buttonStart.ButtonOnClic -= Battle;
        _buttonRemuve.gameObject.SetActive(false);
        _buttonStart.gameObject.SetActive(false);
        _timerBattle.enabled = true;
        _battleSpeed.gameObject.SetActive(false);
        foreach (var item in HeroEnemyList.Enemies)
        {
            var enemy = item.GetComponent<FindTarget>();
            enemy.FindNearestOpponent();
            enemy.Agent.StateMachine.ChangeState(AIStateId.ChasePlayer);
        }
        foreach (var item in HeroEnemyList.Heroes)
        {
            var hero = item.GetComponent<FindTarget>();
            hero.FindNearestOpponent();
            hero.Agent.StateMachine.ChangeState(AIStateId.ChasePlayer);
            hero.gameObject.layer = LayerMask.NameToLayer("HeroBox");
        }
    }

    private void DiacivateCollider(ZoneCell zoneCell)
    {
        foreach (PlayerCell item in zoneCell.PlayerCells)
        {
            item.Collider.enabled = false;
        }
    }
}
