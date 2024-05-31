using Scripts.StaticData;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.BattleLogic.StateMachine.States
{
    public class AIChasePlayerState : IState
    {
        private float _timer = 0;
        private float _startSpeed;
        float _distMelleDamage = 0;
       
        public void Enter(AIAgentBase agent)
        {
            //agent.GetComponent<BoxCollider>().enabled = false;
            agent.GetComponent<ShowPopup>().enabled = false;
            //agent.FindNewOpponent();
            if (agent.Target == null) return;
            agent.NavMeshAgent.enabled = true;
            agent.NavMeshAgent.destination = agent.Target.position;
            _startSpeed = agent.Config.MaxSpeed;
            agent.ActivateSliderAttack();
            //agent.NavMeshAgent.speed = _startSpeed;
        }

        public void Exit(AIAgentBase agent)
        {
        }

        public AIStateId GetId()
        {
            return AIStateId.ChasePlayer;
        }

        public void Update(AIAgentBase agent)
        {
            if (agent.Target == null)
            {
                agent.StateMachine.ChangeState(AIStateId.Victory);
                return;
            }
            if(agent.IsStun == false)
            {
                agent.FindNewOpponent();
                Move(agent);
                Rotate(agent);
                SetMovementAnimation(agent);
                TransitionToAttack(agent);
            }
        }

        private void Move(AIAgentBase agent)
        {
            if(agent.IsStopMove == false)
            {
                _timer -= Time.unscaledDeltaTime;
                if (_timer < 0)
                {
                    agent.NavMeshAgent.destination = agent.Target.position;
                    _timer = agent.Config.RepathFrequency;
                }
            }
        }

        private void SetMovementAnimation(AIAgentBase agent)
        {
            var speed = agent.NavMeshAgent.velocity.sqrMagnitude / (_startSpeed * _startSpeed); // from 0 to 1
            agent.Animation.SetMovementSpeed(speed);
        }

        private void Rotate(AIAgentBase agent)
        {
           //agent.transform.rotation = Quaternion.LookRotation(agent.Target.position - agent.transform.position);
            var direction = (agent.Target.position - agent.transform.position).normalized;
            direction.y= 0;
            agent.transform.rotation = Quaternion.LookRotation(direction);
        }

        private void TransitionToAttack(AIAgentBase agent)
        {
            if (agent.Soldier.SpecialAttack == SpecialAttack.Heal || agent.Soldier.SpecialAttack == SpecialAttack.Hypnosis)
            {
                HealTransitionToAttack(agent);
            }
            else if(agent.Soldier.SpecialAttack == SpecialAttack.Shoot ||agent.Soldier.SpecialAttack == SpecialAttack.Fireball||agent.Soldier.SpecialAttack == SpecialAttack.Lightning)
            {
                _distMelleDamage = Vector3.Distance(agent.transform.position, agent.Target.position);
                _distMelleDamage *= 100;
                if (_distMelleDamage < agent.StoppingDistance)
                {
                    agent.DiativateMove();
                    agent.StateMachine.ChangeState(AIStateId.Attack);
                }
            }
            else
            {
                if ((agent.Target.position - agent.transform.position).sqrMagnitude < agent.StoppingDistance * agent.StoppingDistance)
                    agent.StateMachine.ChangeState(AIStateId.Attack);
            }
        }

        private void HealTransitionToAttack(AIAgentBase agent)
        {
            if (agent.NavMeshAgent.speed <= 0 && agent.ISSpecAttack == true)
            {
                agent.StateMachine.ChangeState(AIStateId.Attack);
            }
            else if(agent.NavMeshAgent.speed <= 0 && agent.IsMeleedamage == true)
            {
                if ((agent.Target.position - agent.transform.position).sqrMagnitude < agent.StoppingDistance * agent.StoppingDistance)
                {
                    agent.StateMachine.ChangeState(AIStateId.Attack);
                }
            }
            else
            {
                if (agent.NavMeshAgent.speed > 0 && agent.ISSpecAttack == true)
                {
                    agent.StateMachine.ChangeState(AIStateId.Attack);
                }
                else if (agent.NavMeshAgent.speed > 0 && agent.ISSpecAttack == false)
                {
                    if ((agent.Target.position - agent.transform.position).sqrMagnitude < agent.StoppingDistance * agent.StoppingDistance)
                    {
                        agent.StateMachine.ChangeState(AIStateId.Attack);
                    }
                }
            }
        }
    }
}
