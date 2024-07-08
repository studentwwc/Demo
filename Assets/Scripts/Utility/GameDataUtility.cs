using System.Collections.Generic;
using DarkGod.System;
using Protocal;
using Service;

namespace Utility
{
    public static class GameDataUtility
    {
        public static Dictionary<int, PlayerPack> playerPacks = new Dictionary<int, PlayerPack>();
        public static int roleid;

        public static void SetUp()
        {
            NetService.Instance.SendMsg(new NetMsg()
            {
                cmd =(int)TransCode.AllPackReq,
                allPackReq = new AllPackReq()
                {
                    userid = GameRoot.Instance.PlayerData.id
                }
            });
        }
        public static void HandleAllPackRes(NetMsg msg)
        {
            AllPackRes packRes= msg.allPackRes;
            for (int i = 0; i < packRes.packs.Count; i++)
            {
                playerPacks.Add(packRes.packs[i].id,packRes.packs[i]);
            }
        }
        public static void AddPack(int id,int goodsid,int count,int index)
        {
            if (playerPacks.ContainsKey(id))
            {
                playerPacks[id].count += count;
                return;
            }

            InventorySystem.Instance.AddPack(id,goodsid,count,index);
            playerPacks.Add(id,new PlayerPack()
            {
                id = id,
                count = count,
                packindex = index,
                userid = GameRoot.Instance.PlayerData.id
            });
        }

        public static void ChangeIndex(int id,int index)
        {
            playerPacks[id].packindex = index;
            NetService.Instance.SendMsg(new NetMsg()
                        {
                            cmd =(int)TransCode.UpdatePackStateReq,
                            updatePackStateReq = new UpdatePackStateReq()
                            {
                                packId = id,
                                changeIndex = index,
                                opt =1,
                            }
                        });
        }
        public static void CalculateExp(PlayerData pd,int addexp) {
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
    }
}