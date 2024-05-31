using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.BattleLogic.StateMachine.Agents
{
    public class StunAgent : AIAgentBase
    {
        public const double Gravity = 9.8;
        [SerializeField] private AttackType _attackType;
        [SerializeField] private Animator _weapon;
        [SerializeField] private AudioClip _audioClipStun;
        [SerializeField] private AudioClip _audioClipSword;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _audioSourceSpec;
        private bool _killOpponent;
        private int mask;
        private float _speed;
        private List<AIAgentBase> _enemies = new List<AIAgentBase>();
        AIAgentBase _opponent;
        EnemyHeaith _health;
        public override void Start()
        {
            if(Soldier.TypeSoldier == HeroType.Hero)
            {
                mask = LayerMask.NameToLayer("Enemy");
            }
            else
            {
                mask = LayerMask.NameToLayer("HeroBox");
            }
            
            base.Start();
            StoppingDistance = Config.StoppingDistance;// устанавливаем дистанцию
            NavMeshAgent.stoppingDistance = StoppingDistance;// присваеваем дистанцию на которой мы остановимся и не будем идти 
            StateMachine = new HeroStateMachine(this);
            RegisterStates();
            StateMachine.ChangeState(InitialState);
        }

        public override void Attack(AttackState state)
        {
            //Animation.DefaulteAttack += OnAttack;
            StartAttack(false);
        }

        public override void SpecAttack(AttackState state)
        {
            //Animation.SpecAttacEvent += OnAttack;
            //if ((gameObject.transform.position - Target.transform.position).sqrMagnitude < StoppingDistance * 2)
            //{
            //    StartAttack(true);
            //}
            //else
            //{
            //    state.IsPerformance = false;
            //}
            StartAttack(true);
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

                Collider[] enemies = Physics.OverlapSphere(gameObject.transform.position, StoppingDistance, 1 << mask);
                if (enemies != null && enemies.Length != 0)
                {
                    _enemies?.Clear();
                    Shortlist(enemies);
                    //_enemies = enemies;
                    //Animation.SetAudioClip(_audioClipStun);
                    Animation.AttackSpec();
                    ResetTimeSpecAttack();
                    if(_enemies != null)
                    {
                        int index = UnityEngine.Random.Range(0, _enemies.Count);
                        if (_enemies[index].TryGetComponent(out AIAgentBase enemy))
                        {
                            // Vector3 forward = enemies[index].transform.position - gameObject.transform.position;
                            //enemy.StunnedDirection = forward;
                            Enemy = enemy;
                            enemy.Enemy = this;
                            enemy.Stune = Soldier.TimeSpecialSkill;
                            Target = enemies[index].transform;
                        }
                    }
                    if (_audioSourceSpec.enabled)
                    {
                        _audioSourceSpec.clip = _audioClipStun;
                        _audioSourceSpec.Play();
                    }
                    
                    //OnAttack(specAttack);
                }
            }
            else
            {
                Enemy = Target.GetComponent<AIAgentBase>();
                //Animation.SetAudioClip(_audioClipSword);
                Animation.Attack();
                ResetTimeMeleeDamage();
                if (_audioSource.enabled)
                {
                    _audioSource.clip = _audioClipSword;
                    _audioSource.Play();
                }
                
            }
        }

        private void Shortlist(Collider[] enemies)
        {
            foreach (var item in enemies)
            {
                if(item.TryGetComponent(out AIAgentBase aI))
                {
                    if(aI.IsDead == false)
                    {
                        _enemies.Add(aI);
                    }
                }
            }
        }

        private void GetopponentActive()
        {
            if (Enemy.IsDead == false)
            {
                _opponent = Enemy;
                _health = Enemy.Health;
            }
            else
            {
                _health = Target?.gameObject?.GetComponent<EnemyHeaith>();
                _opponent = Target?.GetComponent<AIAgentBase>();
            }
        }

        public void OnAttack(bool specAttack)
        {
            GetopponentActive();

            if (specAttack && Target != null)
            {
                Target = Enemy.IsDead == false ? Enemy.transform : Target;
                DamageNumbersPro.DamageNumber damageSpec = damageNumberProSpec.Spawn(Target.transform.position, Mathf.Ceil(SpecDamage * DamageMultiplayer));
            }
            else if(specAttack == false && Target != null)
            {
                DamageNumbersPro.DamageNumber DamageNumber = DamageNumberProText.Spawn(Target.transform.position, Mathf.Ceil(Damage * DamageMultiplayer));
            }
            _health?.Damage(specAttack ? Mathf.Ceil(SpecDamage) : Mathf.Ceil(Damage * DamageMultiplayer));

            GetopponentActive();
            if (_health?.Current <= 0)
            {
                _killOpponent = true;
                if (!_opponent.IsDead)
                {
                    _opponent.IsDead = true;
                    _opponent.StateMachine.ChangeState(AIStateId.Death);
                }
            }
            else if(specAttack)
            {
                if (Enemy == _opponent)
                {
                    _opponent.Enemy = this;
                    _opponent?.BuffAndDebuff.Stane.gameObject.SetActive(true);
                    _opponent?.StateMachine?.ChangeState(AIStateId.Stunned);
                }
            }
            //Animation.DefaulteAttack -= OnAttack;
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
            }

            _killOpponent = false;
        }

        private IEnumerator CallWithDelay(float delay,AIAgentBase aIAgentBase ,bool specAttack, Action<bool> action)
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

