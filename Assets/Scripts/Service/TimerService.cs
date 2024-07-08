using System;
using UnityEngine;

namespace Service
{
    public class TimerService:MonoBehaviour
    {
        private static TimerService instance;

        public static TimerService Instance
        {
            get
            {
                return instance;
            }
        }

        private PETimer pt;
        
        public void Init()
        {
            instance = this;
            pt = new PETimer();
            pt.SetLog((string log) =>
            {
                NetCommon.Log(log);
            });
            NetCommon.Log("TimerService Init");
        }

        public int AddTimerTask(Action<int>callBack,double time,PETimeUnit unit=PETimeUnit.Millisecond)
        {
            return pt.AddTimeTask(callBack, time, unit);
        }

        private void Update()
        {
            pt.Update();
        }
        public void RemoveTimerTask(int tid)
        {
             pt.DeleteTimeTask(tid);
        }
    }
}