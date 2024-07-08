using Battle.FSM;
using Config;
using DarkGod.System;
using UnityEngine;
using Wwc.Cfg;

namespace Battle.Entity
{
    public class MonsterEntity : EntityBase
    {
        public int lv;

        public MonsterEntity()
        {
            entityType = EntityType.Monster;
        }

        public override void SetBattleData(BattleData battleData)
        {
            BattleData temp = new BattleData()
            {
                hp = battleData.hp * lv,
                ad = battleData.ad * lv,
                ap = battleData.ap * lv,
                addef = battleData.addef * lv,
                apdef = battleData.apdef * lv,
                dodge = battleData.dodge * lv,
                pierce = battleData.pierce * lv,
                critical = battleData.critical * lv,
            };
            _battleData = temp;
            currentHp = temp.hp;
        }

        public override EntityBase GetCloseTarget()
        {
            return battleMgr.playerEntity;
        }

        protected float aiAttackTimer = 2;
        protected float aiAttackCounter = 0;

        public override void TickAiLogic()
        {
            if(BattleSystem.Instance.gameState==GameState.End) return;
            if (battleMgr.playerEntity == null || battleMgr.playerEntity.currentState == State.Die ||
                currentState == State.Born)
            {
                return;
            }

            if (aiCounter <= aiTimer)
            {
                aiCounter += Time.deltaTime;
            }
            else
            {
                aiCounter -= aiTimer;
                //判断攻击距离
                if (!InAttackRange(attackDis))
                {
                    //设置方向
                    Vector2 dir = GetTarDir();
                    SetMoveDir(dir);
                    //设置状态
                    Move();
                    aiAttackCounter = aiAttackTimer;
                }
            }

            if (InAttackRange(attackDis))
            {
                SetMoveDir(Vector2.zero);
                //判断是否在CD
                if (aiAttackTimer <= aiAttackCounter)
                {
                    aiAttackCounter -= aiAttackTimer;
                    Attack(skillID);
                }
                else
                {
                    aiAttackCounter += Time.deltaTime;
                    Idle();
                }
            }
        }
    }
}