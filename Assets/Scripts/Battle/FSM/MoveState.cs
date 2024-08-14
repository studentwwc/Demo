using Battle.Entity;
using Common;

namespace Battle.FSM
{
    public class MoveState:IState
    {
        public override void Exit(EntityBase entity,params object[]param)
        {
            //NetCommon.Log(entity.entityName+"Exit Move State");
        }

        public override void Enter(EntityBase entity,params object[]param)
        {
            entity.currentState = State.Move;
            entity.SetBlend(Constant.playerRunBlend);
           
            //NetCommon.Log(entity.entityName+"Enter Move State");
        }

        public override void Process(EntityBase entity,params object[]param)
        {
            //NetCommon.Log(entity.entityName+"Process Move State");
        }
    }
}