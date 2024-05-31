using Scripts.Army.TypesSoldiers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    [SerializeField] private FollowTo _follow;
    [HideInInspector] public AIAgentBase Agent;
   // private int i = 0;
    private void Awake()
    {
        Agent = GetComponent<AIAgentBase>();
        Agent.FindTarget = this;
    }

    public void FindNearestOpponent()
    {
        Transform result = Find(Agent.Type == HeroType.Hero ? HeroEnemyList.Enemies : HeroEnemyList.Heroes);
        Agent.Target = result;
        if (Agent.Target != null)
        {
            Agent.ActivateMove();
        }
    }

    public bool FindNearestFriend()
    {
        var collection = Agent.Type == HeroType.Hero ? HeroEnemyList.Heroes : HeroEnemyList.Enemies;
        var list = collection.Where(x => x.GetComponent<AIAgentBase>() != Agent).ToList();
        if (list.Count == 0)
        {
            if(collection[0].TryGetComponent(out AIAgentBase agentt))
            {
                agentt.StateMachine.ChangeState(AIStateId.Idle);
            }
            return false;
        }
        Transform result = Find(list);
        if(result.TryGetComponent(out AIAgentBase agent))
        {
            if (agent == Agent)
            {
                agent.StateMachine.ChangeState(AIStateId.Idle);
                return false;
            }
        }

        Agent.Target = result;
        return true;
    }

    private Transform Find(List<Soldier> targetList)
    {
        return TryFindNearestSoldier(transform.position, targetList)?.transform;
    }


    private Soldier TryFindNearestSoldier(Vector3 pos, List<Soldier> list)
    {
        Soldier soldier = null;
        float dist = float.MaxValue;
        foreach(Soldier s in list)
        {
            float d = Vector3.Distance(s.transform.position, pos);
            if (d < dist)
            {
                dist = d;
                soldier = s;
            }
        }

        return soldier;
    }
}
