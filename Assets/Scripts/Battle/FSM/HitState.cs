using Battle.Entity;
using Common;
using Service;
using UnityEngine;

namespace Battle.FSM
{
    public class HitState:IState
    {
        public override void Exit(EntityBase entity, params object[] param)
        {
            
        }

        public override void Enter(EntityBase entity, params object[] param)
        {
            entity.currentState = State.Hit;
            for (int i = 0; i < entity.moveLstCb.Count; i++)
            {
                TimerService.Instance.RemoveTimerTask(entity.moveLstCb[i]);
            }
            for (int i = 0; i < entity.actionLstCb.Count; i++)
            {
                TimerService.Instance.RemoveTimerTask(entity.actionLstCb[i]);
            }
        }

        public override void Process(EntityBase entity, params object[] param)
        {
           entity.SetAction(Constant.hitAction);
           if (entity.entityType == EntityType.Player)
           {
               AudioService.Instance.PlayPlayerAudio(Constant.AudioPlayerHit,entity.audioSource);
           }

           TimerService.Instance.AddTimerTask((tid) =>
           {
               entity.Idle();
           },entity.GetHitLenght());
        }
    }
}