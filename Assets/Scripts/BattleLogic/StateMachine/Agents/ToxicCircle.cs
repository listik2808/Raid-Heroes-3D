using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Army.TypesSoldiers;
using Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToxicCircle : AIAgentBase
{
    [SerializeField] private AudioClip _audioClipSpec;
    [SerializeField] private AudioClip _audioClipSword;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSourceSpec;
    [SerializeField] private AttackType _attackType;
    [SerializeField] private Animator _weapon;
    [SerializeField] private float _poisoningDelay;
    [SerializeField] private Poisoner _poisoner;
    [SerializeField] private BulletToxic _bullet;
    private int _poisoningCounter;
    private bool _killOpponent;
    private AIAgentBase _opponent;
    private List<AIAgentBase> _targetsColider = new List<AIAgentBase>();
    private int mask;
    private float _attackRadius = 2;
    Collider[] _enemiesColider;
    public override void Start()
    {
        if (Soldier.TypeSoldier == HeroType.Hero)
        {
            mask = LayerMask.NameToLayer("Enemy");
        }
        else
        {
            mask = LayerMask.NameToLayer("HeroBox");
        }
        base.Start();
        StoppingDistance = _poisoner.RadiusAction;
        NavMeshAgent.stoppingDistance = (StoppingDistance == 0 ? Config.StoppingDistance : StoppingDistance);
        StateMachine = new HeroStateMachine(this);
        RegisterStates();
        StateMachine.ChangeState(InitialState);
    }

    public override void Attack(AttackState state)
    {
        if (TargerIsNear(transform, Target) == false)
        {
            Animation.DefaulteAttack += OnAttack;
            StartAttack(false);
        }
        else
        {
            state.IsPerformance = false;
        }
    }

    public override void SpecAttack(AttackState state)
    {
        if (TargerIsNear(transform, Target) == true)
        {
            StartAttack(true);
        }
        else
        {
            state.IsPerformance = false;
        }
    }

    //public override void EventAttack(bool specAttac)
    //{
    //    OnAttack(specAttac);
    //}

    private void StartAttack(bool specAttack)
    {

        if (specAttack)
        {

            if (_weapon != null)
            {
                _weapon.SetTrigger("SpecAttack");
            }
            if (_bullet != null)
            {
                Animation.SpecAttacEvent += CreatingPoisonousSphere;
                Animation.AttackSpec();
                if (_audioSourceSpec.enabled)
                {
                    _audioSourceSpec.clip = _audioClipSpec;
                    _audioSourceSpec.Play();
                }
            }
            //else
            //{
            //    Animation.AttackSpec();
            //    ResetTimeSpecAttack();
            //}

            //_enemiesColider = Physics.OverlapSphere(Target.position, StoppingDistance, 1 << mask);
            //if (_enemiesColider != null && _enemiesColider.Length != 0)
            //{
            //    _targetsColider.Clear();
            //    //Animation.AttackSpec();
            //    //ResetTimeSpecAttack();
            //    StartPoisoning(_enemiesColider);
            //}
        }
        else
        {
            Animation.Attack();
            ResetTimeMeleeDamage();
            if (_audioSource.enabled)
            {
                _audioSource.clip = _audioClipSword;
                _audioSource.Play();
            }
        }
    }

    public void OnAttack(bool specAttack)
    {
        EnemyHeaith health = null;

        if (specAttack)
        {
            foreach (var item in _targetsColider)
            {
                Target = item.gameObject.transform;
                health = TryDamage(specAttack, health);
                AIAgentBase opponent = item;
                ToxicEndDead(health, opponent, specAttack);
            }
        }
        else
        {
            health = TryDamage(specAttack, health);
            AIAgentBase opponent = Target?.GetComponent<AIAgentBase>();
            ToxicEndDead(health, opponent, specAttack);
            Animation.DefaulteAttack -= OnAttack;
        }
        Animation.DefaulteAttack -= OnAttack;
        OnAttackEnded(specAttack);
    }

    public void OnAttackEnded(bool specAttack)
    {
        if (_killOpponent)
        {
            _opponent?.BuffAndDebuff.Poisoner.gameObject.SetActive(false);
            FindNewOpponent();
        }

        if (StateMachine?.GetState(AIStateId.Attack) is AttackState attackState)
        {
            attackState.IsPerformance = false;
        }

        _killOpponent = false;
    }

    private void StartPoisoning(Collider[] _enemiesColider,AIAgentBase aIAgentBase)
    {
        StartCoroutine(Poisoning(_enemiesColider, aIAgentBase));
    }

    private IEnumerator Poisoning(Collider[] _enemiesColider, AIAgentBase aIAgentBase)
    {

        foreach (var item in _enemiesColider)
        {
            if (item.TryGetComponent(out AIAgentBase enemy))
            {
                if(aIAgentBase.Type != enemy.Type)
                {
                    _targetsColider.Add(enemy);
                    _opponent = enemy;
                    ActivateBuff();
                }
            }
        }
        
        //yield return new WaitForSeconds(0.2f);
        OnAttack(true);
        //if (_killOpponent)
        //{
        //    _opponent?.BuffAndDebuff.Poisoner.gameObject.SetActive(false);
        //    OnAttackEnded(true);
        //    yield break;
        //}

        yield return new WaitForSeconds(_poisoningDelay);

        _poisoningCounter++;
        if (_poisoningCounter < 3)
        {
            StartPoisoning(_enemiesColider, this);
        }
        else
        {
            _opponent?.BuffAndDebuff.Poisoner.gameObject.SetActive(false);
            OnAttackEnded(true);
            _poisoningCounter = 0;
        }
    }

    private void ActivateBuff()
    {
        if (Target != null && _opponent != null)
        {
            _opponent?.BuffAndDebuff.Poisoner.gameObject.SetActive(true);
        }
    }

    private void DamageText(bool specAttack)
    {
        if (specAttack && Target != null)
        {
            DamageNumbersPro.DamageNumber damageSpec = damageNumberProSpec.Spawn(Target.transform.position, Mathf.Ceil(SpecDamage));
        }
        else if (specAttack == false && Target != null)
        {
            DamageNumbersPro.DamageNumber DamageNumber = DamageNumberProText.Spawn(Target.transform.position, Mathf.Ceil(Damage * DamageMultiplayer));
        }
    }

    private EnemyHeaith TryDamage(bool specAttack, EnemyHeaith health)
    {
        health = Target?.gameObject?.GetComponent<EnemyHeaith>();
        DamageText(specAttack);
        health?.Damage(specAttack ? Mathf.Ceil(SpecDamage) : Mathf.Ceil(Damage * DamageMultiplayer));
        return health;
    }

    private void ToxicEndDead(EnemyHeaith health, AIAgentBase opponent, bool specAttack)
    {
        if (health?.Current <= 0)
        {
            _killOpponent = true;
            if (!opponent.IsDead)
            {
                opponent.IsDead = true;
                opponent.StateMachine.ChangeState(AIStateId.Death);
            }
        }
        else if (specAttack)
        {
            opponent?.BuffAndDebuff.Stane.gameObject.SetActive(true);
            //opponent?.StateMachine?.ChangeState(AIStateId.Attack);
        }
    }

    private void CreatingPoisonousSphere(bool value)
    {
        Vector3 position = Vector3.zero;
        Animation.SpecAttacEvent -= CreatingPoisonousSphere;
        ResetTimeSpecAttack();
        if (Target != null)
        {
            position = Target.position;
        }
        else
        {
            FindNewOpponent();
        }

        BulletToxic newBullet = Instantiate(_bullet, position, Quaternion.identity);
        newBullet.ActivivateToxic();
        _enemiesColider = Physics.OverlapSphere(Target.position, _attackRadius, 1 << mask);
        if (_enemiesColider != null && _enemiesColider.Length != 0)
        {
            _targetsColider.Clear();
            StartPoisoning(_enemiesColider,this);
        }
    }
    private bool TargerIsNear(Transform hero, Transform target)
    {
        var dist = Vector3.Distance(hero.position, target.position);
        var result = dist < StoppingDistance / 3;
        Debug.LogWarning("dist " + dist + " " + (result ? "attack" : "specattack"));
        return result;
    }
}