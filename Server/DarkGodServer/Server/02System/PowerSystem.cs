using Protocal;
using Server._01Service._03Timer;
using Server._03Cache;
using Server.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server._02System
{
    internal class PowerSystem:Singleton<PowerSystem>
    {
        public void Init() {
            TimerService.Instance.AddTimerTask(HandleOnLineRepower, NetCommon.repowerTime, PETimeUnit.Minute, 0);
            NetCommon.Log("PowerSystem Init Done", NetLogType.Normal);
        }

        //处理在线玩家的体力恢复
        public void HandleOnLineRepower(int tid) {
            Dictionary<ServerSession,PlayerData> dic= CacheService.Instance.GetOnLineSessoonDic();
            foreach (var c in dic ) {
                PlayerData temp = c.Value;
                int powerLimit= NetCommon.GetPowerLimit(temp.lv);
                if (temp.power >= powerLimit)
                {
                    continue;
                }
                else {
                    temp.power = temp.power + NetCommon.resumePower > powerLimit ? powerLimit : temp.power + NetCommon.resumePower;
                    temp.repower = (long)TimerService.Instance.pt.GetMillisecondsTime();
                    CacheService.Instance.UpdatePlayerData(c.Key);
                    c.Key.SendMsg(new NetMsg()
                    {   cmd=(int)TransCode.RepowerRes,
                        resumePowerRes = new RePowerRes()
                        {
                            power = temp.power
                        }
                    }) ;
                }
            }
        }
        public void HandleOutLineRepower(MsgPack msgPack) {

            PlayerData pd= CacheService.Instance.GetPlayerDataBySession(msgPack.session);
            int powerLimit = NetCommon.GetPowerLimit(pd.lv);
            if (powerLimit == pd.power) {
                return;
            }
            else{
                long now = (long)TimerService.Instance.pt.GetMillisecondsTime();
                long pre = pd.repower;
                long timeSpace = (1000 * 60 * NetCommon.repowerTime);
                long residue = now % timeSpace;
                int resumeCount = (int)((now - pre) /timeSpace*NetCommon.resumePower);
                pd.repower = now - residue;
                pd.power = pd.power + resumeCount > powerLimit ? powerLimit : pd.power + resumeCount;
                
                CacheService.Instance.UpdatePlayerData(msgPack.session);
                msgPack.session.SendMsg(new NetMsg() {
                    cmd=(int)TransCode.RepowerRes,
                   resumePowerRes=new RePowerRes() { 
                     power=pd.power,
                   }
                });
            }

        
        }
    }
}
