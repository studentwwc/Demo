using System;
using System.Collections.Generic;
using Common;
using Config;
using DarkGod.Bag;
using Protocal;
using Service;
using UnityEngine;
using Utility;
using Wwc.Cfg;

namespace DarkGod.System
{
    public class InventorySystem : SystemRoot<InventorySystem>
    {
        public BagWnd BagWnd;
        public Dictionary<int, Goods> GoodsBag = new Dictionary<int, Goods>();
        public bool isFirst=true;
        public override void InitSystem()
        {
            base.InitSystem();
            NetCommon.Log("Inventory Init Done");
        }
       
        public void SetUp()
        {
            if (!isFirst)
            {
                return;
            }

            isFirst = false;
            Dictionary<int, PlayerPack> playerPacks = GameDataUtility.playerPacks;
            foreach (var playerPack in playerPacks)
            {
                EquipConfig equipConfig = CfgUtility.EquipConfigs[playerPack.Value.goodsid];
                GameObject go = Instantiate(Resources.Load<GameObject>("PrefasUI/GoodsItem"));
                GoodsItem item = go.GetComponent<GoodsItem>();
                item.Init(playerPack.Value.count, equipConfig.AssetIcon);
                item.goodsData = equipConfig;
                item.GoodsType = (GoodsType)equipConfig.EquipType;
                item.goodsData.ID = playerPack.Key;
                item.goodsData.cfgId = playerPack.Value.goodsid;
                GoodsBag.Add(playerPack.Key,new Goods(item, playerPack.Value.count, playerPack.Value.packindex));
                if (playerPack.Value.packindex != -1)
                {
                    go.transform.SetParent(BagWnd.packsTransform.GetChild(playerPack.Value.packindex));
                }
                else
                {
                    EquipType type = (EquipType)equipConfig.EquipType;
                    switch (type)
                    {
                        case EquipType.None:
                            break;
                        case EquipType.Weapon:
                            go.transform.SetParent(BagWnd.WeaponBox.transform);
                            break;
                        case EquipType.Jacket:
                            go.transform.SetParent(BagWnd.JacketBox.transform);
                            break;
                        case EquipType.Trouser:
                            go.transform.SetParent(BagWnd.TrouserBox.transform);
                            break;
                        case EquipType.Necklace:
                            go.transform.SetParent(BagWnd.NecklaceBox.transform);
                            break;
                        case EquipType.Belt:
                            go.transform.SetParent(BagWnd.BeltBox.transform);
                            break;
                        case EquipType.Shoes:
                            go.transform.SetParent(BagWnd.ShoesBox.transform);
                            break;
                    }
                }

                go.transform.localPosition = Vector3.zero;
            }
            BagWnd.RefreshUI();
        }

        public void AddPack(int id,int goodsId,int count,int tarIndex)
        {
            EquipConfig equipConfig = CfgUtility.EquipConfigs[goodsId];
            GameObject go = Instantiate(Resources.Load<GameObject>("PrefasUI/GoodsItem"), BagWnd.packsTransform.GetChild(tarIndex), true);
            go.transform.localPosition=Vector3.zero;
            GoodsItem item = go.GetComponent<GoodsItem>();
            item.Init(count, equipConfig.AssetIcon);
            item.goodsData = equipConfig;
            item.GoodsType = (GoodsType)equipConfig.EquipType;
            item.goodsData.ID =id;
            item.goodsData.cfgId = goodsId;
            GoodsBag.Add(id, new Goods(item, count, tarIndex));
        }
        


        public void SetEquipToBox(Transform goods, Transform box)
        {
            GoodsItem goodsItem= goods.GetComponent<GoodsItem>();
            BoxItem goodsParentItem= goodsItem.orignalParent.GetComponent<BoxItem>();
            BoxItem boxItem= box.GetComponent<BoxItem>();
            EquipConfig equipConfig= CfgUtility.EquipConfigs[goodsItem.goodsData.cfgId];
            if (goodsParentItem.GoodsType==GoodsType.None&&boxItem.GoodsType!=GoodsType.None)
            {
                RefreshPropty(equipConfig.Prop);
               
            }else if (goodsParentItem.GoodsType!=GoodsType.None&&boxItem.GoodsType==GoodsType.None)
            {
                RefreshPropty(equipConfig.Prop,false);
            }
            goods.SetParent(box.transform);
            goods.GetComponent<GoodsItem>().orignalParent = box.transform;
            goods.localPosition = Vector3.zero;
        }

        public void RefreshPropty(string prop,bool isAdd=true)
        {
            string[] props = prop.Split("|");
            for (int i = 0; i < props.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        GameRoot.Instance.PlayerData.ad = isAdd?
                            GameRoot.Instance.PlayerData.ad+int.Parse(props[i]):
                            GameRoot.Instance.PlayerData.ad-int.Parse(props[i]);
                        break;
                    case 1:
                        GameRoot.Instance.PlayerData.ap = isAdd?
                            GameRoot.Instance.PlayerData.ap+int.Parse(props[i]):
                            GameRoot.Instance.PlayerData.ap-int.Parse(props[i]);
                        break;
                    case 2:
                        GameRoot.Instance.PlayerData.hp = isAdd?
                            GameRoot.Instance.PlayerData.hp+int.Parse(props[i]):
                            GameRoot.Instance.PlayerData.hp-int.Parse(props[i]);
                        break; 
                    case 3:
                        GameRoot.Instance.PlayerData.dodge = isAdd?
                            GameRoot.Instance.PlayerData.dodge+int.Parse(props[i]):
                            GameRoot.Instance.PlayerData.dodge-int.Parse(props[i]);
                        break;
                    case 4:
                        GameRoot.Instance.PlayerData.ad = isAdd?
                            GameRoot.Instance.PlayerData.addef+int.Parse(props[i]):
                            GameRoot.Instance.PlayerData.addef-int.Parse(props[i]);
                        break;
                    case 5:
                        GameRoot.Instance.PlayerData.critical = isAdd?
                            GameRoot.Instance.PlayerData.critical+int.Parse(props[i]):
                            GameRoot.Instance.PlayerData.critical-int.Parse(props[i]);
                        break;
                }
            }
            _netService.SendMsg(new NetMsg()
            {
                cmd =(int) TransCode.UpdateUserPropReq,
                updateUserPropReq =  new UpdateUserPropReq()
                {
                    userid = GameRoot.Instance.PlayerData.id,
                    ad = GameRoot.Instance.PlayerData.ad,
                    ap = GameRoot.Instance.PlayerData.ap,
                    addef = GameRoot.Instance.PlayerData.addef,
                    hp = GameRoot.Instance.PlayerData.hp,
                    critical = GameRoot.Instance.PlayerData.critical,
                    dodge = GameRoot.Instance.PlayerData.dodge,
                }
            });
            GameRoot.Instance.AddTips("战力: "+NetCommon.GetFight(GameRoot.Instance.PlayerData));
            BagWnd.RefreshUI();
            MainCitySys.Instance.RefreshUI();
        }
    }
}