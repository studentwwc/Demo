using System;
using UnityEngine;

namespace Stray
{
    public class LoopDragonAnim:MonoBehaviour
    {
        private Animation _animation;

        private void Awake()
        {
            _animation = GetComponent<Animation>();
            InvokeRepeating("PlayDragonAnim",0,_animation.GetClip("long@long_fly_around").length+1);
        }

        public void PlayDragonAnim()
        {
            if (_animation!=null)
            {
                _animation.Play();
            }
        }
    }
}