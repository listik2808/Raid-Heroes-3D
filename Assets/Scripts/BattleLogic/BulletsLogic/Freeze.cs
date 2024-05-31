using Assets.Scripts.BattleLogic.StateMachine.Agents;
using System.Collections;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    [HideInInspector] public float FreezeTime;
    [HideInInspector] public float FreezeTimeLeft;// сколько времени заморозки осталось
    private int _layer;
    private float _speed;
    private RandomShootAgent _randomShootAgent;
    private AIAgentBase _agent;
    //private void Start()
    //{
    //    _layer = LayerMask.NameToLayer("Enemy");
    //}

    public void SetLayerTarget(LayerMask layer ,RandomShootAgent randomShootAgent,AIAgentBase aIAgentBase)
    {
        _layer = layer;
        _randomShootAgent = randomShootAgent;
        _agent = aIAgentBase;
        StartFreeze();
    }

    public void StopFreeze()
    {
        StopCoroutine("Unfreeze");
        Destroy(gameObject);
    }

    private void StartFreeze()
    {
        bool replaceFreeze = true;
        if (FreezeTime > _agent.Freeze?.FreezeTimeLeft)
        {
            if (_agent.Freeze != null)
            {
                _agent.Animation.SetAnimatorSpeed(_speed);
                _agent.BuffAndDebuff.Freezing.gameObject.SetActive(false);
                _agent.Freeze.StopFreeze();
            }
        }
        else if (_agent.Freeze != null)
        {
            replaceFreeze = false;
        }

        if (!replaceFreeze)
            return;

        if (replaceFreeze)
        {
            _agent.Freeze = this;
            _agent.BuffAndDebuff.Freezing.gameObject.SetActive(true);
            _randomShootAgent.OnAttack(true);
            _agent?.StateMachine?.ChangeState(AIStateId.Idle);
            _speed = _agent.Animation.Animator.speed;
            _agent?.Animation.SetAnimatorSpeed(0);
            FreezeTimeLeft = FreezeTime;
            StartCoroutine("Unfreeze", _agent);
        }
    }

    private IEnumerator Unfreeze(AIAgentBase agent)
    {
        Debug.Log("Unfreeze called");
        agent.Freeze.FreezeTimeLeft -= Time.deltaTime;
        yield return new WaitForSeconds(FreezeTime);

        if (agent.IsDead)
            yield break;

        agent.StateMachine.ChangeState(AIStateId.ChasePlayer);
        agent.BuffAndDebuff.Freezing.gameObject.SetActive(false);
        agent.Animation.SetAnimatorSpeed(_speed);
        Debug.Log("Unfreeze");
        Destroy(gameObject);
    }
}
