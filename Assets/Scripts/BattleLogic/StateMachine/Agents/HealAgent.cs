using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Army.TypesSoldiers;
using Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BattleLogic.StateMachine.Agents
{
    public class HealAgent : AIAgentBase
    {
        [SerializeField] private AudioClip _audioClipHp;
        [SerializeField] private AudioClip _audioClipSword;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _audioSourceSpec;
        [SerializeField] private AttackType _attackType;
        [SerializeField] private Animator _weapon;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _bulletSpawn;
        private bool _killOpponent;
        private AIAgentBase _agentBase;
        EnemyHeaith _solderHeal;
        private AttackState _attackState;

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
            _attackState = state;
            if ((Target.position - gameObject.transform.position).sqrMagnitude < StoppingDistance * StoppingDistance)
            {
                //Animation.DefaulteAttack += OnAttack;
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
            //Animation.SpecAttacEvent += HealSoldier;
            StartAttack(true);
        }

        public override void EventAttack(bool specAttac)
        {
            if (specAttac)
            {
                HealSoldier(specAttac);
            }
            else
            {
                OnAttack(specAttac);
            }

        }

        private void StartAttack(bool specAttack)
        {
            if (!specAttack)
            {
                //Animation.SetAudioClip(_audioClipSword);
                Animation.Attack();
                ResetTimeMeleeDamage();
                if (_audioSource.enabled)
                {
                    _audioSource.clip = _audioClipSword;
                    _audioSource.Play();
                }
                
                //Система выстрела 
                //if (_bullet != null)
                //{
                //    var position = _bulletSpawn ? _bulletSpawn.position : transform.position;
                //    var newBullet = Instantiate(_bullet, position, transform.rotation);
                //    if (newBullet.TryGetComponent(out Bullet bullet))
                //    {
                //        bullet.SetHeal(this);
                //    }
                //    Debug.Log(newBullet.transform.position);
                //    Rigidbody bulletRB;
                //    if (newBullet.TryGetComponent(out bulletRB) == false)
                //        bulletRB = newBullet.AddComponent<Rigidbody>();
                //    bulletRB.useGravity = false;

                //    bulletRB.velocity = (Target.position - position + new Vector3(0f, UnityEngine.Random.Range(0.3f, 1f), 0f)).normalized * Config.BulletSpeed;
                //}
            }
            else if(specAttack)
            {
                if (Type == HeroType.Hero)
                {
                    _solderHeal = Heal.FindHeroToHeal(HeroEnemyList.Heroes);
                    
                    if (_solderHeal != null)
                    {
                        // Animation.StartAttack += OnAttack;
                        //Animation.SetAudioClip(_audioClipHp);
                        Animation.AttackSpec();
                        ResetTimeSpecAttack();
                        if (_audioSourceSpec.enabled)
                        {
                            _audioSourceSpec.clip = _audioClipHp;
                            _audioSourceSpec.Play();
                        }
                        
                        _agentBase = _solderHeal.Soldier.Agent;
                        _agentBase.BuffAndDebuff.Healing.gameObject.SetActive(true);
                    }
                    else
                    {
                        _attackState.IsPerformance = false;
                    }
                }
                else if (Type == HeroType.Enemy)
                {
                    _solderHeal = Heal.FindHeroToHeal(HeroEnemyList.Enemies);
                    if (_solderHeal != null)
                    {
                        Animation.StartAttack += OnAttack;
                        Animation.AttackSpec();
                        ResetTimeSpecAttack();
                        if (_audioSourceSpec.enabled)
                        {
                            _audioSourceSpec.clip = _audioClipHp;
                            _audioSourceSpec.Play();
                        }
                        //_agentBase = _solderHeal.Soldier.Agent;
                        //_agentBase.BuffAndDebuff.Healing.gameObject.SetActive(true);
                    }
                    else
                    {
                        _attackState.IsPerformance = false;
                    }
                }
                else
                {
                    _attackState.IsPerformance = false;
                }
            }
        }

        private void HealSoldier(bool specAttack)
        {
            if(Type == HeroType.Hero)
            {
                OnAttack(specAttack);
            }
            else
            {
                _agentBase = _solderHeal.Soldier.Agent;
                _agentBase.BuffAndDebuff.Healing.gameObject.SetActive(true);
                float hpCurrent = _solderHeal.Damage(Mathf.Ceil(-SpecDamage));
                DamageNumbersPro.DamageNumber damageSpec = damageNumberProSpec.Spawn(_solderHeal.transform.position, Mathf.Ceil(hpCurrent));
            }
        }

        public void OnAttack(bool specAttack)
        {
            var health = Target?.gameObject?.GetComponent<EnemyHeaith>();
            if(Target != null && specAttack == false)
            {
                DamageNumbersPro.DamageNumber DamageNumber = DamageNumberProText.Spawn(Target.transform.position, Mathf.Ceil(Damage * DamageMultiplayer));
                health?.Damage(specAttack ? Mathf.Ceil(SpecDamage) : Mathf.Ceil(Damage * DamageMultiplayer));
                //Animation.DefaulteAttack -= OnAttack;
            }
            else if (Target != null && specAttack && Type == HeroType.Hero)
            {
                float hpCurrent = _solderHeal.Damage(Mathf.Ceil(-SpecDamage));
                DamageNumbersPro.DamageNumber damageSpec = damageNumberProSpec.Spawn(_solderHeal.transform.position, Mathf.Ceil(hpCurrent));
            }
            else if(specAttack && Type == HeroType.Enemy)
            {
                Animation.StartAttack -= OnAttack;
            }

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
            _agentBase?.BuffAndDebuff.Healing.gameObject.SetActive(false);
        }

        private IEnumerator CallWithDelay(float delay, bool specAttack)
        {
            if (IsDead == true) yield return null;

            yield return new WaitForSeconds(delay);
            if(_agentBase != null)
            {
                _agentBase.BuffAndDebuff.Healing.gameObject.SetActive(false);
            }
            //action?.Invoke(specAttack);
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

