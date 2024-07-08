using PENet;
using Protocal;
using Server._02System;
using Server.Frame;
using System.Collections.Generic;

namespace Server._01Service
{
    internal class NetService : Singleton<NetService>
    {
        private static readonly string obj = "lock";
        private Queue<MsgPack> msgQueue;
        public void Init()
        {
            PESocket<ServerSession, NetMsg> server = new PENet.PESocket<ServerSession, NetMsg>();
            server.StartAsServer(IPCfg.srv, IPCfg.port);
            msgQueue = new Queue<MsgPack>();
            NetCommon.Log("NetService Init Done");
        }
        public void AddMsg(MsgPack msg)
        {
            lock (obj)
            {

                msgQueue.Enqueue(msg);

            }
        }
        public void Update()
        {
            if (msgQueue.Count > 0) {
                lock (obj) {
                   MsgPack msg= msgQueue.Dequeue();
                   HandleRequest(msg);
                }
            }
        }
        public void HandleRequest(MsgPack msgPack)
        {
            switch ((TransCode)msgPack.netMsg.cmd)
            {
                case TransCode.None:
                    break;
                case TransCode.LoginReq:
                    LoginSystem.Instance.HandleLoginRequest(msgPack);
                    break;
                case TransCode.RenameReq:
                    LoginSystem.Instance.HandleRenameRequest(msgPack);
                    break;
                case TransCode.TaskReq:
                    TaskSystem.Instance.HandleGuideReq(msgPack);
                    break;
                case TransCode.StrongReq:
                    StrongSystem.Instance.HandleStrongReq(msgPack);
                    break;
                case TransCode.ChatReq:
                    ChatSystem.Instance.HandleChatReq(msgPack);
                    break;
                case TransCode.BuyReq:
                    BuySystem.Instance.HandleBuyReq(msgPack);
                    break;
                case TransCode.TaskRewardReq:
                    TaskSystem.Instance.HandleTaskReward(msgPack);
                    break;
                case TransCode.LevelPassReq:
                    LevelPassSystem.Instance.HandleLevelPassReq(msgPack);
                    break;
                case TransCode.LevelPassStateReq:
                    LevelPassSystem.Instance.HandleLevelPassStateReq(msgPack);
                    break;
                case TransCode.AllPackReq:
                    InventorySystem.Instance.HandleGetAllPackReq(msgPack);
                    break;
                case TransCode.UpdatePackStateReq:
                    InventorySystem.Instance.HandleUpdatePackStateReq(msgPack);
                    break;
                case TransCode.UpdateUserPropReq:
                    InventorySystem.Instance.HandlePropUpdateReq(msgPack);
                    break;
                case TransCode.BuyShopItemReq:
                    BuySystem.Instance.HandleBuyShopItemReq(msgPack);
                    break;
            }
        }
    }
}
