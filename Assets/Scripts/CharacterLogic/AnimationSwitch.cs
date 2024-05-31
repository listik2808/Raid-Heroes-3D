using Assets.Scripts.BattleLogic.StateMachine.States;
using System;
using UnityEngine;

public class AnimationSwitch : MonoBehaviour
{
    public Animator Animator;
    protected int _speed = Animator.StringToHash("Speed");
    protected int _attack = Animator.StringToHash("Attack");
    protected int _attackSpec = Animator.StringToHash("AttackSpec");
    protected int _victory = Animator.StringToHash("Victory");
    protected int _die = Animator.StringToHash("Die");
    protected int _dizzy = Animator.StringToHash("Dizzy");
    protected int _isNoStune = Animator.StringToHash("No");
    [SerializeField] private AudioSource _audioSourceDie;
    [SerializeField] private AudioClip _audioClipDie;
    private AIAgentBase _agentBase;
    private AttackState _attackState;

    public event Action<bool> DefaulteAttack;
    public event Action<bool> SpecAttacEvent;
    public event Action<bool> StartAttack;

    protected void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void SetAnimatorSpeed(float value) => Animator.speed = value;

    public void DelayAttackSpec()
    {
        StartAttack?.Invoke(true);
    }

    public void SetStateAttack(AIAgentBase aIAgentBase,AttackState attackState)
    {
        _agentBase = aIAgentBase;
        _attackState = attackState;
    }

    public void New()
    {
        Animator.ResetTrigger(_dizzy);
        Animator.SetTrigger(_isNoStune);
    }

    public void SetMovementSpeed(float value)
    {
        Animator.SetFloat(_speed, value);
    }

    public void AttackDefaulte()
    {
        _agentBase.EventAttack(false);
        DefaulteAttack?.Invoke(false);
    }

    public void AttacSpecEvent()
    {
        _agentBase.EventAttack(true);
        SpecAttacEvent?.Invoke(true);
    }

    public void Attack()
    {
        Animator.SetTrigger(_attack);
    }

    public void AttackSpec()
    {
        Animator.SetTrigger(_attackSpec);
    }

    public void Victory()
    {
        Animator.SetTrigger(_victory);
    }

    public void Die()
    {
        if(_audioSourceDie.enabled)
        {
            _audioSourceDie.clip = _audioClipDie;
            _audioSourceDie.Play();
        }

        Animator.SetTrigger(_die);
    }

    public void Dizzy()
    {
        Animator.SetTrigger(_dizzy);
    }

}
