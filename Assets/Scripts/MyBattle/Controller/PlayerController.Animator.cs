using System.Collections.Generic;
using UnityEngine;

namespace Ws.Battle
{
    public partial class PlayerController
    {
        private Animator _animator;
        private Dictionary<string,AnimatorClipInfo> animationClipInfoDic;

        public void BlendSpeedAnimator()
        {
            if (TargetSpeed == 0)
            {
                _animator.SetFloat("Speed",0);
                updates -= BlendSpeedAnimator;
                return;
            }
            if (TargetSpeed - CurrentSpeed < 0.05f)
            {
                CurrentSpeed = TargetSpeed;
                updates -= BlendSpeedAnimator;
            }
            _animator.SetFloat("Speed",CurrentSpeed/TargetSpeed);
        }
        public float GetAnimationLength(string name)
        {
            if (animationClipInfoDic == null)
            {
                animationClipInfoDic = new Dictionary<string, AnimatorClipInfo>();
                AnimatorClipInfo[]infos=_animator.GetCurrentAnimatorClipInfo(0);
                for (int i = 0; i < infos.Length; i++)
                {
                    animationClipInfoDic.Add(infos[i].clip.name,infos[i]);
                }
            }

            return animationClipInfoDic[name].clip.length;
        }
    }
}