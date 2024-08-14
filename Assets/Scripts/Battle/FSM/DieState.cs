using Battle.Entity;
using Common;
using DarkGod.System;
using Service;
using UnityEngine;

namespace Battle.FSM
{
    public class DieState:IState
    {
        public override void Exit(EntityBase entity, params object[] param)
        {
           
        }

        public override void Enter(EntityBase entity, params object[] param)
        {
            entity.currentState = State.Die;
        }

        public override void Process(EntityBase entity, params object[] param)
        {
            entity.SetAction(Constant.dieAction);
            if (entity.entityType == EntityType.Player)
            {
                BattleSystem.Instance.gameState = GameState.End;
            }

            TimerService.Instance.AddTimerTask((int tid) =>
            {
                entity.SetActive(false);
                if (entity.entityType == EntityType.Player)
                {
                    BattleSystem.Instance.GameOver(false);
                }
                else
                {
                    BattleSystem.Instance.KillMonster();
                    Debug.Log("Die a Monster");
                }
            },1000);
        }
    }
}