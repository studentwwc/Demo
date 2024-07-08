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
    internal class LevelPassSystem:Singleton<LevelPassSystem>
    {
        public void Init() {
            NetCommon.Log("LevelPassSystem Init Done", NetLogType.Normal);
        }
        public void HandleLevelPassReq(MsgPack msgPack) {
            NetMsg netMsg = new NetMsg();
            PlayerData pd = CacheService.Instance.GetPlayerDataBySession(msgPack.session);
            MapConfig mc = CfgService.Instance.GetMapConfig(msgPack.netMsg.levelPassReq.lid);
            if (pd.power < mc.power)
            {
                netMsg.err = (int)NetErrorCode.LackPower;
            } else if (pd.levelpass<mc.ID) {
                netMsg.err = (int)NetErrorCode.UnMatch;
            }
            else {
                pd.power -= mc.power;
                if (CacheService.Instance.UpdatePlayerData(msgPack.session))
                {
                    netMsg.levelPassRes = new LevelPassRes()
                    {
                        lid = mc.ID,
                        power = pd.power
                    };
                    netMsg.cmd = (int)TransCode.LevelPassRes;
                }
                else {
                    netMsg.err = (int)NetErrorCode.UpdateFail;
                }
            }
            msgPack.session.SendMsg(netMsg);
        }
        public void HandleLevelPassStateReq(MsgPack msgPack)
        {
            NetMsg netMsg = new NetMsg();
            PlayerData pd = CacheService.Instance.GetPlayerDataBySession(msgPack.session);
            LevelPassStateReq levelPassStateReq = msgPack.netMsg.levelPassStateReq;
            MapConfig mc = CfgService.Instance.GetMapConfig(levelPassStateReq.lid);
            if (mc != null && levelPassStateReq != null && levelPassStateReq.state)
            {
                pd.levelpass = msgPack.netMsg.levelPassStateReq.lid + 1;
                bool res= CacheService.Instance.UpdatePlayerData(msgPack.session);
                if (res)
                {
                    netMsg.cmd = (int)TransCode.LevelPassStateRes;
                    netMsg.levelPassStateRes = new LevelPassStateRes() {
                        lid = pd.levelpass
                    };
                }
                else {
                    netMsg.err = (int)NetErrorCode.UpdateFail;
                    pd.levelpass = msgPack.netMsg.levelPassStateReq.lid;
                }
            }
            else if(mc==null){
                netMsg.err = (int)NetErrorCode.UnMatch;
            }
            msgPack.session.SendMsg(netMsg);
        }
    }
}
