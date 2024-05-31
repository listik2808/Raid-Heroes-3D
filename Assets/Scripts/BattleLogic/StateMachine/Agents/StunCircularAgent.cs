using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.BattleLogic.StateMachine.Agents
{
    public class StunCircularAgent : AIAgentBase
    {
        [SerializeField] private AudioClip _audioClipSpecStunCircular;
        [SerializeField] private AudioClip _audioClipSword;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _audioSourceSpec;
        [SerializeField] private AttackType _attackType;
        [SerializeField] private Animator _weapon;
        private bool _killOpponent;
        private int mask;
        private float _speed;
        private List<Transform> _targetsColider = new List<Transform>();
        private List<AIAgentBase> _enemyAi = new List<AIAgentBase>();
        private AttackState _attackState;

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
            StoppingDistance = Config.StoppingDistance;
            NavMeshAgent.stoppingDistance = StoppingDistance;
            StateMachine = new HeroStateMachine(this);
            RegisterStates();
            StateMachine.ChangeState(InitialState);
        }

        public override void Attack(AttackState state)
        {
            //Animation.DefaulteAttack += OnAttack;
            //StartAttack(false);
            _attackState =state;
            if ((gameObject.transform.position - Target.transform.position).sqrMagnitude < StoppingDistance * StoppingDistance)
            {
               // Animation.DefaulteAttack += OnAttack;
                StartAttack(false);
            }
            else
            {
                state.IsPerformance = false;
            }
        }

        public override void SpecAttack(AttackState state)
        {
            _attackState = state;
            if ((gameObject.transform.position - Target.transform.position).sqrMagnitude < StoppingDistance * StoppingDistance)
            {
                //Animation.SpecAttacEvent += OnAttack;
                StartAttack(true);
            }
            else
            {
                state.IsPerformance = false;
            }
            //StartAttack(true);
        }

        public override void EventAttack(bool specAttac)
        {
            OnAttack(specAttac);
        }

        private void StartAttack(bool specAttack)
        {
            if (specAttack)
            {
                if (_weapon != null)
                {
                    _weapon.SetTrigger("SpecAttack");
                }
                Collider[] _enemiesColider = Physics.OverlapSphere(gameObject.transform.position, StoppingDistance, 1 << mask);
                if (_enemiesColider != null && _enemiesColider.Length != 0)
                {
                    _targetsColider.Clear();
                    Animation.AttackSpec();
                    ResetTimeSpecAttack();
                    _enemyAi.Clear();
                    if (_audioSourceSpec.enabled)
                    {
                        _audioSourceSpec.clip = _audioClipSpecStunCircular;
                        _audioSourceSpec.Play();
                    }
                    
                    foreach (var item in _enemiesColider)
                    {
                        if (item.TryGetComponent(out AIAgentBase enemy))
                        {
                            _targetsColider.Add(item.gameObject.transform);
                            //var forward = new Vector3(0, 0, transform.localPosition.x - this.Soldier.DataSoldier.DistanceFigtMelle * 4);
                            //var forward = item.transform.position + gameObject.transform.position;
                            //var up = item.transform.position + new Vector3(0, forward.magnitude, 0);
                            //var direction = up - forward;
                            // direction = direction + new Vector3(0, 0, this.Soldier.DataSoldier.DistanceFigtMelle * 4);
                            //var direction = new Vector3(forward.x, 0, forward.z);
                            //Vector3 forceDirection = Target.transform.InverseTransformDirection(Vector3.back * (this.Soldier.DataSoldier.DistanceFigtMelle * 4));
                            //enemy.StunnedDirection = direction.normalized * _throwPower;
                            //enemy.StunnedDirection = forceDirection * _throwPower;
                            //Vector3 relativePosition = Rigidbody.transform.InverseTransformPoint(enemy.Rigidbody.position);
                            //enemy.StunnedDirection = relativePosition;
                            _enemyAi.Add(enemy);
                            enemy.Stune = Soldier.TimeSpecialSkill;
                        }
                    }
                    //OnAttack(specAttack);
                }
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
            //    onAttackDelay = 0.2f;
            //    onAttackEndedDelay = 0.4f;
            //}
            //else
            //{
            //    var dist = Vector3.Distance(Target.position, transform.position);
            //    var speed = Config.BulletSpeed;
            //    onAttackDelay = dist / speed;
            //    onAttackEndedDelay = onAttackDelay + 0.2f;
            //}
        }

        public void OnAttack(bool specAttack)
        {
            EnemyHeaith health;
            if (specAttack)
            {
                foreach (var item in _enemyAi)
                {
                    //Target = item.transform;
                    health = TryDamage(specAttack, item);
                    AIAgentBase opponent = item;
                    opponent.Enemy = this;
                    StunEndDead(health, opponent,specAttack);
                }
            }
            else
            {
                AIAgentBase opponent = Target?.GetComponent<AIAgentBase>();
                if(opponent.IsDead == false)
                {
                    health = TryDamage(specAttack, opponent);
                    
                }
                else
                {
                    health = opponent.Health;
                }

                StunEndDead(health, opponent, specAttack);
                //Animation.DefaulteAttack -= OnAttack;
            }

            //health = Target?.gameObject?.GetComponent<EnemyHeaith>();
            //DamageText(specAttack);
            //health?.Damage(specAttack ? SpecDamage : Damage * DamageMultiplayer);
            //AIAgentBase opponent = Target?.GetComponent<AIAgentBase>();
            
            //Animation.DefaulteAttack -= OnAttack;
            OnAttackEnded(specAttack);
        }

        public void OnAttackEnded(bool specAttack)
        {
            if (_killOpponent)
            {
                FindNewOpponent();
            }

            _attackState.IsPerformance = false;

            _killOpponent = false;
        }

        private IEnumerator CallWithDelay(float delay, AIAgentBase aIAgentBase, bool specAttack, Action<bool> action)
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

        private void DamageText(bool specAttack,AIAgentBase aIAgentBase)
        {
            if (specAttack && aIAgentBase.IsDead == false)
            {
                DamageNumbersPro.DamageNumber damageSpec = damageNumberProSpec.Spawn(aIAgentBase.transform.position, Mathf.Ceil(SpecDamage * DamageMultiplayer));
            }
            else if (specAttack == false && aIAgentBase.IsDead == false)
            {
                DamageNumbersPro.DamageNumber DamageNumber = DamageNumberProText.Spawn(aIAgentBase.transform.position, Mathf.Ceil(Damage * DamageMultiplayer));
            }
        }

        private EnemyHeaith TryDamage(bool specAttack,AIAgentBase aIAgentBase)
        {
            EnemyHeaith health = aIAgentBase.Health;
            if (aIAgentBase.IsDead == false && health.Current > 0)
            {
                DamageText(specAttack, aIAgentBase);
                health?.Damage(specAttack ? Mathf.Ceil(SpecDamage) : Mathf.Ceil(Damage * DamageMultiplayer));
                return health;
            }
            return health;
        }

        private void StunEndDead(EnemyHeaith health, AIAgentBase opponent,bool specAttack)
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
                opponent?.StateMachine?.ChangeState(AIStateId.Stunned);
            }
        }
    }
}

