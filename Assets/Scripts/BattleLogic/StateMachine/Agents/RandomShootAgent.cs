using Assets.Scripts.BattleLogic.StateMachine.States;
using Scripts.Army.TypesSoldiers;
using Scripts.Enemy;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.BattleLogic.StateMachine.Agents
{
    //shoot light fireball
    public class RandomShootAgent : AIAgentBase
    {
        [SerializeField] private AudioClip _audioClipSpecSword;
        [SerializeField] private AudioClip _audioClipSword;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _audioSourceSpec;
        [SerializeField] private AttackType _attackType;
        [SerializeField] private Animator _weapon;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _bulletSpawn;
        [SerializeField] private float _freezeTime;
        private bool _killOpponent;
        private int mask;

        public override void Start()
        {
            base.Start();
            mask = LayerMask.NameToLayer("Enemy");
            NavMeshAgent.stoppingDistance = (StoppingDistance == 0? Config.StoppingDistance : StoppingDistance);
            Debug.Log(NavMeshAgent.stoppingDistance + " stopping distance");
            StateMachine = new HeroStateMachine(this);
            RegisterStates();
            StateMachine.ChangeState(InitialState);
        }

        public override void Attack(AttackState state)
        {
            Animation.DefaulteAttack += OnAttack;
            StartAttack(false);
            //Debug.Log("dist attck");
            //if (TargerIsNear(transform, Target) == true)
            //{
            //    Animation.DefaulteAttack += OnAttack;
            //    Debug.Log("Archer Attack called");
            //    //state.AttackEnded = false;
            //    StartAttack(false);
            //}
            //else
            //{
            //    state.IsPerformance = false;
            //    //state.AttackEnded = true;
            //    //state.AttackCooldownTimer = AttackCooldown;
            //}
        }
        
        //bow shoot
        public override void SpecAttack(AttackState state)
        {
            StartAttack(true);
            // Debug.Log("dist specattck");
            //if ((gameObject.transform.position - Target.transform.position).sqrMagnitude < StoppingDistance)
            //    StartAttack(true);
            //else
            //    state.IsPerformance = false;
            //if (TargerIsNear(transform, Target) == false)
            //{
            //    //Animation.SpecAttacEvent += OnAttack;
            //    Debug.Log("Archer Spec Attack called");
            //    //state.SpecAttackEnded = false;
            //    StartAttack(true);
            //}
            //else
            //{
            //    state.IsPerformance = false;
            //    //state.SpecAttackEnded = true;
            //    //state.SpecAttackCooldownTimer = SpecAttackCooldown;
            //}
        }

        //public override void EventAttack(bool specAttac)
        //{
        //    OnAttack(specAttac);
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

                var enemies = Physics.OverlapSphere(gameObject.transform.position, Soldier.DataSoldier.DistanceFigtMelle, 1 << mask);
                if (enemies != null && enemies.Length != 0)
                {
                    int index = UnityEngine.Random.Range(0, enemies.Length);
                    if (enemies[index].TryGetComponent(out AIAgentBase agentEnemy))
                    {
                        Enemy = agentEnemy;
                        Target = Enemy.transform;
                    }
                }

                if (_bullet != null)
                {
                    //Animation.SpecAttacEvent += OnAttack;
                    Animation.AttackSpec();
                    ResetTimeSpecAttack();
                    var newBullet = Instantiate(_bullet, Target.transform.position, transform.rotation);
                    if (_audioSourceSpec.enabled)
                    {
                        _audioSourceSpec.clip = _audioClipSpecSword;
                        _audioSourceSpec.Play();
                    }
                    
                    newBullet.transform.SetParent(Target.transform);
                    if (newBullet.TryGetComponent(out Freeze freeze))
                    {
                        freeze.FreezeTime = _freezeTime;
                    }
                    if (Type == HeroType.Hero)
                    {
                        freeze.SetLayerTarget(LayerMask.NameToLayer("Enemy"),this,Enemy);
                    }
                    else if(Type == HeroType.Enemy)
                    {
                        freeze.SetLayerTarget(LayerMask.NameToLayer("HeroBox"), this, Enemy);
                    }

                    //OnAttackEnded(specAttack);
                }
                //else
                //{
                //    OnAttack(specAttack);
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

        public void OnAttack(bool specAttack)
        {
            var health = Target?.gameObject?.GetComponent<EnemyHeaith>();
            if(Target != null && specAttack ==false)
            {
                DamageNumbersPro.DamageNumber DamageNumber = DamageNumberProText.Spawn(Target.transform.position, Mathf.Ceil(Damage * DamageMultiplayer));
            }
            else if ( Target != null && specAttack)
            {
                DamageNumbersPro.DamageNumber damageNumber = damageNumberProSpec.Spawn(Target.transform.position, Mathf.Ceil(SpecDamage * DamageMultiplayer));
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
            Animation.DefaulteAttack -= OnAttack;
           // Animation.SpecAttacEvent -= OnAttack;
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

        private bool TargerIsNear(Transform hero, Transform target)
        {
            var dist = Vector3.Distance(hero.position, target.position);
            var result = dist < StoppingDistance - 0.2f;
            Debug.Log("dist " + dist + " " + (result? "attack" : "specattack"));
            return result;
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