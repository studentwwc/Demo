using System;
using UnityEngine;

namespace Service
{
    public class Delay
    {
        public float TargetTime;
        public float CurrentTime;
        public Action action;

        public Delay(Action action,float time)
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
                action?.Invoke();
                GameRoot.Instance.updates -= Update;
            }
        }
    }
}