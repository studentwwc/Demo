using System.Collections.Generic;
using Battle.Entity;
using Ws.Battle;

namespace Ws.FSM
{
    public class StateMachine
    {
        public State CurrentState;
        private Dictionary<State, IState> States=new Dictionary<State, IState>();
        //条件转换     哪些状态可以转换哪些状态
        private Dictionary<State, List<State>> ConditionTransform=new Dictionary<State, List<State>>();
        public void AddState(State state,IState IState)
        {
            States.Add(state,IState);
        }

        public void AddStateTransform(State state,List<State>canTrans)
        {
            if (ConditionTransform.ContainsKey(state))
            {
                ConditionTransform[state].AddRange(canTrans);
            }
            else
            {
                ConditionTransform.Add(state,canTrans);
            }
        }

        public void ChangeState(State state)
        {
            if (CurrentState == State.None && States.ContainsKey(state))
            {
                CurrentState = state;
                States[CurrentState].Enter();
                GameRoot.Instance.updates += States[CurrentState].Proccess;
                return;
            }
            if (!CanTransform(state))
            {
                return;
            }

            if (States.ContainsKey(state)&&CurrentState!=State.None)
            {
                GameRoot.Instance.updates -= States[CurrentState].Proccess;
                States[CurrentState].Exit();
                CurrentState = state;
                States[CurrentState].Enter();
                GameRoot.Instance.updates += States[CurrentState].Proccess;
            }
        }

        public bool CanTransform(State targetState)
        {
            if (ConditionTransform.ContainsKey(CurrentState))
            {
                return ConditionTransform[CurrentState].Contains(targetState);
            }

            return false;
        }
    }
}