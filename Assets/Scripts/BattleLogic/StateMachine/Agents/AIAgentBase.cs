using Assets.Scripts.BattleLogic.StateMachine;
using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Army.TypesSoldiers;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Enemy;
using DamageNumbersPro;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using UnityEngine.SceneManagement;

public enum HeroType
{
    Hero,
    Enemy
}

public enum AttackType
{
    Short,
    Long
}

public class AIAgentBase : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public BuffAndDebuff BuffAndDebuff;
    public DamageNumber DamageNumberProText;
    public DamageNumber damageNumberProSpec;
    public HeroStateMachine StateMachine;
    public AIStateId InitialState;
    public AIAgentConfig Config;
    public NavMeshAgent NavMeshAgent;
    public AnimationSwitch Animation;
    public HeroType Type;
    public Soldier Soldier;
    public EnemyHeaith Health;
    public float AttackCooldown;
    public float Damage;
    public float SpecAttackCooldown;
    public float SpecDamage;
    public bool IsDead;
    public bool UnderHypno;
    [HideInInspector] public float StoppingDistance;
    [HideInInspector] public List<AIAgentBase> Attackers = new();
    [HideInInspector] public Freeze Freeze;// заморозка на герое
    [HideInInspector] public Hypno Hypno;// гипноз на герое
    [HideInInspector] public Motivation Motivation;// мотивация на герое
    [HideInInspector] public float Stune;
    [HideInInspector] public float DamageMultiplayer = 1f;
    public Transform _target;
    public FindTarget FindTarget;

    //fields for states 
    [HideInInspector] public Vector3 StunnedDirection;
    public AIAgentBase Enemy;
    public SliderAttac SliderAttac;
    public bool IsStun = false;
    private bool _isSpecAttac = false;
    private bool _isMeleedamage = false;
    private bool _isStopMove = false;
    private IPersistenProgressService _persistenProgress;

    public bool IsStopMove => _isStopMove;
    public bool IsMeleedamage => _isMeleedamage;
    public bool ISSpecAttack => _isSpecAttac;

    private void OnEnable()
    {
        Soldier.ChangedSpecAttack += SetSpecialAttack;
        Soldier.ChangedDamage += SetMeleeDamage;
    }

    private void OnDisable()
    {
        Soldier.ChangedSpecAttack -= SetSpecialAttack;
        Soldier.ChangedDamage -= SetMeleeDamage;
    }

    public void ActivateMove()
    {
        _isStopMove = false;
    }
    public void DiativateMove()
    {
        _isStopMove = true;
    }

    public void ActivateSliderAttack()
    {
        SliderAttac.gameObject.SetActive(true);
    }

    public Transform Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
            if (value == null)
                return;

            _target.GetComponent<AIAgentBase>().Attackers.Add(this);
        }
    }

    public virtual void Start()
    {
        if(_persistenProgress == null)
        {
            _persistenProgress = AllServices.Container.Single<IPersistenProgressService>();
        }
        InitialState = AIStateId.Idle;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Health = GetComponent<EnemyHeaith>();
        if (Soldier != null)
        {
            if(StoppingDistance == 0)
            {
                StoppingDistance = Config.StoppingDistance;
            }
            
            //StoppingDistance = Soldier.DataSoldier.RangeAttack;
            AttackCooldown = Soldier.DataSoldier.DurationRecoveryMeleeDamage;
            Damage = Soldier.CurrentMeleeDamage;
            SpecDamage = Soldier.CurrenValueSpecAttack;
            SpecAttackCooldown = Soldier.DurationRecoverySpecAttack;
        }
    }

    public virtual void Update()
    {
        StateMachine?.Update();
    }

    protected virtual void RegisterStates()
    {
        StateMachine.RegisterState(new AIIdleState());
        StateMachine.RegisterState(new AIChasePlayerState());
        StateMachine.RegisterState(new AttackState());
        StateMachine.RegisterState(new AIDeathState());
        StateMachine.RegisterState(new VictoryState());
        StateMachine.RegisterState(new StunnedState());
    }

    public virtual void Attack(AttackState state) { }

    public virtual void SpecAttack(AttackState state) { }

    public virtual void EventAttack(bool value) { }

    public void StopEfects()
    {
        BuffAndDebuff.gameObject.SetActive(false);
    }

    public void TutorDamage()
    {
        Damage = Soldier.CurrentMeleeDamage;
        SpecDamage = Soldier.CurrenValueSpecAttack;
    }

    public void FindNewOpponent()
    {
        if (FindTarget == null)
        {
            FindTarget = GetComponent<FindTarget>();
        }
        

        bool result = true;

        if (UnderHypno)
            //if false then transition to idle state
            result = FindTarget.FindNearestFriend();
        else
            FindTarget.FindNearestOpponent();

        if (result == true)
            StateMachine?.ChangeState(AIStateId.ChasePlayer);
    }

    public void ResetTimeMeleeDamage()
    {
        SliderAttac.ResetTimeMeleeDamageSkill();
    }

    public void ResetTimeSpecAttack()
    {
        SliderAttac.ResetTimeSpecialSkill();
    }

    public void TimeAttac(bool value)
    {
        _isMeleedamage = value;
    }

    public void TimeSpecAttac(bool value)
    {
        _isSpecAttac = value;
    }

    private void SetSpecialAttack()
    {
        SpecDamage = Soldier.CurrenValueSpecAttack;
    }

    private void SetMeleeDamage()
    {
        Damage = Soldier.CurrentMeleeDamage;
    }
}