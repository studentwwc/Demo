using System;
using System.Collections.Generic;
using Service;
using UnityEngine;

namespace Ws.Battle
{
    public partial class PlayerController:Controller
    {
        private CharacterController _cc;
        public int TargetSpeed;
        public float CurrentSpeed;
        private Vector3 dir;
        private float Gravity = 9.8f;
        private float GravityVol = 0;
        private List<int> AttackAction;
        
        public Action updates;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _cc = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            AttackAction=new List<int>(2);
            GameRoot.Instance.updates += GravityMove;
        }
        
        private void Update()
        {
            PlayerInput();
            updates?.Invoke();
        }

        public void PlayerInput()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Dir = new Vector3(h, 0,v);
            if (Input.GetKeyDown(KeyCode.X))
            {
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Skill1();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Skill2();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Skill3();
            }
        }
        public Vector3 Dir
        {
            get
            {
                return dir;
            }
            set
            {
                if (dir == Vector3.zero && value != Vector3.zero)
                {
                    updates += BlendSpeed;
                    updates += BlendSpeedAnimator;
                    updates += Move;
                    TargetSpeed = 10;
                }
                dir = value;
                transform.LookAt(transform.localPosition+dir);
                if (value == Vector3.zero)
                {
                    updates -= Move;
                    CurrentSpeed = 0;
                    TargetSpeed = 0;
                }
            }
        }
        public void Move()
        {
            currentFlag=_cc.Move(Dir * CurrentSpeed);
        }

        public void BlendSpeed()
        {
            if (TargetSpeed == 0)
            {
                _animator.SetFloat("Speed",0);
                CurrentSpeed = 0;
                updates -= BlendSpeed;
                return;
            }

            if (CurrentSpeed < TargetSpeed)
            {
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, 0.1f);
            }

            if (TargetSpeed - CurrentSpeed < 0.05f)
            {
                CurrentSpeed = TargetSpeed;
                updates -= BlendSpeed;
            }
        }

        private CollisionFlags currentFlag;
        private float speed;
        private float tempTime;
        private float targetTime;
        public override void SkillMove(Vector3 dir,float distance,float time)
        {
            speed = distance / time;
            tempTime = 0;
            GameRoot.Instance.updates += BeginSkillMove;

        }

        public void BeginSkillMove()
        {
            currentFlag=_cc.Move(dir * speed*Time.deltaTime);
            tempTime += Time.deltaTime;
            if (Math.Abs(tempTime - targetTime) < Time.deltaTime)
            {
                GameRoot.Instance.updates -= BeginSkillMove;
            }
        }

        public void GravityMove()
        {
            if (currentFlag != CollisionFlags.CollidedBelow)
            {
                GravityVol -= Gravity*Time.deltaTime;
                currentFlag=_cc.Move(transform.up * GravityVol*Time.deltaTime);
            }
        }
    }
}