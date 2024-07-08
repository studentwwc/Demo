using System;
using System.Collections;
using Common;
using UnityEngine;

namespace Battle
{
    public class PlayerTest : MonoBehaviour
    {
        private Transform cameraTran;
        [SerializeField] private CharacterController _cc;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject effectSkill1;
        private float currentBlend;
        private int targetBlend;
        private float currentMoveSpeed;
        private Vector3 cameraFollowOffset;
        protected Vector2 dir;
        protected bool isMove;

        public Vector2 Dir
        {
            get { return dir; }
            set
            {
                if (value != Vector2.zero)
                {
                    isMove = true;
                    targetBlend = Constant.playerRunBlend;
                }
                else
                {
                    isMove = false;
                    targetBlend = Constant.playerIdleBlend;
                }

                dir = value;
            }
        }

        public void Start()
        {
            _cc = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            cameraTran = Camera.main.transform;
            cameraFollowOffset = Camera.main.transform.position - transform.position;
        }

        private void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Dir = new Vector2(h, v);
            if (Dir != Vector2.zero)
            {
                SetDir();
            }
            else
            {
            }


            if (isMove)
            {
                SetDir();
                SetMove();
                SetCameraFollow();
            }

            if (targetBlend != currentBlend)
            {
                MixBlend();
            }
        }

        public void SetDir()
        {
            if (dir != Vector2.zero)
            {
                float angle = Vector2.SignedAngle(dir, new Vector2(0, 1));
                transform.localEulerAngles =
                    new Vector3(0, angle, 0) + new Vector3(0, cameraTran.localEulerAngles.y, 0);
            }
        }

        public void SetMove()
        {
            _cc.Move(transform.forward * Time.deltaTime * Constant.playerMoveSpeed);
        }

        public void SetCameraFollow()
        {
            if (cameraTran != null)
            {
                cameraTran.transform.position = transform.position + cameraFollowOffset;
            }
        }


        // public void SetBlend(int tar)
        // {
        //     targetBlend = tar;
        // }

        public void MixBlend()
        {
            if (Mathf.Abs(currentBlend - targetBlend) < Constant.playerMoveAccelerateSpeed * Time.deltaTime)
            {
                currentBlend = targetBlend;
            }
            else if (currentBlend > targetBlend)
            {
                currentBlend -= Time.deltaTime * Constant.playerMoveAccelerateSpeed;
            }
            else
            {
                currentBlend += Time.deltaTime * Constant.playerMoveAccelerateSpeed;
            }

            _animator.SetFloat("Blend", currentBlend);
        }
        public void OnClickSkill1()
        {
           _animator.SetInteger("Action",1);
           effectSkill1.gameObject.SetActive(true);
           StartCoroutine(skill1(0.9f, () =>
           {
                _animator.SetInteger("Action",-1);
                effectSkill1.gameObject.SetActive(false);
           }));
        }

        IEnumerator skill1(float times,Action action)
        {
            yield return new WaitForSeconds(times);
            action();

        }
    }
}