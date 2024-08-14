using Server.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server._01Service._03Timer
{
    public class TimerPack {
        public Action<int> cb;
        public int tid;
        public TimerPack(Action<int> cb,int tid) {
            this.cb = cb;
            this.tid = tid;
        }
    }
    public class TimerService:Singleton<TimerService>
    {
        public PETimer pt;
        Queue<TimerPack> timerPacks = new Queue<TimerPack>();
        private static readonly string timerLock = "";
        public void Init() {
            pt = new PETimer(100);
            timerPacks.Clear();
            pt.SetLog((string log) => { NetCommon.Log(log); });
            NetCommon.Log("TimerService Init Done");
            pt.SetHandle((Action<int> cb, int tid) => {
                if (cb != null) {
                    lock (timerLock) {
                        timerPacks.Enqueue(new TimerPack(cb,tid));
                    }
                }
            });
        }
        public void AddTimerTask(Action<int>callBack,double delay,PETimeUnit unit=PETimeUnit.Millisecond,int count=1) {
            pt.AddTimeTask(callBack,delay,unit,count);

        }
        public void Update() {
            while (timerPacks.Count > 0) {
                TimerPack pack = null;
                lock (timerLock) {
                    pack = timerPacks.Dequeue();
                }
                pack.cb(pack.tid);
            }
        }
    }
}
