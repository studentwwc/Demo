using System.Collections.Generic;
using Service;
using UnityEngine;

namespace Battle
{
    public abstract class Controller:MonoBehaviour
    {
        protected TimerService _timerService;
        protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();
        protected CharacterController _cc;
        [HideInInspector] public Animator ani;
        protected Vector2 dir;
        protected bool isMove;
        protected bool isSkillMove;
        protected float skillMoveSpeed;
        protected Transform cameraTran;

        [SerializeField] public Transform hpTrans;
        public Vector2 Dir
        {
            get { return dir; }
            set
            {
                if (value != Vector2.zero)
                {
                    isMove = true;
               
                }
                else
                {
                    isMove = false;
               
                }
                dir = value;
            }
        }

        public virtual void Init()
        {
            _cc = GetComponent<CharacterController>();
            ani = GetComponent<Animator>();
            _timerService=TimerService.Instance;
        }

        public virtual void SetBlend(int blend)
        {
            ani.SetFloat("Blend", blend);
        }

        public virtual void SetAction(int act)
        {
            ani.SetInteger("Action",act);
        }

        public  virtual void SetFx(string name,float time)
        {
            GameObject temp = null;
            fxDic.TryGetValue(name,out temp);
            if (temp != null)
            {
                temp.gameObject.SetActive(true);
                _timerService.AddTimerTask((tid) =>
                {
                    temp.gameObject.SetActive(false);
                },time);
            }
        }

        public virtual void SetSkillMoveState(bool isSkillMove,float skillMoveSpeed=0)
        {
            this.isSkillMove = isSkillMove;
            this.skillMoveSpeed = skillMoveSpeed;
        }

        public virtual void SetAttackDir(Vector2 dir)
        {
            float angle = Vector2.SignedAngle(dir, new Vector2(0, 1));
            transform.localEulerAngles = new Vector3(0, angle, 0)+new Vector3(0,cameraTran.localEulerAngles.y,0);
        }
        public virtual void SetAttackDirNotCam(Vector2 dir)
        {
            float angle = Vector2.SignedAngle(dir, new Vector2(0, 1));
            transform.localEulerAngles = new Vector3(0, angle, 0);
        }
    }
}