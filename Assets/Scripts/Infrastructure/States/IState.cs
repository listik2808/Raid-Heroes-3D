namespace Scripts.Infrastructure.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }

    public interface IPayloadedStateInt<TPayload,TPayLoadInt> : IExitableState
    {
        void Enter(TPayload payload,TPayLoadInt payLoadInt);
    }

    public interface IExitableState
    {
        void Exit();
    }
}