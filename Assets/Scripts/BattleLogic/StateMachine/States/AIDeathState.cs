using Scripts.Army.TypesSoldiers;
using UnityEngine;

public class AIDeathState : IState
{
    public void Enter(AIAgentBase agent)
    {
        agent.IsDead = true;

        agent.StopAllCoroutines();

        Soldier soldier = agent.GetComponent<Soldier>();
        if (agent.Type == HeroType.Hero)
            HeroEnemyList.RemoveHero(soldier);
        else
            HeroEnemyList.RemoveEnemy(soldier);


        foreach (AIAgentBase attacker in agent.Attackers)
        {
            if(attacker.IsDead) continue;

            AIAgentBase attackerAgent = attacker.GetComponent<AIAgentBase>();
            attackerAgent.FindNewOpponent();
            attacker.StateMachine.ChangeState(AIStateId.ChasePlayer);
        }

        agent.Attackers.Clear();


        agent.NavMeshAgent.enabled = false;

        agent.Animation.Die();
        agent.GetComponent<HealthView>().SliderHp.gameObject.SetActive(false);
        agent.SliderAttac.gameObject.SetActive(false);
        agent.BuffAndDebuff.gameObject.SetActive(false);
        agent.StateMachine = null;
    }

    public void Exit(AIAgentBase agent)
    {
    }

    public AIStateId GetId()
    {
        return AIStateId.Death;
    }

    public void Update(AIAgentBase agent)
    {
    }
}
