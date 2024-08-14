/****************************************************
    文件：GoodsItem.cs
	作者：WWC
    邮箱: 1469359779@qq.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using Config;
using DarkGod;
using DarkGod.Bag;
using DarkGod.System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GoodsItem : BagItem,IBeginDragHandler,IDragHandler,IEndDragHandler
{
	private Image img;
	[HideInInspector] public Transform orignalParent;
	public GoodsData goodsData;
	private TMP_Text count_Text;

	public void Init(int count,string iconPath)
	{
		img = GetComponent<Image>();
		count_Text = transform.GetComponentInChildren<TMP_Text>();
		count_Text.text= count < 2 ? "" : count.ToString();
        SetSprite(iconPath);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		orignalParent = transform.parent;
		transform.SetParent(GameRoot.Instance.Canvas);
		img.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		img.raycastTarget = true;
		BagItem item= eventData.pointerEnter.GetComponent<BagItem>();
		if (item == null)
		{
			InventorySystem.Instance.SetEquipToBox(transform,orignalParent);
		}else if (item as BoxItem)
		{
			((BoxItem)item).Receive(this);
		}
		else if(item as GoodsItem)
		{
			((GoodsItem)item).Receive(this);
		}
	}

	public override void Receive(GoodsItem item)
	{

		BoxItem selfBoxItem = transform.parent.GetComponent<BoxItem>();
		BoxItem other = item.orignalParent.GetComponent<BoxItem>();
		if (selfBoxItem.GoodsType != other.GoodsType)//装备栏的物品与背包栏的物品对换，但类型不匹配
		{
			InventorySystem.Instance.SetEquipToBox(item.transform,other.transform);
		}else if (selfBoxItem.GoodsType == other.GoodsType && selfBoxItem.GoodsType == GoodsType.None)//背包栏与背包栏对换
		{
			InventorySystem.Instance.SetEquipToBox(item.transform,selfBoxItem.transform);
			InventorySystem.Instance.SetEquipToBox(transform,other.transform);
		}else if (selfBoxItem.GoodsType == other.GoodsType && selfBoxItem.GoodsType != GoodsType.None)//装备栏与背包栏对换
		{
			InventorySystem.Instance.SetEquipToBox(item.transform,selfBoxItem.transform);
			InventorySystem.Instance.SetEquipToBox(transform,other.transform);
		}

		// Transform tempParent = item.orignalParent;
		// InventorySystem.Instance.SetEquipToBox(item.transform,orignalParent);
		// InventorySystem.Instance.SetEquipToBox(transform,tempParent);
	}
	public void SetSprite(string path)
	{
		img.sprite = Resources.Load<Sprite>(path);
	}
}



