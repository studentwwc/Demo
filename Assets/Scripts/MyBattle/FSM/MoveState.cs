using Ws.Battle;

namespace Ws.FSM
{
    public class MoveState:IState
    {
        public MoveState(Entity entity) : base(entity)
        {
        }
        public override void Enter()
        {
            entity._controller.MoveAnimator();
        }

        public override void Proccess()
        {
            entity._controller.Move();
        }

        public override void Exit()
        {
           
        }

        
    }
}