using System.Collections.Generic;
using Ws.FSM;

namespace Ws.Battle
{
    public class Entity
    {
        public Controller _controller;
        protected StateMachine _stateMachine;

        public virtual void Init()
        {
            _stateMachine = new StateMachine();
            _stateMachine.AddState(State.Idle,new IdleState(this));
            _stateMachine.AddState(State.Die,new DieState(this));
            _stateMachine.AddState(State.Wound,new DieState(this));
            _stateMachine.AddState(State.Attack,new AttackState(this));
            _stateMachine.AddStateTransform(State.Die,new List<State>() { State.None });
            _stateMachine.AddStateTransform(State.Idle,new List<State>() { State.Wound ,State.Attack,State.Die});
            _stateMachine.AddStateTransform(State.Attack,new List<State>() { State.Idle,State.Idle,State.Die});
            _stateMachine.AddStateTransform(State.Wound,new List<State>() { State.Idle,State.Die });
        }
    }
}