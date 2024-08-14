using PENet;
using Protocal;
using Server._01Service;
using Server._02System;

namespace Server
{
    public class ServerSession:PESession<NetMsg>
    {
        protected override void OnConnected()
        {
            NetCommon.Log("Client Connect");
        }
        protected override void OnDisConnected()
        {
            NetCommon.Log("Client DisConnect");
            LoginSystem.Instance.AccOutLine(this);
        }
        protected override void OnReciveMsg(NetMsg msg)
        {
            NetCommon.Log("Client Request:"+ ((TransCode)msg.cmd).ToString());
            NetService.Instance.HandleRequest(new MsgPack(this,msg));
        }
    }
    public class MsgPack {
        public NetMsg netMsg;
        public ServerSession session;
        public MsgPack(ServerSession session,NetMsg msg) {
            this.netMsg = msg;
            this.session = session;
        }
    }
}
