using PENet;
using Protocal;
using Server._03Cache;
using Server.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server._02System
{
    internal class ChatSystem : Singleton<ChatSystem>
    {
        public void Init()
        {
            NetCommon.Log("ChatSystem Init Done", NetLogType.Normal);
        }
        public void HandleChatReq(MsgPack msgPack)
        {
            List<ServerSession> sessions = CacheService.Instance.GetAllOnLineSession();
            PlayerData pd = CacheService.Instance.GetPlayerDataBySession(msgPack.session);
            NetMsg netMsg = new NetMsg()
            {
                chatRes = new ChatRes()
                {
                    msg = msgPack.netMsg.chatReq.msg,
                    name = pd.name
                },
                cmd = (int)TransCode.ChatRes,

            };
            //修改任务进度
            TaskProgRes progRes = TaskSystem.Instance.HandleTaskProg(6, msgPack.session);
            if (progRes != null)
            {
                netMsg.taskProgRes = progRes;
            }
            else
            {
                throw new Exception("任务进度异常");
            }
            
       
            byte[] bytes = PETool.PackNetMsg(netMsg);
            foreach (var v in sessions)
            {
                v.SendMsg(bytes);
            }
        }
    }
}
