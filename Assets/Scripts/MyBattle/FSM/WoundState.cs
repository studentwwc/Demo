using Ws.Battle;

namespace Ws.FSM
{
    public class WoundState:IState
    {
        public WoundState(Entity entity) : base(entity)
        {
            
        }
        public override void Enter()
        {
            _controller.WoundAnimator();
            _controller.Wound();
        }

        public override void Proccess()
        {
            
        }

        public override void Exit()
        {
            
        }

        
    }
}