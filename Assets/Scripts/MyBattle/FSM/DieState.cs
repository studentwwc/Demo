using Ws.Battle;

namespace Ws.FSM
{
    public class DieState:IState
    {
        public DieState(Entity entity) : base(entity)
        {
            
        }
        public override void Enter()
        {
            entity._controller.DieAnimator();
            entity._controller.Die();
        }

        public override void Proccess()
        {
            
        }

        public override void Exit()
        {
            
        }

        
    }
}