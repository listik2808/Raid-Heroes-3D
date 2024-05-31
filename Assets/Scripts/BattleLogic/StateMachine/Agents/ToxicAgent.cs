using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Army.TypesSoldiers;
using Scripts.Enemy;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BattleLogic.StateMachine.Agents
{
    public class ToxicAgent : AIAgentBase
    {
        [SerializeField] private AudioClip _audioClipSpecSword;
        [SerializeField] private AudioClip _audioClipSword;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _audioSourceSpec;
        [SerializeField] private AttackType _attackType;
        [SerializeField] private Animator _weapon;
        [SerializeField] private float _poisoningDelay;
        [SerializeField] private Poisoner _poisoner;
        private int _poisoningCounter;
        private bool _killOpponent;
        private AIAgentBase _opponent;
        public override void Start()
        {
            base.Start();
            StoppingDistance = _poisoner.RadiusAction;
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
            //Animation.SpecAttacEvent += OnAttack;
            StartAttack(true);
        }

        public override void EventAttack(bool specAttac)
        {
            OnAttack(specAttac);
        }

        private void StartAttack(bool specAttack)
        {
            //transform.LookAt(Target);

            if (specAttack)
            {
                
                if (_weapon != null)
                {
                    _weapon.SetTrigger("SpecAttack");
                }
                StartPoisoning();
            }
            else
            {
                //Animation.SetAudioClip(_audioClipSword);
                Animation.Attack();
                ResetTimeMeleeDamage();
                if (_audioSource.enabled)
                {
                    _audioSource.clip = _audioClipSword;
                    _audioSource.Play();
                }
            }

            float onAttackDelay;
            float onAttackEndedDelay;

            if (_attackType == AttackType.Short)
            {
                onAttackDelay = 0;
                onAttackEndedDelay = 0;
            }
            else
            {
                var dist = Vector3.Distance(Target.position, transform.position);
                var speed = Config.BulletSpeed;
                onAttackDelay = dist / speed;
                onAttackEndedDelay = onAttackDelay + 0.2f;
                Debug.Log(onAttackDelay);
            }

            //how quickly will the damage be done
            //if (specAttack == false)
            //{
            //    //StartCoroutine(CallWithDelay(onAttackDelay, specAttack, OnAttack));
            //    //StartCoroutine(CallWithDelay(onAttackEndedDelay, specAttack, OnAttackEnded));
            //}
        }

        public void OnAttack(bool specAttack)
        {
            EnemyHeaith health = Target?.gameObject?.GetComponent<EnemyHeaith>();
            
            if (specAttack && Target != null)
            {
                DamageNumbersPro.DamageNumber damageSpec = damageNumberProSpec.Spawn(Target.transform.position, Mathf.Ceil(SpecDamage));
            }
            else if (specAttack == false && Target != null)
            {
                //_opponent.BuffAndDebuff.Poisoner.gameObject.SetActive(true);
                DamageNumbersPro.DamageNumber DamageNumber = DamageNumberProText.Spawn(Target.transform.position, Mathf.Ceil(Damage * DamageMultiplayer));
            }
            health?.Damage(specAttack ? Mathf.Ceil(SpecDamage) : Mathf.Ceil(Damage * DamageMultiplayer));
            if (health?.Current <= 0)
            {
                 _opponent = Target?.GetComponent<AIAgentBase>();

                _killOpponent = true;
                if (!_opponent.IsDead)
                {
                    _opponent.IsDead = true;
                    _opponent.StateMachine.ChangeState(AIStateId.Death);
                }
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

        private void StartPoisoning()
        {
            StartCoroutine(Poisoning());
        }

        private IEnumerator Poisoning()
        {
            //Animation.SetAudioClip(_audioClipSpecSword);
            if (_audioSourceSpec.enabled)
            {
                _audioSourceSpec.clip = _audioClipSpecSword;
                _audioSourceSpec.Play();
            }
            
            Animation.AttackSpec();
            ResetTimeSpecAttack();
            
            if (Target != null && _opponent != null)
            {
                _opponent?.BuffAndDebuff.Poisoner.gameObject.SetActive(true);
            }
            else
            {
                _opponent = Target?.GetComponent<AIAgentBase>();
                _opponent?.BuffAndDebuff.Poisoner.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.2f);
            OnAttack(true);
            if (_killOpponent)
            {
                _opponent?.BuffAndDebuff.Poisoner.gameObject.SetActive(false);
                OnAttackEnded(true);
                yield break;
            }

            yield return new WaitForSeconds(_poisoningDelay);

            _poisoningCounter++;
            if (_poisoningCounter < 3)
            {
                StartPoisoning();
            }
            else
            {
                _opponent?.BuffAndDebuff.Poisoner.gameObject.SetActive(false);
                OnAttackEnded(true);
                _poisoningCounter = 0;
            }
        }
    }
}
