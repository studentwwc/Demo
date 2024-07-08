using System.Collections.Generic;
using DarkGod.Bag;
using DarkGod.System;
using UnityEngine;
using Utility;

namespace DarkGod
{
    public class BoxItem:BagItem
    {
        public int index;
        public override void Receive(GoodsItem item)
        {
            BoxItem originalBox= item.orignalParent.GetComponent<BoxItem>();
            //背包栏Box
            if (GoodsType == GoodsType.None)
            {
                //来自背包栏中的物品
                GameDataUtility.ChangeIndex(item.goodsData.ID,index);
                InventorySystem.Instance.SetEquipToBox(item.transform,transform);
            }
            else   //装备栏的Box
            {   //装备栏与物品的类型不匹配
                if (item.GoodsType != GoodsType)
                {
                    InventorySystem.Instance.SetEquipToBox(item.transform,originalBox.transform);
                }
                else//类型匹配
                {
                    GameDataUtility.ChangeIndex(item.goodsData.ID,-1);
                    InventorySystem.Instance.SetEquipToBox(item.transform,transform);
                }
            }

        }
    }
}