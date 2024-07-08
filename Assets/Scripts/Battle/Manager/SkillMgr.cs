using System;
using System.Collections.Generic;
using Battle.Entity;
using Config;
using Service;
using UnityEngine;
using Wwc.Cfg;
using Random = UnityEngine.Random;

namespace battle.mgr
{
    public class SkillMgr : MonoBehaviour
    {
        private ResService _resService;
        private TimerService _timerService;

        public void Init()
        {
            _resService = ResService.Instance;
            _timerService = TimerService.Instance;
        }

        public void SkillAttack(EntityBase entity, int skillId)
        {
            entity.moveLstCb.Clear();
            entity.actionLstCb.Clear();
            if (entity.entityType != EntityType.Monster)
            {
                if(entity.GetCurrentDir()==Vector2.zero){
                    //TODO:导航去最近的目标
                    EntityBase entityBase= entity.GetCloseTarget();
                    if (entityBase != null)
                    {
                        Vector2 dir = new Vector2(entityBase.GetCurrentPos().x - entity.GetCurrentPos().x,
                            entityBase.GetCurrentPos().z - entity.GetCurrentPos().z);
                        entity.SetAttackDir(dir,false);
                        entity.SetAttackDir(dir,false);
                    }
                }
                else
                {
                    entity.SetAttackDir(entity.GetCurrentDir());
                }
            }

            SkillData skillData= _resService.GetSkillConfig(skillId);
            if (entity.entityType==EntityType.Player&&!skillData.isCollide)
            {
                Physics.IgnoreLayerCollision(7,8);
            }

            if (!skillData.isBreak)
            {
                entity.entityState = EntityState.HotSeat;
            }

            entity.SetMoveDir(Vector2.zero);
            entity.SetAction(skillData.aniAction);
            AttackEffect(entity,skillData);
            SkillMove(entity,skillData);
            AttackDamage(entity,skillData);
            _timerService.AddTimerTask((tid) =>
            {
                Physics.IgnoreLayerCollision(7,8,false);
                entity.isCanSkill = true;
                entity.entityState = EntityState.None;
            },skillData.skillTime);
        }
        //技能伤害
        private void AttackDamage(EntityBase entity, SkillData skillData)
        {
            
            List<int> skillActions = skillData.skillActions;
            int sum = 0;
            for (int i = 0; i < skillActions.Count; i++)
            {
                SkillActionConfig skillActionConfig = _resService.GetSkillActionConfig(skillActions[i]);
                float damage = skillData.skillDamageLst[i];
                sum += skillActionConfig.delayTime;
                if (sum > 0)
                {
                    int tid=_timerService.AddTimerTask((tid) =>
                    {
                        AttackAction(entity, skillActionConfig,skillData.dmgType,damage);
                        entity.actionLstCb.Remove(tid);
                    },sum);
                    entity.actionLstCb.Add(tid);
                }
                else
                {
                    AttackAction(entity, skillActionConfig,skillData.dmgType,damage);
                }
            }
        }

        private void AttackAction(EntityBase entity,SkillActionConfig actionConfig,DamageType type,float damage)
        {
            if (entity.entityType == EntityType.Player)
            {
                List<MonsterEntity>monsterEntities= entity.battleMgr.GetAllMonster();
                for (int i = 0; i < monsterEntities.Count; i++)
                {
                    MonsterEntity monsterEntity = monsterEntities[i];
                    if (IsInRange(entity.GetCurrentPos(),monsterEntity.GetCurrentPos(),actionConfig.radius) 
                        && IsInAngle(entity.GetCurrentTrans(),monsterEntity.GetCurrentPos(),actionConfig.angle))
                    {
                        CalculateDamage(entity,monsterEntity,type,damage);
                    }
                }
            }
            else
            {
                PlayerEntity playerEntity= entity.battleMgr.playerEntity;
                if (IsInRange(entity.GetCurrentPos(),playerEntity.GetCurrentPos(),actionConfig.radius) 
                    && IsInAngle(entity.GetCurrentTrans(),playerEntity.GetCurrentPos(),actionConfig.angle))
                {
                    CalculateDamage(entity,playerEntity,type,damage);
                }
            }
        }

        private void CalculateDamage(EntityBase entity,EntityBase monster,DamageType type,float damage)
        {
            if (entity.entityType == monster.entityType)
            {
                return;
            }

            int resDamage =(int)damage ;
            bool isCritical = false;
            switch (type)
            {
                case DamageType.AD:
                {
                    //计算闪避
                    int randomDo= Random.Range(1, 100);
                    if (randomDo <= monster.Props.dodge)
                    {
                     
                        monster.SetDodge();
                        return;
                    }
                    //附加属性
                    resDamage += entity.Props.ad;
                    //计算暴击
                    int randCri= Random.Range(1, 100);
                    if (randCri <= entity.Props.critical)
                    {
                        float rand= Random.Range(1.0f,2.0f);
                        resDamage += (int)((rand - 1) * resDamage);
                        isCritical = true;
                    }
                    //计算穿甲
                    resDamage-=(int)((100 - entity.Props.pierce) / 100.0f * monster.Props.addef);
                }
                    break;
                case DamageType.AP:
                {
                    //附加属性
                    resDamage += entity.Props.ap;
                    
                    resDamage-=monster.Props.apdef;
                }
                    break;
            }

            if (resDamage <= 0)
            {
                resDamage = 5;
            }

            if (monster.CurrentHp <=resDamage)
            {
                monster.CurrentHp = 0;
                monster.Die();
                monster.battleMgr.RemoveMonster(monster);
            }
            else
            {
                monster.CurrentHp -=resDamage;
               if (monster.entityState != EntityState.HotSeat)
                {
                    monster.Hit();
                }
            }

            monster.SetDamage(resDamage,isCritical);
        }

        private bool IsInRange(Vector3 from,Vector3 to,float range)
        {
            return Vector3.Distance(from, to) <=range;
        }

        private bool IsInAngle(Transform playerTrans,Vector3 tar,float angle)
        {
            return Vector3.Angle(playerTrans.forward, (tar-playerTrans.position).normalized)<=angle/2;
        }

        //技能效果表现
        private void AttackEffect(EntityBase entity, SkillData skillData)
        {
            if (skillData.fx != null)
            {
                entity.SetFx(skillData.fx, skillData.skillTime);
            }
        }
        private void SkillMove(EntityBase entity,SkillData skillData)
        {
            int sum = 0;
            Debug.Log("wwc------>entityName:"+entity.entityName+"    skillId:"+skillData.ID+"   skillMovesCount:"+skillData.skillMoves.Count);
            for (int i = 0; i < skillData.skillMoves.Count; i++)
            {
                SkillMoveConfig skillMoveConfig = _resService.GetSkillMoveConfig(skillData.skillMoves[i]);
                float skillMoveSpeed = skillMoveConfig.moveDis / (skillMoveConfig.moveTime * 1.0f / 1000);
                
                sum += skillMoveConfig.delayTime;
                if (sum > 0)
                {
                    int tid=_timerService.AddTimerTask(tid => { 
                        entity.SetSkillMoveState(true, skillMoveSpeed);
                        entity.moveLstCb.Remove(tid);
                    }, sum);
                    entity.moveLstCb.Add(tid);
                }
                else
                {
                    entity.SetSkillMoveState(true, skillMoveSpeed);
                }

                sum += skillMoveConfig.moveTime;
                int tid2=_timerService.AddTimerTask((tid) => { entity.SetSkillMoveState(false);  entity.moveLstCb.Remove(tid); },
                    sum);
                entity.moveLstCb.Add(tid2);
            }
        }
    }
}