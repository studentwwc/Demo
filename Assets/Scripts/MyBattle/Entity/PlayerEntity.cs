using System.Collections.Generic;
using Ws.FSM;

namespace Ws.Battle
{
    public class PlayerEntity:Entity
    {
        public override void Init()
        {
            base.Init();
            _stateMachine.AddState(State.Skill1,new Skill1State(this));
            _stateMachine.AddState(State.Skill2,new Skill1State(this));
            _stateMachine.AddState(State.Skill3,new Skill1State(this));
            _stateMachine.AddStateTransform(State.Skill1,new List<State>() { State.Idle,State.Die });
            _stateMachine.AddStateTransform(State.Skill2,new List<State>() { State.Idle,State.Die });
            _stateMachine.AddStateTransform(State.Skill3,new List<State>() { State.Idle,State.Die });
        }
    }
}