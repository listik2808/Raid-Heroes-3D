using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Enemy;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.BattleLogic.StateMachine.Agents
{
    public class MotivationAgent : AIAgentBase
    {
        [SerializeField] private AudioClip _audioClipSpec;
        [SerializeField] private AudioClip _audioClipSword;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _audioSourceSpec;
        [SerializeField] private AttackType _attackType;
        [SerializeField] private Animator _weapon;
        [SerializeField] private Motivation _motivation;
        [SerializeField] private float _motivationTime;
        private bool _killOpponent;

        public override void Start()
        {
            base.Start();
            StoppingDistance = Config.StoppingDistance;
            NavMeshAgent.stoppingDistance = StoppingDistance;
            StateMachine = new HeroStateMachine(this);
            RegisterStates();
            StateMachine.ChangeState(InitialState);
        }

        public override void Attack(AttackState state)
        {
            Animation.DefaulteAttack += OnAttack;
            StartAttack(false);
        }

        public override void SpecAttack(AttackState state)
        {
            Animation.SpecAttacEvent += OnAttack;
            StartAttack(true);
        }

        public override void EventAttack(bool specAttac)
        {
            OnAttack(specAttac);
        }

        private void StartAttack(bool specAttack)
        {
            transform.LookAt(Target);

            if (specAttack)
            {
                var motivatedAgent = HeroEnemyList.FindStrongestAgentWithoutMotivation(Type);
                if (motivatedAgent != null)
                {
                    Animation.AttackSpec();
                    ResetTimeSpecAttack();
                    if (_weapon != null)
                    {
                        _weapon.SetTrigger("SpecAttack");
                    }
                    var motivation = Instantiate(_motivation);
                    if (_audioSourceSpec.enabled)
                    {
                        _audioSourceSpec.clip = _audioClipSpec;
                        _audioSourceSpec.Play();
                    }
                   
                    motivation?.StartMotivation(motivatedAgent, SpecDamage, _motivationTime);
                }
                else
                {
                    Animation.SpecAttacEvent -= OnAttack;
                    OnAttackEnded(specAttack);
                    if (_audioSourceSpec.enabled)
                    {
                        _audioSourceSpec.clip = _audioClipSpec;
                        _audioSourceSpec.Play();
                    }
                }
                //OnAttackEnded(specAttack);
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

            //float onAttackDelay;
            //float onAttackEndedDelay;

            //if (_attackType == AttackType.Short)
            //{
            //    onAttackDelay = 0;
            //    onAttackEndedDelay = 0;
            //}
            //else
            //{
            //    var dist = Vector3.Distance(Target.position, transform.position);
            //    var speed = Config.BulletSpeed;
            //    onAttackDelay = dist / speed;
            //    onAttackEndedDelay = onAttackDelay + 0.2f;
            //}

            //how quickly will the damage be done
            //StartCoroutine(CallWithDelay(onAttackDelay, specAttack, OnAttack));
            //StartCoroutine(CallWithDelay(onAttackEndedDelay, specAttack, OnAttackEnded));
        }

        public void OnAttack(bool specAttack)
        {
            var health = Target?.gameObject?.GetComponent<EnemyHeaith>();
            if (specAttack== false && Target != null)
            {
                DamageNumbersPro.DamageNumber DamageNumber = DamageNumberProText.Spawn(Target.transform.position, Mathf.Ceil(Damage * DamageMultiplayer));
            }
            health?.Damage(specAttack ? Mathf.Ceil(SpecDamage): Mathf.Ceil(Damage * DamageMultiplayer));
            if (health?.Current <= 0)
            {
                _killOpponent = true;
                var opponent = Target?.GetComponent<AIAgentBase>();
                if (!opponent.IsDead)
                {
                    opponent.IsDead = true;
                    opponent.StateMachine.ChangeState(AIStateId.Death);
                }
            }
            Animation.DefaulteAttack -= OnAttack;
            Animation.SpecAttacEvent -= OnAttack;
            OnAttackEnded(specAttack);
        }

        public void OnAttackEnded(bool specAttack)
        {
            if (_killOpponent)
            {
                FindNewOpponent();
            }

            if (StateMachine?.GetState(AIStateId.Attack) is AttackState attackState)
            {
                attackState.IsPerformance = false;
                //if (specAttack)
                //{
                //    attackState.SpecAttackCooldownTimer = SpecAttackCooldown;
                //    attackState.SpecAttackEnded = true;
                //}
                //else
                //{
                //    attackState.AttackCooldownTimer = AttackCooldown;
                //    attackState.AttackEnded = true;
                //}
            }

            _killOpponent = false;
        }

        private IEnumerator CallWithDelay(float delay, bool specAttack, Action<bool> action)
        {
            if (IsDead == true) yield return null;

            yield return new WaitForSeconds(delay);

            action?.Invoke(specAttack);
        }

        private IEnumerator CallWithDelay(float delay, Action action)
        {
            if (IsDead == true) yield return null;

            yield return new WaitForSeconds(delay);

            action?.Invoke();
        }

        private bool IsNear(AIAgentBase agent)
        {
            return (agent.Target.position - agent.transform.position).sqrMagnitude + 0.5f < agent.Config.StoppingDistance * agent.Config.StoppingDistance;
        }
    }
}

