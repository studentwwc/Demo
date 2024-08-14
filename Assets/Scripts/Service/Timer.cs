using System;
using UnityEngine;

namespace Service
{
    public class Timer
    {
        public float TargetTime;
        public float CurrentTime;
        public Action action;

        public Timer(Action action,float time)
        {
            this.action = action;
            TargetTime = time;
            GameRoot.Instance.updates += Update;
        }

        public void Update()
        
        {
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= TargetTime)
            {
                GameRoot.Instance.updates -= Update;
                return;
            }

            action?.Invoke();
            
        }
    }
}