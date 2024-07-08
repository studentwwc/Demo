using Battle.Entity;
using Common;
using Service;

namespace Battle.FSM
{
    public class BornState:IState
    {
        public override void Exit(EntityBase entity, params object[] param)
        {
        }

        public override void Enter(EntityBase entity, params object[] param)
        {
            entity.currentState = State.Born;
        }

        public override void Process(EntityBase entity, params object[] param)
        {
            entity.SetAction(Constant.bornAction);
            TimerService.Instance.AddTimerTask((id) =>
            {
                entity.Idle();
            },1000);
        }
    }
}