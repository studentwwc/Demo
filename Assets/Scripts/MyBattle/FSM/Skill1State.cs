using Ws.Battle;

namespace Ws.FSM
{
    public class Skill1State:IState
    {
        public Skill1State(Entity entity) : base(entity)
        {
            
        }
        public override void Enter()
        {
            _controller.Skill1Animator();
        }

        public override void Proccess()
        {
            
        }

        public override void Exit()
        {
            
        }

        
    }
}