using Scripts.Economics.Buildings;
using Scripts.StaticData;
using System;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BattleLogic.StateMachine.States
{
    public class AttackState : IState
    {
        public float AttackCooldownTimer = 0f;
        public bool AttackEnded = false;
        public float SpecAttackCooldownTimer = 0f;
        public bool SpecAttackEnded = false;
        public bool IsPerformance = false;

        public void Enter(AIAgentBase agent)
        {
            agent.Animation.SetStateAttack(agent, this);
            agent.Animation.SetMovementSpeed(0);
            AttackCooldownTimer = agent.AttackCooldown;
            SpecAttackCooldownTimer = agent.SpecAttackCooldown;
        }

        public void Exit(AIAgentBase agent)
        {

        }

        public AIStateId GetId()
        {
            return AIStateId.Attack;
        }

        public void Update(AIAgentBase agent)
        {
            CheckDistance(agent);
            if (IsPerformance == false)
            {
                SpecAttack(agent);
                Attack(agent);
            }
        }

        private void Attack(AIAgentBase agent)
        {
            if (CanAttack(agent))
            {
                IsPerformance = true;
                AttackEnded = false;
                agent.Attack(this);
            }
        }

        private void SpecAttack(AIAgentBase agent)
        {
            if (CanSpecAttack(agent))
            {
                IsPerformance = true;
                SpecAttackEnded = false;
                agent.SpecAttack(this);
            }
        }

        private bool CanAttack(AIAgentBase agent)
        {
            if(IsPerformance == false)
            {
                AttackEnded = agent.IsMeleedamage;
                return AttackEnded;
            }
            return false;
        }

        private bool CanSpecAttack(AIAgentBase agent)
        {
            if(IsPerformance == false)
            {
                SpecAttackEnded = agent.ISSpecAttack;
                return SpecAttackEnded;
            }
            return false;
        }

        private void CheckDistance(AIAgentBase agent)
        {
            agent.FindNewOpponent();
            //if ((agent.transform.position - agent.Target.transform.position).sqrMagnitude > agent.Config.StoppingDistance * agent.Config.StoppingDistance)
            //{
            //    agent.FindNewOpponent();
            //}
        }
    }
}
