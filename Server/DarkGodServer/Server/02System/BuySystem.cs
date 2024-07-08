using Protocal;
using Server._01Service;
using Server._03Cache;
using Server._04DB;
using Server.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server._02System
{
    internal class BuySystem: Singleton<BuySystem>
    {
        public void Init()
        {
            NetCommon.Log("BuySystem Init Done", NetLogType.Normal);
        }
        public void HandleBuyReq(MsgPack msgPack) {
            NetMsg netMsg = new NetMsg();
            BuyReq buyReq = msgPack.netMsg.buyReq;
            PlayerData pd= CacheService.Instance.GetPlayerDataBySession(msgPack.session);
            if (buyReq.cost > pd.diamond)
            {
                netMsg.err = (int)NetErrorCode.LackDiamond;
            }
            else {
                pd.diamond -= buyReq.cost;
                switch (buyReq.buyType) { //0 buy coin 1 buy power
                    case 0:
                        pd.coin += 100 * buyReq.cost;
                        //修改任务进度
                        TaskProgRes progRes = TaskSystem.Instance.HandleTaskProg(5, msgPack.session);
                        if (progRes != null)
                        {
                            netMsg.taskProgRes = progRes;
                        }
                        else
                        {
                            throw new Exception("任务进度异常");
                        }
                        
                        break;
                    case 1:
                        pd.power += 10 * buyReq.cost;
                        //修改任务进度
                        TaskProgRes progRes2 = TaskSystem.Instance.HandleTaskProg(4, msgPack.session);
                        if(progRes2!=null)
                        {
                            netMsg.taskProgRes = progRes2;
                        }
                        else
                        {
                            throw new Exception("任务进度异常");
                        }
                       
                        break;
                }
                if (CacheService.Instance.UpdatePlayerData(msgPack.session))
                {
                    netMsg.buyRes = new BuyRes()
                    {
                        buyType = buyReq.buyType,
                        coin = pd.coin,
                        diamond = pd.diamond,
                        power = pd.power,
                    };
                    netMsg.cmd = (int)TransCode.BuyRes;
                }
                else {
                    netMsg.err = (int)NetErrorCode.UpdateFail;
                }

            }
            msgPack.session.SendMsg(netMsg);
        }


        public void HandleBuyShopItemReq(MsgPack msgPack) {
            NetMsg resMsg = new NetMsg();
            resMsg.cmd = (int)TransCode.BuyShopItemRes;
            BuyShopItemReq buyShopItem = msgPack.netMsg.buyShopItemReq;
            if (CacheService.Instance.GetPlayerDataBySession(msgPack.session).diamond >= buyShopItem.cost)
            {
                CacheService.Instance.GetPlayerDataBySession(msgPack.session).diamond -= buyShopItem.cost;
                CacheService.Instance.UpdatePlayerData(msgPack.session);
                if (buyShopItem.type!= 1)
                {
                    switch (buyShopItem.type) {
                        case 2:
                            CacheService.Instance.GetPlayerDataBySession(msgPack.session).crystal += buyShopItem.count;
                            break;
                        case 3:
                            TaskSystem.Instance.CalculateExp(CacheService.Instance.GetPlayerDataBySession(msgPack.session), buyShopItem.count);
                            break;
                        case 4:
                            CacheService.Instance.GetPlayerDataBySession(msgPack.session).coin += buyShopItem.count;
                            break;
                    }
                    CacheService.Instance.UpdatePlayerData(msgPack.session);
                    resMsg.buyShopItemRes = new BuyShopItemRes()
                    {
                        diamond = CacheService.Instance.GetPlayerDataBySession(msgPack.session).diamond,
                        type = buyShopItem.type,
                        index = 0,
                        count = buyShopItem.count,
                    };
                }
                if (buyShopItem.type == 1)
                {
                    //查询该角色是否有该物品
                    bool isExist = false;
                    List<PlayerPack> playerPacks = DBManager.Instance.GetPlayerPack(CacheService.Instance.GetPlayerDataBySession(msgPack.session).id);
                    int[] indexs = new int[playerPacks.Count];
                    for (int i = 0; i < playerPacks.Count; i++)
                    {
                        indexs[i] = playerPacks[i].packindex;
                        if (playerPacks[i].goodsid == msgPack.netMsg.buyShopItemReq.id)
                        {
                            playerPacks[i].count++;
                            resMsg.buyShopItemRes.count = playerPacks[i].count;
                            isExist = true;
                            resMsg.buyShopItemRes = new BuyShopItemRes()
                            {
                                diamond = CacheService.Instance.GetPlayerDataBySession(msgPack.session).diamond,
                                type = msgPack.netMsg.buyShopItemReq.type,
                                index = playerPacks[i].packindex,
                                count = buyShopItem.count,
                                id= playerPacks[i].id,
                            };
                            DBManager.Instance.UpdatePackCount(CacheService.Instance.GetPlayerDataBySession(msgPack.session).id, msgPack.netMsg.buyShopItemReq.id, playerPacks[i].count);
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        Array.Sort(indexs);
                        int targetIndex = 0;
                        for (int i = 0; i < indexs.Length; i++) {
                            if (!indexs.Contains(i)) {
                                targetIndex = i;
                                break;
                            }
                        }
                        //添加进背包
                        int id=DBManager.Instance.AddPack(CacheService.Instance.GetPlayerDataBySession(msgPack.session).id, msgPack.netMsg.buyShopItemReq.id, 1, targetIndex);
                        resMsg.buyShopItemRes = new BuyShopItemRes()
                        {
                            diamond = CacheService.Instance.GetPlayerDataBySession(msgPack.session).diamond,
                            type = msgPack.netMsg.buyShopItemReq.type,
                            index = targetIndex,
                            count = 1,
                            goodsid = msgPack.netMsg.buyShopItemReq.id,
                            id = id,
                        };
                    }

                }
            }
            else
            {
                resMsg.err = (int)NetErrorCode.UnMatch;
            }
            
            msgPack.session.SendMsg(resMsg);
        }
        List<int> pack;
        public void Add()
        {
            int index = -1;
            for (int i = 0; i < pack.Count; i++) {
                if (pack[i] - index > 1) {
                    index++;
                    break;
                }
                index = pack[i];
            }
            index = index < 0 ? 0:index;

        }
    }

   
}
