using Ws.Battle;

namespace Ws.FSM
{
    public class Skill2State:IState
    {
        public Skill2State(Entity entity) : base(entity)
        {
            
        }
        public override void Enter()
        {
            _controller.Skill2Animator();
        }

        public override void Proccess()
        {
            
        }

        public override void Exit()
        {
            
        }

        
    }
}