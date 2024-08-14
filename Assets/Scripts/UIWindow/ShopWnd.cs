using System;
using System.Collections;
using System.Collections.Generic;
using DarkGod.System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

public class ShopItem
{
   public enum ShopType
   {
      Bag=1,
      Crystal=2,
      Exp=3,
      Coin=4,
     
     
      
   }

   public ShopType shopType;
   public int id;
   public string icon;
   public int cost;
   public int count;
}
public class ShopWnd : MonoBehaviour
{
   

   private List<ShopItem> _shopItems;
   [SerializeField] private GameObject shopItemTemplate;
   [SerializeField] private RectTransform _transform;
   

   private void Start()
   {
      _shopItems = ResService.Instance.GetShops();
      for (int i = 0; i < _shopItems.Count; i++)
      {
         GameObject item = Instantiate(shopItemTemplate, _transform);
         item.transform.Find("Much").GetComponent<TMP_Text>().text = _shopItems[i].cost.ToString();
         item.transform.Find("Prop").GetComponent<Image>().sprite = ResService.Instance.GetSprite(_shopItems[i].icon);
         item.SetActive(true);
         item.AddComponent<Button>().onClick.AddListener(OnClickItem(i));
      }
   }
   public UnityAction OnClickItem(int i)
   {
      return () =>
      {
         BuyItem(i);
      };
   }
   public void BuyItem(int i)
   {
      if (_shopItems[i].cost > GameRoot.Instance.PlayerData.diamond)
      {
         GameRoot.Instance.AddTips("钻石不足");
         return;
      }
      MainCitySys.Instance.BuyShopItemReq(_shopItems[i]);
   }

   public void OnClickClose()
   {
      MainCitySys.Instance.CloseShopUI();
   }
}
