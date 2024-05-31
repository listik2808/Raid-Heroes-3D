using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Army.TypesSoldiers;
using Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BattleLogic.StateMachine.Agents
{
    //shoot light fireball
    public class ShootAgent : AIAgentBase
    {
        [SerializeField] private AudioClip _audioClipSpecSword;
        [SerializeField] private AudioClip _audioClipSword;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _audioSourceSpec;
        [SerializeField] private AttackType _attackType;
        [SerializeField] private Animator _weapon;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _bulletSpawn;
        private bool _killOpponent;
        private int _mask;
        AttackState _attackState;
        public override void Start()
        {
            if (Soldier.TypeSoldier == HeroType.Hero)
            {
                _mask = LayerMask.NameToLayer("Enemy");
            }
            else
            {
                _mask = LayerMask.NameToLayer("HeroBox");
            }

            base.Start();
            NavMeshAgent.stoppingDistance = StoppingDistance / 100; ;//(StoppingDistance == 0 ? Config.StoppingDistance : StoppingDistance);
            StateMachine = new HeroStateMachine(this);
            RegisterStates();
            StateMachine.ChangeState(InitialState);
        }

        public override void Attack(AttackState state)
        {
            _attackState = state;
            if (TargerIsNearMeleeDamage(transform, Target) == true)
            {
                Animation.DefaulteAttack += OnAttack;
                StartAttack(false);
            }
            else
            {
                state.IsPerformance = false;
            }
        }

        //bow shoot
        public override void SpecAttack(AttackState state)
        {
            _attackState = state;
            if (TargerIsNear(transform, Target,StoppingDistance) == true)
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
        //    if (specAttac == false)
        //    {
        //        OnAttack(specAttac);
        //    }
        //    else
        //    {
        //        Shoot(specAttac);
        //    }
            
        //}

        private void StartAttack(bool specAttack)
        {
            if (specAttack)
            {
                //Animation.AttackSpec();
                //ResetTimeSpecAttack();
                if (_weapon != null)
                {
                    _weapon.SetTrigger("SpecAttack");
                }

                if (_bullet != null)
                {
                    Animation.SpecAttacEvent += Shoot;
                    Animation.AttackSpec();
                    ResetTimeSpecAttack();
                    if (_audioSourceSpec.enabled)
                    {
                        _audioSourceSpec.clip = _audioClipSpecSword;
                        _audioSourceSpec.Play();
                    }
                    
                    //Shoot();
                    //StartCoroutine(CallWithDelay(0.2f, Shoot));
                }
                else
                {
                    //Animation.AttackSpec();
                    //ResetTimeSpecAttack();
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

            //float onAttackDelay = 0.2f;
            //float onAttackEndedDelay = 0.4f;

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
            //    Debug.Log(onAttackDelay);
            //}

            //how quickly will the damage be done
            //StartCoroutine(CallWithDelay(onAttackDelay, specAttack, OnAttack));
            //StartCoroutine(CallWithDelay(onAttackEndedDelay, specAttack, OnAttackEnded));
        }

        private void Shoot(bool value)
        {
            //добавить проверку если target == null
            Animation.SpecAttacEvent -= Shoot;
            var position = _bulletSpawn ? _bulletSpawn.position : transform.position;
            GameObject newBullet = Instantiate(_bullet, position, transform.rotation);

            if(newBullet.TryGetComponent(out Bullet bullet))
            {
                bullet.RigidbodyBullet.useGravity = false;
                if(Target != null)
                    bullet.SetShoot(this, _mask, Target.gameObject);
            }

            if(Target != null)
                bullet.RigidbodyBullet.velocity = (Target.position - position + new Vector3(0f, UnityEngine.Random.Range(0.3f, 1f), 0f)).normalized * Config.BulletSpeed;
        }

        public void OnAttack(bool specAttack)
        {
            var health = Target?.gameObject?.GetComponent<EnemyHeaith>();
            if (specAttack && Target != null)
            {
                DamageNumbersPro.DamageNumber damageSpec = damageNumberProSpec.Spawn(Target.transform.position, Mathf.Ceil(SpecDamage));
            }
            else if (specAttack == false && Target != null)
            {
                DamageNumbersPro.DamageNumber DamageNumber = DamageNumberProText.Spawn(Target.transform.position, Mathf.Ceil(Damage * DamageMultiplayer));
                Animation.DefaulteAttack -= OnAttack;
            }
            health?.Damage(specAttack ? Mathf.Ceil(SpecDamage) : Mathf.Ceil(Damage * DamageMultiplayer));
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

            //Animation.SpecAttacEvent -= OnAttack;
            if(Health.Current > 0)
            {
                OnAttackEnded(specAttack);
            }
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

        private bool TargerIsNear(Transform hero, Transform target,float stoping)
        {
            float dist =0;
            if (target.TryGetComponent(out AIAgentBase aIAgent))
            {
                dist = Vector3.Distance(hero.position, target.position);
                dist *= 100;
                if (dist < stoping && dist / 100 > aIAgent.StoppingDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
            //result = dist < stoping + 1.5f;
            //var dist = Vector3.Distance(hero.position, target.position);
            //var result = dist < StoppingDistance ;
            //Debug.LogWarning("dist " + dist + " " + (result ? "attack" : "specattack"));
            //return result;
        }

        private bool TargerIsNearMeleeDamage(Transform hero, Transform target)
        {
            float distMelleDamage = 0;
            if (target.TryGetComponent(out AIAgentBase aIAgent))
            {
                distMelleDamage = Vector3.Distance(hero.position, target.position);
                if (distMelleDamage <= aIAgent.StoppingDistance)//Soldier.DataSoldier.DistanceFigtMelle)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
           
            //result = dist < stoping + 1.5f;
            //var dist = Vector3.Distance(hero.position, target.position);
            //var result = dist < StoppingDistance ;
            //Debug.LogWarning("dist " + dist + " " + (result ? "attack" : "specattack"));
            //return result;
        }

        public IEnumerator CallWithDelay(float delay, bool specAttack, Action<bool> action)
        {
            if (IsDead == true) yield return null;

            yield return new WaitForSeconds(delay);

            action?.Invoke(specAttack);
        }

        public IEnumerator CallWithDelay(float delay, Action action)
        {
            if (IsDead == true) yield return null;

            yield return new WaitForSeconds(delay);

            action?.Invoke();
        }
    }
}