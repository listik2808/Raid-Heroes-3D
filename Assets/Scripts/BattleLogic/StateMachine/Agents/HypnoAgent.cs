using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Army.TypesSoldiers;
using Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BattleLogic.StateMachine.Agents
{
    public class HypnoAgent : AIAgentBase
    {
        [SerializeField] private AudioClip _audioClipHipno;
        [SerializeField] private AudioClip _audioClipSword;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _specAudio;
        [SerializeField] private AttackType _attackType;
        [SerializeField] private Animator _weapon;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _bulletSpawn;
        private bool _killOpponent;

        public override void Start()
        {
            base.Start();
            NavMeshAgent.stoppingDistance = (StoppingDistance == 0 ? Config.StoppingDistance : StoppingDistance);
            StateMachine = new HeroStateMachine(this);
            RegisterStates();
            StateMachine.ChangeState(InitialState);
        }

        public override void Attack(AttackState state)
        {
            //Animation.DefaulteAttack += OnAttack;
            //Debug.Log("Archer Attack called");
            //state.AttackEnded = false;
            if ((Target.position - gameObject.transform.position).sqrMagnitude < StoppingDistance * StoppingDistance)
                StartAttack(false);
            else
                state.IsPerformance = false;
        }

        //bow shoot
        public override void SpecAttack(AttackState state)
        {
           // Animation.SpecAttacEvent += OnAttackEnded;
            //Debug.Log("Archer Spec Attack called");
            //state.SpecAttackEnded = false;
            StartAttack(true);
        }

        public override void EventAttack(bool specAttac)
        {
            if (specAttac)
            {
                OnAttackEnded(specAttac);

            }
            else
            {
                OnAttack(specAttac);
            }
        }

        private void StartAttack(bool specAttack)
        {
            if (specAttack)
            {
                //Animation.SetAudioClip(_audioClipHipno);
                Animation.AttackSpec();
                ResetTimeSpecAttack();
                if (_weapon != null)
                {
                    _weapon.SetTrigger("SpecAttack");
                }
                GameObject hypnoBullet = Instantiate(_bullet);
                if (_specAudio.enabled)
                {
                    _specAudio.clip = _audioClipHipno;
                    _specAudio.Play();
                }
                
                if (hypnoBullet.TryGetComponent(out Hypno hypno))
                {
                    HypnoOpponent(SpecDamage, hypno);
                }
                //return;
                //OnAttackEnded(specAttack);
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
            //}

            //how quickly will the damage be done
            //StartCoroutine(CallWithDelay(onAttackDelay, specAttack, OnAttack));
            //StartCoroutine(CallWithDelay(onAttackEndedDelay, specAttack, OnAttackEnded));
        }

        private void HypnoOpponent(float time, Hypno hypno)
        {

            if (Target == null) return;

            //find strongest friend
            List<Soldier> collection = Type == HeroType.Enemy ?
                HeroEnemyList.Heroes : HeroEnemyList.Enemies;

            Soldier strongestSoldier = TryFindStrongestSoldier(collection);
            if (strongestSoldier == null)
            {
                return;
            }

            if (strongestSoldier.TryGetComponent(out AIAgentBase agent) == false)
            {
                return;
            }

            agent.Hypno = hypno;
            agent.Hypno?.StartHypno(agent, time);
        }

        private void Shoot()
        {
            var position = _bulletSpawn ? _bulletSpawn.position : transform.position;
            var newBullet = Instantiate(_bullet, position, transform.rotation);
            Rigidbody bulletRB;
            if (newBullet.TryGetComponent(out bulletRB) == false)
                bulletRB = newBullet.AddComponent<Rigidbody>();
            bulletRB.useGravity = false;

            bulletRB.velocity = (Target.position - position + new Vector3(0f, UnityEngine.Random.Range(0.3f, 1f), 0f)).normalized * Config.BulletSpeed;
        }

        public void OnAttack(bool specAttack)
        {
            if (specAttack) return;

            var health = Target?.gameObject?.GetComponent<EnemyHeaith>();
            if (specAttack == false && Target != null)
            {
                DamageNumbersPro.DamageNumber DamageNumber = DamageNumberProText.Spawn(Target.transform.position, Mathf.Ceil(Damage * DamageMultiplayer));
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
            //Animation.DefaulteAttack -= OnAttack;
            OnAttackEnded(specAttack);
        }

        public void OnAttackEnded(bool specAttack)
        {
           // Animation.SpecAttacEvent -= OnAttackEnded;
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

        private Soldier TryFindStrongestSoldier(List<Soldier> list)
        {
            if (list == null) return null;

            Soldier strongestSoldier = null;
            float maxPower = -1;
            foreach (var enemy in list)
            {
                var power = enemy.Power.GetPower(1,true);
                if (power > maxPower)
                {
                    maxPower = power;
                    strongestSoldier = enemy;
                }
            }

            return strongestSoldier;
        }
    }
}