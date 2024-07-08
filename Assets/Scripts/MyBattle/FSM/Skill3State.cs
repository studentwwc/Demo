using Ws.Battle;

namespace Ws.FSM
{
    public class Skill3State:IState
    {
        public Skill3State(Entity entity) : base(entity)
        {
            
        }
        public override void Enter()
        {
            _controller.Skill3Animator();
        }

        public override void Proccess()
        {
            
        }

        public override void Exit()
        {
            
        }

        
    }
}