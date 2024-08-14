using Ws.Battle;

namespace Ws.FSM
{
    public class IdleState:IState
    {
        public IdleState(Entity entity) : base(entity)
        {
        }
        public override void Enter()
        {
            entity._controller.IdleAnimator();
            entity._controller.Idle();
        }

        public override void Proccess()
        {
            
        }

        public override void Exit()
        {
            
        }

        
    }
}