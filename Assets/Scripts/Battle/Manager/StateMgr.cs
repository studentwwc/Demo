using System.Collections.Generic;
using Battle.Entity;
using Battle.FSM;
using DarkGod.System;
using UnityEngine;

namespace battle.mgr
{
    public class StateMgr:MonoBehaviour
    {
        private Dictionary<State, IState> stateDic = new Dictionary<State, IState>();
        public void Init()
        {
            stateDic.Add(State.Idle,new IdleState());
            stateDic.Add(State.Move,new MoveState());
            stateDic.Add(State.Attack,new AttackState());
            stateDic.Add(State.Die,new DieState());
            stateDic.Add(State.Born,new BornState());
            stateDic.Add(State.Hit,new HitState());
        }

        public void ChangeState(EntityBase entityBase,State state,params object[]param)
        {
            if (BattleSystem.Instance.gameState == GameState.End)
            {
                return;
            }

            if (entityBase.currentState == state)
            {
                return;
            }
            if (stateDic.ContainsKey(state))
            {
                if (entityBase.currentState != State.None)
                {
                    stateDic[entityBase.currentState].Exit(entityBase,param);
                }

                stateDic[state].Enter(entityBase,param);
                stateDic[state].Process(entityBase,param);
            }
        }
    }
}