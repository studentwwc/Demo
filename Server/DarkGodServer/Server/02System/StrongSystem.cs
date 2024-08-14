using Protocal;
using Server._01Service;
using Server._03Cache;
using Server.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server._02System
{
    internal class StrongSystem:Singleton<StrongSystem>
    {
        public void Init() {
            NetCommon.Log("StrongSystem Init Done", NetLogType.Normal);
        }
        public void HandleStrongReq(MsgPack msg)
        {
            NetMsg netMsg = new NetMsg();
            int pos = msg.netMsg.strongReq.pos;
            //取数据
            PlayerData pd = CacheService.Instance.GetPlayerDataBySession(msg.session);
            StrongConfig strongConfig=CfgService.Instance.GetStrongConfig(pos, pd.strongArr[pos]+1);

            //对比数据
            if (pd.lv < strongConfig.minlv)
            {
                netMsg.err = (int)NetErrorCode.LackLevel;
            }
            else if (pd.coin < strongConfig.coin)
            {
                netMsg.err = (int)NetErrorCode.LackCoin;
            }
            else if (pd.crystal < strongConfig.crystal)
            {
                netMsg.err = (int)NetErrorCode.LackCrystal;
            }
            else {
                //扣除
                pd.coin -= strongConfig.coin;
                pd.crystal -= strongConfig.crystal;
                //奖励
                pd.strongArr[pos] += 1;
                pd.hp += strongConfig.addhp;
                pd.ap += strongConfig.addhurt;
                pd.ad += strongConfig.addhurt;
                pd.addef += strongConfig.adddef;
                pd.apdef += strongConfig.adddef;
                //修改任务进度
                TaskProgRes progRes = TaskSystem.Instance.HandleTaskProg(3, msg.session);
                if (progRes != null)
                {
                    netMsg.taskProgRes = progRes;
                }
                else
                {
                    throw new Exception("任务进度异常");
                }
               
                //修改缓存
                if (CacheService.Instance.UpdatePlayerData(msg.session))
                {
                    netMsg.strongRes = new StrongRes()
                    {
                        hp=pd.hp,
                        ad = pd.ad,
                        ap=pd.ap,
                        addef=pd.addef,
                        apdef=pd.apdef,
                        coin=pd.coin,
                        crystal=pd.crystal,
                        stongArr=pd.strongArr,

                    };
                    netMsg.cmd =(int) TransCode.StrongRes;
                }
                else
                {
                    netMsg.err = (int)NetErrorCode.UpdateFail;
                }
            }
            msg.session.SendMsg(netMsg);
            
        }
    }
}
