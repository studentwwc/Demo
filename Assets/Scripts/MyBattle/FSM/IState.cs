using Ws.Battle;

namespace Ws.FSM
{
    public abstract class IState
    {
        protected Entity entity;
        protected Controller _controller;

        public IState(Entity entity)
        {
            this.entity = entity;
            _controller = entity._controller;
        }

        public abstract void Enter();
        public abstract void Proccess();
        public abstract void Exit();
    }
}