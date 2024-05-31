using Scripts.Infrastructure.Services;

namespace Scripts.Infrastructure.States
{
    public interface IGameStateMachine : IService
    {
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
        void Enter<TState, TPayload,TPayLoadInt>(TPayload payload,TPayLoadInt payLoadInt) where TState : class, IPayloadedStateInt<TPayload,TPayLoadInt>;
        void Enter<TState>() where TState : class, IState;
    }
}