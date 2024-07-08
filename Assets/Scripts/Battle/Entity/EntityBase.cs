using System.Collections.Generic;
using Battle.FSM;
using battle.mgr;
using Config;
using UnityEngine;
using Wwc.Cfg;

namespace Battle.Entity
{
    public enum EntityType
    {
        None,
        Player,
        Monster,
    }

    public enum EntityState
    {
        None,
        HotSeat,
    }

    public abstract class EntityBase
    {
        public string entityName;
        public EntityType entityType = EntityType.None;
        public BattleMgr battleMgr;
        public State currentState=State.None;
        public EntityState entityState = EntityState.None;
        public StateMgr stateMgr=null;
        public SkillMgr skillMgr = null;
        protected Controller controller = null;
        public bool isCanMove=true;
        protected BattleData _battleData;
        protected int currentHp;
        public SkillData currentSkill;
        public int skillID;
        public float attackDis;
        public bool isCanSkill=true;
        public AudioSource audioSource;
        public List<int> moveLstCb=new List<int>();
        public List<int> actionLstCb=new List<int>();
        public BattleData Props
        {
            get => _battleData;
            protected set => _battleData = value;
        }

        public int CurrentHp
        {
            get => currentHp;
            set
            {
                NetCommon.Log("HP from "+currentHp+" to "+value);
                SetHpChange(currentHp,value);
                currentHp = value;
            }
        }

        public virtual void SetBattleData(BattleData battleData)
        {
            currentHp = battleData.hp;
            Props = battleData;
        }

        #region 设置实体状态

        public void Idle()
        {
            stateMgr.ChangeState(this,State.Idle);
        }
        public void Move()
        {
            stateMgr.ChangeState(this,State.Move);
        }
        public void Attack(int skillId)
        {
            stateMgr.ChangeState(this,State.Attack,skillId);
        }
        public void Born()
        {
            stateMgr.ChangeState(this,State.Born);
        }
        public void Die()
        {
            stateMgr.ChangeState(this,State.Die);
        }
        public void Hit()
        {
            stateMgr.ChangeState(this,State.Hit);
        }

        #endregion

        #region 设置表现实体
        public virtual void SetBlend(int target)
        {
            if (controller != null)
            {
                controller.SetBlend(target);
            }
        }

        public virtual void SetMoveDir(Vector2 dir)
        {
            controller.Dir = dir;
        }

        public virtual void SetAction(int act)
        {
            controller.SetAction(act);
        }
        public virtual void SetFx(string name,float time)
        {
            controller.SetFx(name,time);
        }

        public virtual void SetSkillMoveState(bool isSkillMove,float speed=0f)
        {
            controller.SetSkillMoveState(isSkillMove,speed);
        }
        

        #endregion

        public Queue<int> comboQueue = new Queue<int>();
        public int nextSkill=0;
        public virtual void SkillAttact(int skillId)
        {
            skillMgr.SkillAttack(this,skillId);
        }

        public virtual Vector2 GetCurrentDir()
        {
            return Vector2.zero;
        }

        public virtual Transform GetCurrentTrans()
        {
            return controller.transform;
        }
        public virtual Vector3 GetCurrentPos()
        {
            return controller.transform.localPosition;
        }

        public virtual void SetHpChange(int oldHp,int newHp)
        {
            GameRoot.Instance.dynamicWnd.SetHpChange(controller.gameObject.name,oldHp,newHp);
        }

        public void SetController(Controller controller)
        {
            this.controller = controller;
        }

        public void SetActive(bool valid=true)
        {
            if (controller != null)
            {
                controller.gameObject.SetActive(valid);
            }
        }

        public void SetAttackDir(Vector3 dir,bool isCamOffset=true)
        {
            if (isCamOffset)
            {
                controller.SetAttackDir(dir);
            }
            else
            {
                controller.SetAttackDirNotCam(dir);
            }
        }

        public float GetHitLenght()
        {
            AnimationClip[] animationClips = controller.ani.runtimeAnimatorController.animationClips;
            for (int i = 0; i < animationClips.Length; i++)
            {
                if (animationClips[i].name.Contains("Hit") || animationClips[i].name.Contains("hit") ||
                    animationClips[i].name.Contains("HIT"))
                {
                    return animationClips[i].length;
                }
            }
           
            return 1;
        }

        public Vector2 GetTarDir()
        {
            EntityBase entityBase= GetCloseTarget();
            return new Vector2(entityBase.GetCurrentPos().x - GetCurrentPos().x,
                entityBase.GetCurrentPos().z - GetCurrentPos().z);
        }

        public virtual EntityBase GetCloseTarget()
        {
            return null;
        }

        protected float aiTimer = 2;
        protected float aiCounter = 0;
        public virtual void TickAiLogic()
        {
        }

        public bool InAttackRange(float dis)
        {
           
            EntityBase entityBase = GetCloseTarget();
            return Vector3.Distance(entityBase.GetCurrentPos(), GetCurrentPos()) <= dis;

        }

        public virtual void SetDodge()
        {
            GameRoot.Instance.dynamicWnd.SetDodge(entityName);
        }

        public virtual void SetDamage(int damage,bool isCritical=false)
        {
            if (isCritical)
            {
                GameRoot.Instance.dynamicWnd.SetCritical(entityName,damage);
            }
            else
            {
                GameRoot.Instance.dynamicWnd.SetDamage(entityName,damage);
            }
        }

        public void RemoveMoveCb(int id)
        {
            for (int i = 0; i < moveLstCb.Count; i++)
            {
                if (id == moveLstCb[i])
                {
                    moveLstCb.Remove(id);
                    break;
                }
            }
        }
        public void RemoveActionCb(int id)
        {
            for (int i = 0; i < actionLstCb.Count; i++)
            {
                if (id == actionLstCb[i])
                {
                    actionLstCb.Remove(id);
                    break;
                }
            }
        }
    }
}