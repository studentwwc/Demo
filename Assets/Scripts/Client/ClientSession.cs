using PENet;
using Protocal;
using Service;
using UnityEngine;

namespace Client
{
    public class ClientSession:PESession<NetMsg>
    {
        protected override void OnConnected()
        {
            NetCommon.Log("Server Connect");
        }

        protected override void OnDisConnected()
        {
            NetCommon.Log("Server DisConnect");
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            NetCommon.Log("Server Response:"+((TransCode)msg.cmd).ToString());
            NetService.Instance.AddMsgRes(msg);
        }
    }
}