/****************************************************
    文件：GameRoot.cs
	作者：WWC
    邮箱: 1469359779@qq.com
    日期：2023/4/22 10:5:53
	功能：负责模块与系统的初始化
*****************************************************/

using System;
using Common;
using DarkGod.System;
using DarkGod.UIWindow;
using Protocal;
using Service;
using UnityEngine;
using Wwc.Cfg;
using Color = Common.Color;

public class GameRoot : MonoBehaviour
{
	private static GameRoot instance;
	public LoadingWnd loadingWnd;
	public DynamicWnd dynamicWnd;
	private PlayerData _playerData;
	

	public Transform Canvas;

	public Action updates;

	public static GameRoot Instance
	{
		get
		{
			return instance;
		}
	}

	public PlayerData PlayerData
	{
		get
		{
			return _playerData;
		}
	}

	private void Awake()
	{
		CfgUtility.SetUp();
		instance = this;
		DontDestroyOnLoad(this);
		NetCommon.Log("GameRoot Init");
		Init();
	}

	private void Init()
	{
		//Init Service
		NetService netService = GetComponent<NetService>();
		netService.Init();
		ResService resService = GetComponent<ResService>();
		resService.Init();
		AudioService audioService = GetComponent<AudioService>();
		audioService.Init();
		TimerService timerService = GetComponent<TimerService>();
		timerService.Init();


		//Init System
		LoginSys loginSys = GetComponent<LoginSys>();
		loginSys.InitSystem();
		MainCitySys mainCitySys = GetComponent<MainCitySys>();
		mainCitySys.InitSystem();
		LevelSystem levelSystem = GetComponent<LevelSystem>();
		levelSystem.InitSystem();
		BattleSystem battleSystem = GetComponent<BattleSystem>();
		battleSystem.InitSystem();
		InventorySystem inventory = GetComponent<InventorySystem>();
		inventory.InitSystem();
		
		ClearUI();
		
		//Loading Login Panel
		Canvas = transform.Find("Canvas");
		loginSys.EnterLogin();
	}

	private void ClearUI()
	{
		Transform cav= transform.Find("Canvas");
		for (int i = 0; i < cav.childCount; i++)
		{
			cav.GetChild(i).gameObject.SetActive(false);
		}
		dynamicWnd.SetWndState();
	}

	public void AddTips(string tips,Color color=Color.Yellow)
	{
		string res = "";
		res = GetColorText(tips, color);
		dynamicWnd.AddTips(res);
	}

	public string GetColorText(string msg,Color color)
	{
		string res = "";
		switch (color)
		{
			case Color.White:
				res += Constant.whiteColor + msg + Constant.colorEnd;
				break;
			case Color.Black:
				res += Constant.blackColor + msg + Constant.colorEnd;
				break;
			case Color.Blue:
				res += Constant.blueColor + msg + Constant.colorEnd;
				break;
			case Color.Yellow:
				res += Constant.yellowColor + msg + Constant.colorEnd;
				break;
			case Color.Red:
				res += Constant.redColor + msg + Constant.colorEnd;
				break;
		}

		return res;
	}

	public void SetPlayerData(PlayerData playerData)
	{
		this._playerData = playerData;
	}

	public void SetPlayerDataByTask(TaskRes res)
	{
		_playerData.lv = res.lv;
		_playerData.coin = res.coin;
		_playerData.exp = res.exp;
		_playerData.guideid = res.guideid;
	}

	public void EnterLevelPass()
	{
		LevelSystem.Instance.OnEnterLevelPass();
	}

	public void SetItemEntityHp(string key,int hp,Transform hpTrans)
	{
		dynamicWnd.SetItemEntityHp(key,hp,hpTrans);
	}

	public void Update()
	{
		updates?.Invoke();
	}
}



