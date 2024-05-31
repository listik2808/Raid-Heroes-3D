using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStateMachine
{
    public IState[] States;
    public AIAgentBase Agent;
    public AIStateId CurrentState;

    public HeroStateMachine(AIAgentBase agent)
    {
        Agent = agent;
        int numStates = System.Enum.GetNames(typeof(AIStateId)).Length;
        States = new IState[numStates];
    }

    public void RegisterState(IState state)
    {
        int index = (int)state.GetId();
        States[index] = state;
    }

    public IState GetState(AIStateId stateId)
    {
        int index = (int)stateId;
        return States[index];
    }

    public void Update()
    {
        GetState(CurrentState)?.Update(Agent);
    }

    public void ChangeState(AIStateId newState) 
    {
        GetState(CurrentState)?.Exit(Agent);
        CurrentState = newState;
        GetState(CurrentState)?.Enter(Agent);
    }
}
