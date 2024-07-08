using System.Collections.Generic;
using DarkGod.System;
using UnityEngine;

namespace Battle.Entity
{
    public class PlayerEntity:EntityBase
    {
        public PlayerEntity()
        {
            entityType = EntityType.Player;
        }
        public override Vector2 GetCurrentDir()
        {
            return battleMgr.GetCurrentDir();
            
        }
        public override EntityBase GetCloseTarget()
        {
            List<MonsterEntity> monsterEntities= battleMgr.GetAllMonster();
            if (monsterEntities.Count <= 0)
            {
                return null;
            }

            int closeIndex = 0;//默认第一个最近
            for(int i=1;i<monsterEntities.Count;i++){
                if(Vector3.Distance(GetCurrentPos(),monsterEntities[i].GetCurrentPos())<
                                                    Vector3.Distance(GetCurrentPos(),monsterEntities[closeIndex].GetCurrentPos()))
                {
                    closeIndex = i;
                }
            }
            return monsterEntities[closeIndex];
        }

        public override void SetDodge()
        {
            GameRoot.Instance.dynamicWnd.SetSelfDodge();
        }
        public override void SetHpChange(int oldHp,int newHp)
        {
            BattleSystem.Instance._playerCtWnd.SetSelfHp(newHp);
        }
    }
}