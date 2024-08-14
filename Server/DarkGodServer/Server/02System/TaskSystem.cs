using Protocal;
using Server._01Service;
using Server._03Cache;
using Server.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server._02System
{
    internal class TaskSystem:Singleton<TaskSystem>
    {
        public void Init()
        {
            NetCommon.Log("TaskSystem Init Done", NetLogType.Normal);
        }
        public void HandleGuideReq(MsgPack msg) {
            NetMsg netMsg = new NetMsg() {
                cmd = (int)TransCode.TaskRes
            };
            PlayerData pd = CacheService.Instance.GetPlayerDataBySession(msg.session);
            int guideid = msg.netMsg.taskReq.guideid;
            AutoGuideConfig guideConfig = CfgService.Instance.getTaskConfig(guideid);
          
            if (pd.guideid != guideid)
            {
                netMsg.err = (int)NetErrorCode.UnMatch;
            }
            else {
                //计算奖励
                CalculateExp(pd, guideConfig.exp);
                pd.guideid = guideid + 1;
                pd.coin = guideConfig.coin;
                //修改任务进度
                if (guideid == 1001) {
                    TaskProgRes progRes= HandleTaskProg(1, msg.session);
                    if (progRes != null)
                    {
                        netMsg.taskProgRes = progRes;
                    }
                    else {
                        throw new Exception("任务进度异常");
                    }
                }
                //修改数据
                bool isException= CacheService.Instance.UpdatePlayerData(msg.session);
                if (!isException)
                {
                    netMsg.err = (int)NetErrorCode.UpdateFail;
                }
                else {
                    netMsg.taskRes = new TaskRes()
                    {
                        coin = pd.coin,
                        exp = pd.exp,
                        lv = pd.lv,
                        guideid=guideid+1
                     
                    };
                }
            }
            msg.session.SendMsg(netMsg);
        
        }

        public void HandleTaskReward(MsgPack msg) {
            int tid = msg.netMsg.taskRewardReq.tid;
            NetMsg netMsg = new NetMsg();
            PlayerData pd = CacheService.Instance.GetPlayerDataBySession(msg.session);
            TaskRewardConfig rewardConfig = CfgService.Instance.GetTaskRewardConfig(tid);
            TaskRewardData rewardData= GetTaskRewardData(tid, pd);
            if (rewardData.prog == rewardConfig.count && !rewardData.isGet)
            {
                rewardData.isGet = true;
                pd.coin += rewardConfig.coin;
                CalculateExp(pd, rewardConfig.exp);
                ChangeTaskRewardData(pd, rewardData);
                if (CacheService.Instance.UpdatePlayerData(msg.session))
                {
                    netMsg.cmd = (int)TransCode.TaskRewardRes;
                    netMsg.taskRewardRes = new TaskRewardRes()
                    {  
                        coin=pd.coin,
                        exp=pd.exp,
                        lv=pd.lv,
                        taskArr=pd.taskReward,
                    };
                }
                else {
                    netMsg.err = (int)NetErrorCode.UpdateFail;
                }
            }
            else {
                netMsg.err = (int)NetErrorCode.UnMatch;
            }
            msg.session.SendMsg(netMsg);
            
        }
        public TaskRewardData GetTaskRewardData(int tid,PlayerData pd) {
            TaskRewardData data = null;
            for (int i = 0; i < pd.taskReward.Length; i++) {
                string []temps=pd.taskReward[i].Split('|');
                if (int.Parse(temps[0]) == tid) {
                    data = new TaskRewardData()
                    {
                        ID = int.Parse(temps[0]),
                        prog = int.Parse(temps[1]),
                        isGet = int.Parse(temps[2])>0
                    };
                    break;
                }
            }
            return data;
        }

        public void ChangeTaskRewardData(PlayerData pd,TaskRewardData data) {
            for (int i = 0; i < pd.taskReward.Length; i++) {
                string[] temps = pd.taskReward[i].Split('|');
                if (int.Parse(temps[0]) ==data.ID)
                {
                    pd.taskReward[i] = data.ID + "|" + data.prog + "|" + (data.isGet ? 1 : 0);
                    break;
                }
            }
        }
        public void CalculateExp(PlayerData pd,int addexp) {
            int currentLv = pd.lv;
            int currentExp = pd.exp;
            int addExp = addexp;
            while (true) {
                int needExp = NetCommon.GetExpLimit(currentLv)-currentExp;
                if (needExp <= addExp)
                {
                    addExp -= needExp;
                    currentLv += 1;
                    currentExp = 0;
                }
                else {
                    currentExp += addExp;
                    pd.exp = currentExp;
                    pd.lv = currentLv;
                    break;
                }
            }
        }

        public TaskProgRes HandleTaskProg(int tid,ServerSession session) {
            PlayerData pd = CacheService.Instance.GetPlayerDataBySession(session);
            TaskRewardData rewardData = GetTaskRewardData(tid, pd);
            TaskRewardConfig rewardConfig = CfgService.Instance.GetTaskRewardConfig(tid);
            if (rewardData.prog < rewardConfig.count) {
                rewardData.prog++;
            }
            ChangeTaskRewardData(pd, rewardData);
            if (CacheService.Instance.UpdatePlayerData(session))
            {
                return new TaskProgRes()
                {
                    taskRewardArr = pd.taskReward,
                };
            }
            else {
                return null;
            }
    
        }
    }
}
