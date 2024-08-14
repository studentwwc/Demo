using Battle.Entity;
using Common;
using Config;
using Service;
using Wwc.Cfg;

namespace Battle.FSM
{
    public class AttackState:IState
    {
        public override void Enter(EntityBase entity,params object[]param)
        {
            entity.currentState = State.Attack;
            entity.currentSkill = ResService.Instance.GetSkillConfig((int)param[0]);
            entity.isCanMove = false;
            //NetCommon.Log(entity.entityName+"Enter Attack State");
        }
        
        public override void Process(EntityBase entity,params object[]param)
        {
            //NetCommon.Log(entity.entityName+"Process Attack State");
            entity.SkillAttact((int)param[0]);
            SkillData data =ResService.Instance.GetSkillConfig((int)param[0]);
            TimerService.Instance.AddTimerTask((tid) => { entity.Idle(); },data.skillTime );
        }
        
        public override void Exit(EntityBase entity,params object[]param)
        {
            entity.isCanMove = true;
            entity.SetAction(Constant.defaultAction);
            if (entity.comboQueue.Count > 0&&entity.currentSkill.isCombo)
            {
                entity.nextSkill = entity.comboQueue.Dequeue();
            }
            else
            {
                entity.comboQueue.Clear();
                entity.nextSkill = 0;
            }

         
        }
    }
}