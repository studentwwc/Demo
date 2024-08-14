using Battle.Entity;

namespace Battle.FSM
{
    public abstract class IState
    {
        public abstract void Exit(EntityBase entity,params object[]param);
        
        public abstract void Enter(EntityBase entity,params object[]param);
        
        public abstract void Process(EntityBase entity,params object[]param);
    }

    public enum State
    {
        None,
        Idle,
        Move,
        Attack,
        Born,
        Die,
        Hit,
    }
}