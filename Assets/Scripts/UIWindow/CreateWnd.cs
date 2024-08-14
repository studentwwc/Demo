/****************************************************
    文件：CreateWnd.cs
	作者：WWC
    邮箱: 1469359779@qq.com
    日期：2023/4/23 17:20:52
	功能：创建角色
*****************************************************/


using System.Collections.Generic;
using Common;
using Config;
using DarkGod.System;
using Protocal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Wwc.Cfg;

public class CreateWnd : WindowRoot
{
	[SerializeField] private TMP_InputField ipfPlayerName;
	[SerializeField] private Button btnRadomName;
	[SerializeField] private Button btnEenter;
	public Button preview;
	public Button next;
	public Image roleIcon;
	public TMP_Text roleDes;
	public TMP_Text roleName;
	private int index=0;
	private List<RoleData> _roleDatas=new List<RoleData>();
	protected override void InitWnd()
	{
		base.InitWnd();
		OnRandomNameClick();
		btnRadomName.onClick.AddListener(OnRandomNameClick);
		btnEenter.onClick.AddListener(OnEnterClick);
		next.onClick.AddListener(Next);
		preview.onClick.AddListener(Preview);
	}

	public void InitRoleDataInfo()
	{
		foreach (var roleConfig in CfgUtility.RoleConfigs)
		{
			Debug.Log(roleConfig.Value.IconAsset);
			_roleDatas.Add(new RoleData(Resources.Load<Sprite>(roleConfig.Value.DesIconAsset),roleConfig.Value));
		}
	}

	public void OnRandomNameClick()
	{
		_audioService.PlayUIAudio(Constant.AudioPathPrefix+Constant.AudioUIClickBtn);
		ipfPlayerName.text = _resService.GetRandomName();
	}

	public void OnEnterClick()
	{
		_audioService.PlayUIAudio(Constant.AudioPathPrefix+Constant.AudioUIClickBtn);
		if (string.IsNullOrEmpty(ipfPlayerName.text))
		{
			GameRoot.Instance.AddTips("名字不能为空");
			return;
		}
		LoginSys.Instance.RenameReq(_roleDatas[index].RoleConfig.Prop,_roleDatas[index].RoleConfig.ID,ipfPlayerName.text);
	}

	public void Next()
	{
		index = (index + 1) % _roleDatas.Count;
		RefreshData();
	}

	public void Preview()
	{
		if (index == 0)
		{
			index= _roleDatas.Count - 1;
		}
		else
		{
			index = (index - 1) % _roleDatas.Count;
		}
        RefreshData();
		
	}

	public void RefreshData()
	{
		roleIcon.sprite = _roleDatas[index].Sprite;
		roleDes.text = _roleDatas[index].RoleConfig.Des;
		roleName.text = _roleDatas[index].RoleConfig.Name;
	}
}



