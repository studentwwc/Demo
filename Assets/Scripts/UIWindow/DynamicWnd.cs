/****************************************************
    文件：DynamicWnd.cs
	作者：WWC
    邮箱: 1469359779@qq.com
    日期：2023/4/23 15:8:35
	功能：Tips加载
*****************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DarkGod.UIWindow;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DynamicWnd : WindowRoot
{
	[SerializeField] private Animation tipsAnim;
	[SerializeField] private TMP_Text text;
	[SerializeField] private Animation selfDodgeAnim;
	private Queue<string> tipsQue;
	private bool isShowing;

	[SerializeField] private Transform itemHpRoot;
	private Dictionary<string, ItemEntityHp> _itemEntityHpDic = new Dictionary<string, ItemEntityHp>();

	protected override void InitWnd()
	{
		base.InitWnd();
		SetActive(text,false);
		tipsQue = new Queue<string>();
	}

	public void AddTips(string tips)
	{
		lock (tipsQue)
		{
			tipsQue.Enqueue(tips);
		}
	}

	//设置Tips
	private void SetTips(string tips)
	{
		SetActive(text);
		SetText(text,tips);
		if (tipsAnim != null)
		{
			tipsAnim.Play();
		}
		StartCoroutine(PlayAnimDone(tipsAnim.GetClip(("TipsAnim")).length, () =>
		{
			SetActive(text,false);
			isShowing = false;
		}));
	}

	private IEnumerator PlayAnimDone(float time,Action ac)
	{
		yield return new WaitForSeconds(time);
		ac();
	}

	private void Update()
	{
		if (tipsQue.Count > 0 && !isShowing)
		{
			lock (tipsQue)
			{
				string tips= tipsQue.Dequeue();
				isShowing = true;
				SetTips(tips);
			}
		}
	}

	public void SetItemEntityHp(string name,int hp,Transform hpTransform)
	{
		if (_itemEntityHpDic.ContainsKey(name))
		{
			return;
		}

		if (_resService == null)
		{
			SetWndState();
		}

		GameObject item = _resService.GetGameObject(Constant.ItemEntityHpPath);
		ItemEntityHp itemEntityHp = item.GetComponent<ItemEntityHp>();
		itemEntityHp.transform.localPosition = new Vector3(-1000, 0, 0);
		itemEntityHp.Init(hp,hpTransform);
		item.transform.SetParent(itemHpRoot);
		_itemEntityHpDic.Add(name,itemEntityHp);
	}

	public void SetCritical(string name,int cri)
	{
		if (_itemEntityHpDic.ContainsKey(name))
		{
			_itemEntityHpDic[name].SetCritical(cri);
		}
	}
	public void SetDamage(string name,int dmg)
	{
		if (_itemEntityHpDic.ContainsKey(name))
		{
			_itemEntityHpDic[name].SetDamage(dmg);
		}
	}
	public void SetDodge(string name)
	{
		if (_itemEntityHpDic.ContainsKey(name))
		{
			_itemEntityHpDic[name].SetDodge();
		}
	}

	public void SetSelfDodge()
	{
		if (selfDodgeAnim != null)
		{
			selfDodgeAnim.Stop();
			selfDodgeAnim.Play();
		}
	}

	public void SetHpChange(string name, int oldHp, int newHp)
	{
		if (_itemEntityHpDic.ContainsKey(name))
		{
			_itemEntityHpDic[name].SetHpChange(oldHp,newHp);
		}
	}

	public void ItemEntityHpInvalid(string name)
	{
		if (_itemEntityHpDic.ContainsKey(name))
		{
			_itemEntityHpDic[name].gameObject.SetActive(false);
		}
	}

	public void Clear()
	{
		foreach (var VARIABLE in _itemEntityHpDic)
		{
			GameObject.Destroy(VARIABLE.Value.gameObject);
		}
		_itemEntityHpDic.Clear();
		
	}
}



