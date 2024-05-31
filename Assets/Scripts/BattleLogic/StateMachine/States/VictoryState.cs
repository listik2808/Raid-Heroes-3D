
using System;
using UnityEngine;

namespace Assets.Scripts.BattleLogic.StateMachine.States
{
    public class VictoryState : IState
    {
        public void Enter(AIAgentBase agent)
        {
            if (agent.IsDead) return;
            if(agent.UnderHypno)
            {
                agent.StateMachine.ChangeState(AIStateId.Idle);
            }
            agent.NavMeshAgent.enabled = false;
            agent.Animation.Victory();
            agent.SliderAttac.gameObject.SetActive(false);// выключаем слайдера атак
            agent.StopEfects();
        }

        public void Exit(AIAgentBase agent)
        {
        }

        public AIStateId GetId()
        {
            return AIStateId.Victory;
        }

        public void Update(AIAgentBase agent)
        {
        }
    }
}
