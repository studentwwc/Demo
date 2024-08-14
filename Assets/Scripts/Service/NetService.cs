using System;
using System.Collections.Generic;
using Client;
using DarkGod.System;
using PENet;
using Protocal;
using UnityEngine;
using Utility;
using Color = Common.Color;

namespace Service
{
    public class NetService:MonoBehaviour
    {
        public static NetService Instance;
        private PESocket<ClientSession,NetMsg> client;
        private static readonly string obj = "";//为保证数据安全锁的
        private Queue<NetMsg> msgQueue;
        public void Init()
        {
            Instance = this;
            client = new PESocket<ClientSession, NetMsg>();
            msgQueue = new Queue<NetMsg>();
            client.SetLog(true,((s, i) =>
            {
                switch (i)
                {
                    case 0:
                        Debug.Log("Log:"+s);
                        break;
                    case 1:
                        Debug.LogWarning("Warning:"+s);
                        break;
                    case 2:
                        Debug.LogError("Error:"+s);
                        break;
                    case 3:
                        Debug.Log("Info"+s);
                        break;
                }
            }));
            client.StartAsClient(IPCfg.srv,IPCfg.port);
	
        }

        public void AddMsgRes(NetMsg msg)
        {
            lock (obj)
            {
                msgQueue.Enqueue(msg);
            }
        }

        public void HandleRes(NetMsg netMsg)
        {
            if (netMsg.err != (int)NetErrorCode.None)
            {
                switch ((NetErrorCode)netMsg.err)
                {
                    case NetErrorCode.AccOnLine:
                        GameRoot.Instance.AddTips("用户已登录");
                        break;
                    case NetErrorCode.ErrorPassword:
                        GameRoot.Instance.AddTips("密码错误");
                        break;
                    case NetErrorCode.NameExisted:
                        GameRoot.Instance.AddTips("名字已存在,请重新输入");
                        break;
                    case NetErrorCode.UpdateFail:
                        GameRoot.Instance.AddTips("数据库更新发生错误");
                        break;
                    case NetErrorCode.UnMatch:
                        GameRoot.Instance.AddTips("客户端信息异常");
                        break;
                    case NetErrorCode.LackLevel:
                        GameRoot.Instance.AddTips("等级不足");
                        break;
                    case NetErrorCode.LackCoin:
                        GameRoot.Instance.AddTips("金币不足");
                        break;
                    case NetErrorCode.LackCrystal:
                        GameRoot.Instance.AddTips("水晶不足");
                        break;
                    case NetErrorCode.LackDiamond:
                        GameRoot.Instance.AddTips("钻石不足");
                        break;
                    case NetErrorCode.LackPower:
                        GameRoot.Instance.AddTips("体力不足");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return;
            }

            switch ((TransCode)netMsg.cmd)
            {
                case TransCode.None:
                    GameRoot.Instance.AddTips("TransCode为空",Color.Red);
                    break;
                case TransCode.LoginRes:
                    LoginSys.Instance.LoginResp(netMsg);
                    break;
                case TransCode.RenameRes:
                    LoginSys.Instance.RenameResp(netMsg);
                    break;
                case TransCode.TaskRes:
                    MainCitySys.Instance.TaskRes(netMsg);
                    CheckHasUnionTaskProg(netMsg);
                    break;
                case TransCode.StrongRes:
                    MainCitySys.Instance.StrongRes(netMsg);
                    CheckHasUnionTaskProg(netMsg);
                    break;
                case TransCode.ChatRes:
                    MainCitySys.Instance.ChatRes(netMsg);;
                    CheckHasUnionTaskProg(netMsg);
                    break;
                case  TransCode.BuyRes:
                    MainCitySys.Instance.BuyRes(netMsg);
                    CheckHasUnionTaskProg(netMsg);
                    break;
                case TransCode.RepowerRes:
                    MainCitySys.Instance.RepowerRes(netMsg);
                    break;
                case TransCode.TaskRewardRes:
                    MainCitySys.Instance.TaskRewardRes(netMsg);
                    CheckHasUnionTaskProg(netMsg);
                    break;
                case TransCode.TaskProgRes:
                    MainCitySys.Instance.TaskProgRes(netMsg);
                    break;
                case TransCode.LevelPassRes:
                    LevelSystem.Instance.LevelPassRes(netMsg);
                    break;
                case TransCode.AllPackRes:
                    GameDataUtility.HandleAllPackRes(netMsg);
                    break;
                case TransCode.LevelPassStateRes:
                    LevelSystem.Instance.HandleLevelPassStateRes(netMsg);
                    break;
                case TransCode.BuyShopItemRes:
                    MainCitySys.Instance.BuyShopItemRes(netMsg.buyShopItemRes);
                    break;
                default:
                    NetCommon.Log("传输码不存在",NetLogType.Error);
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SendMsg(NetMsg netMsg)
        {
            if (client.session != null)
            {
                client.session.SendMsg(netMsg);
            }
            else
            {
                Init();
                client.session.SendMsg(netMsg);
                
            }
        }
        private void Update()
        {
            if (msgQueue.Count > 0)
            {
                lock (obj)
                {
                    NetMsg netMsg= msgQueue.Dequeue();
                    HandleRes(netMsg);
                }
            }
        }

        public void CheckHasUnionTaskProg(NetMsg netMsg)
        {
            if (netMsg.taskProgRes != null)
            {
                MainCitySys.Instance.TaskProgRes(netMsg);
            }
        }
    }
}