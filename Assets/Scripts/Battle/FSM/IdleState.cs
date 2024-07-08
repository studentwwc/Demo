using Battle.Entity;
using Common;
using UnityEngine;

namespace Battle.FSM
{
    public class IdleState:IState
    {
        public override void Exit(EntityBase entity,params object[]param)
        {
            //NetCommon.Log("Exit Idle State");
        }

        public override void Enter(EntityBase entity,params object[]param)
        {
            entity.currentState = State.Idle;
            entity.SetMoveDir(Vector2.zero);
           // NetCommon.Log("Enter Idle State");
        }

        public override void Process(EntityBase entity,params object[]param)
        {
            //NetCommon.Log("Process Idle State");
            if (entity.nextSkill != 0)
            {
                entity.Attack(entity.nextSkill);
                return;
            }
            entity.SetAction(Constant.defaultAction);
            if (entity.GetCurrentDir() != Vector2.zero)
            {
                entity.Move();
                entity.SetMoveDir(entity.GetCurrentDir());
            }
            else
            {
                entity.SetBlend(Constant.playerIdleBlend);
            }
            
        }
    }
}