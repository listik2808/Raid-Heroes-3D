using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Enemy;
using Scripts.StaticData;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.BattleLogic.StateMachine.Agents
{
    public class DoubleHitAgent : AIAgentBase
    {
        [SerializeField] private AttackType _attackType;
        [SerializeField] private Animator _weapon;
        [SerializeField] private AudioClip _audioClipSpecSword;
        [SerializeField] private AudioClip _audioClipSword;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _specAudio;
        private AttackState _attackState;
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
            _attackState = state;
            //Animation.DefaulteAttack +=OnAttack;
            StartAttack(false);
        }

        public override void SpecAttack(AttackState state)
        {
            _attackState = state;
            //Animation.SpecAttacEvent += OnAttack;
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
                //Animation.SetAudioClip(_audioClipSpecSword);
                Animation.AttackSpec();
                ResetTimeSpecAttack();
                if (_specAudio.enabled)
                {
                    _specAudio.clip = _audioClipSpecSword;
                    _specAudio.Play();
                }
                
                //if (_weapon != null)
                //{
                //    _weapon.SetTrigger("SpecAttack");
                //}
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
            //    Debug.Log(onAttackDelay);
            //}

            //how quickly will the damage be done
            //StartCoroutine(CallWithDelay(onAttackDelay, specAttack, OnAttack));
            //StartCoroutine(CallWithDelay(onAttackEndedDelay, specAttack, OnAttackEnded));
        }

        public void OnAttack(bool specAttack)
        {
            var health = Target?.gameObject?.GetComponent<EnemyHeaith>();
            if (specAttack && Target != null)
            {
                string textDamage = AbbreviationsNumbers.ShortNumber((SpecDamage * 2) * DamageMultiplayer);
                //Debug.Log(textDamage);
                DamageNumbersPro.DamageNumber damageSpec = damageNumberProSpec.Spawn(Target.transform.position, Mathf.Ceil((SpecDamage * 2) * DamageMultiplayer));
                health?.Damage(Mathf.Ceil((SpecDamage *2)*DamageMultiplayer));
                //Animation.SpecAttacEvent -= OnAttack;
            }
            else if(specAttack == false && Target != null)
            {
                //string textDamage = AbbreviationsNumbers.ShortNumber(SpecDamage * DamageMultiplayer);
                //Debug.Log(textDamage);
                DamageNumbersPro.DamageNumber damageNumber = DamageNumberProText.Spawn(Target.transform.position, Mathf.Ceil(SpecDamage * DamageMultiplayer));
                health?.Damage(Mathf.Ceil(Damage * DamageMultiplayer));
                //Animation.DefaulteAttack -= OnAttack;
            }
            //health?.Damage(specAttack ? SpecDamage : Damage * DamageMultiplayer);
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

            _attackState.IsPerformance = false;

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

