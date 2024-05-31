using UnityEngine;

namespace Assets.Scripts.BattleLogic.StateMachine.States
{
    public class AIIdleState : IState
    {
        public void Enter(AIAgentBase agent)
        {
            agent.Animation.SetMovementSpeed(0);
            agent.NavMeshAgent.enabled = false;
        }

        public void Exit(AIAgentBase agent)
        {
        }

        public AIStateId GetId()
        {
            return AIStateId.Idle;
        }

        public void Update(AIAgentBase agent)
        {
        }
    }
}
