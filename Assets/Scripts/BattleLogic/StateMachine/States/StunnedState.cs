using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.BattleLogic.StateMachine.States
{
    public class StunnedState : IState
    {
        //private float _timeSpeed = 1f;
        private bool _isStunned;
        private float _timer;
        private float _stunnedTime = 1f;
        private float _startSpeed;
        private Rigidbody _rb;
        private float _elepsedTime = 0;
        private float _timerStune;
        private float _speedAnimation;
        public void Enter(AIAgentBase agent)
        {
            //foreach (var attacker in agent.Attackers)
            //{
            //    var attckerAgent = attacker.GetComponent<AIAgentBase>();
            //    attckerAgent.FindNewOpponent();
            //    attacker.StateMachine.ChangeState(AIStateId.ChasePlayer);
            //}
            //Временно пока не разберусь что с этим сделать !!!!!!!!!!!!!!!!!!!
            _isStunned = true;
            agent.IsStun = true;
            _startSpeed = agent.NavMeshAgent.speed;
            agent.NavMeshAgent.speed = 0f;
            _rb = agent.Rigidbody;
            _timerStune = agent.Stune;

            _rb.useGravity = true;
            _rb.isKinematic = false;
            _speedAnimation = agent.Animation.Animator.speed;
            agent.NavMeshAgent.enabled = false;
            //if (agent.TryGetComponent(out BoxCollider collider))
            //{
            //    collider.isTrigger = false;
            //}
            if(agent.Enemy != null && agent.IsDead == false)
            {
                _rb.AddForce(agent.Enemy.Soldier.DataSoldier.DistanceFigtMelle * agent.Enemy.transform.forward, ForceMode.Impulse);
            }
        }

        public void Exit(AIAgentBase agent)
        {
            ReturnActivity(agent);
        }

        public AIStateId GetId()
        {
            return AIStateId.Stunned;
        }

        public void Update(AIAgentBase agent)
        {
            if(agent.Enemy != null)
            {
                if (_isStunned == false) return;
                if (_isStunned)
                {
                    agent.BuffAndDebuff.Stane.gameObject.SetActive(true);
                    agent.Animation.Dizzy();
                }
                _timer += Time.deltaTime;
                _elepsedTime += Time.deltaTime;
                if (_timer > _stunnedTime)
                {
                    ReturnActivity(agent);

                }
                if (_elepsedTime >= _timerStune)
                {
                    agent.Animation.Animator.speed = _speedAnimation;
                    ReturnActivity(agent);
                    agent.StateMachine.ChangeState(AIStateId.ChasePlayer);
                    _isStunned = false;
                    agent.IsStun = false;
                    _elepsedTime = 0;
                }
            }
        }

        private void ReturnActivity(AIAgentBase agent)
        {
            //if (agent.TryGetComponent(out BoxCollider collider))
            //{
            //    collider.isTrigger = true;
            //}
            agent.Animation.New();
            agent.NavMeshAgent.enabled = true;
            agent.NavMeshAgent.speed = _startSpeed;
            agent.BuffAndDebuff.Stane.gameObject.SetActive(false);
            _rb.useGravity = false;
            _rb.isKinematic = true;
            agent.BuffAndDebuff.Stane.gameObject.SetActive(false);
            //agent.StateMachine.ChangeState(AIStateId.ChasePlayer);
            //_isStunned = false;
            _timer = 0;
        }
    }
}
