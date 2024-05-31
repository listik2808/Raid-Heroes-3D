public enum AIStateId
{
    ChasePlayer,
    Death,
    Idle,
    Attack,
    Victory,
    Stunned
}

public interface IState 
{
    AIStateId GetId();
    void Enter(AIAgentBase agent);
    void Update(AIAgentBase agent);
    void Exit(AIAgentBase agent);
}
