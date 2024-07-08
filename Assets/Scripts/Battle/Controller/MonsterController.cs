using System;
using Common;
using UnityEngine;

namespace Battle
{
    public class MonsterController:Controller
    {
        private void Update()
        {
            if (isMove)
            {
                SetMove();
                SetDir();
            }
        }
        public void SetMove()
        {
            _cc.Move(transform.forward * Time.deltaTime * Constant.monsterMoveSpeed);
            _cc.Move(-transform.up * Time.deltaTime * Constant.monsterMoveSpeed);
        }
        public void SetDir()
        {
            if (dir != Vector2.zero)
            {
                float angle = Vector2.SignedAngle(dir, new Vector2(0, 1));
                transform.localEulerAngles = new Vector3(0, angle, 0);
            }
        }
    }
}