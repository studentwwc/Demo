/****************************************************
    文件：LoginSys.cs
	作者：WWC
    邮箱: 1469359779@qq.com
    日期：2023/4/22 10:5:0
	功能：Login System
*****************************************************/


using Common;
using DarkGod.UIWindow;
using Protocal;
using UnityEngine;
using Utility;

namespace DarkGod.System
{
	public class LoginSys : SystemRoot<LoginSys>
	{
		public LoginWnd loginWnd;
		public CreateWnd createWnd;
		public override void InitSystem()
		{
			base.InitSystem();
			Debug.Log("Login System Init");
		}

		public void EnterLogin()
		{
			Debug.Log("Enter Login Panel");

			resService.ASyncLoadScene(Constant.SceneLogin, () =>
			{
				loginWnd.SetWndState();
			});//异步加载场景
			
		}

		public void LoginResp(NetMsg netMsg)
		{
			GameRoot.Instance.AddTips("登录成功");
			GameDataUtility.roleid = netMsg.loginRes.roleid;
			
			if (netMsg.loginRes != null)
			{
				GameRoot.Instance.SetPlayerData(netMsg.loginRes.playerData);
			}
			//获取游戏资源
			GameDataUtility.SetUp();
			
			
			//关闭登陆界面
			loginWnd.SetWndState(false);
			
			if (string.IsNullOrEmpty(netMsg.loginRes.playerData.name))
			{
				//打开角色创建界面
				createWnd.InitRoleDataInfo();
				createWnd.SetWndState();
			}else{
				createWnd.SetWndState(false);
				//GameRoot.Instance.AddTips("进入主城");
 			    MainCitySys.Instance.EnterMainCity();
			}
		}

		public void RenameReq(string roleProp,int roleid,string ipfPlayerName)
		{
			string prop = roleProp;
			string[]props=prop.Split('|');
			GameRoot.Instance.PlayerData.ad = int.Parse(props[0]);
			GameRoot.Instance.PlayerData.ap = int.Parse(props[1]);
			GameRoot.Instance.PlayerData.hp = int.Parse(props[2]);
			GameRoot.Instance.PlayerData.dodge = int.Parse(props[3]);
			GameRoot.Instance.PlayerData.addef = int.Parse(props[4]);
			GameRoot.Instance.PlayerData.critical = int.Parse(props[5]);
			GameRoot.Instance.PlayerData.name= ipfPlayerName;
			GameDataUtility.roleid =roleid;
			_netService.SendMsg(new NetMsg()
			{
				cmd =(int)TransCode.RenameReq,
				renameReq = new RenameReq()
				{
					name = ipfPlayerName,
					prop = prop,
					roleid = roleid,
				}
			});
		}

		public void RenameResp(NetMsg netMsg)
		{
			createWnd.SetWndState(false);
			//GameRoot.Instance.AddTips("进入主城");
			MainCitySys.Instance.EnterMainCity();
			
		}

		public void LoginReq(string account,string password)
		{
			PlayerPrefs.SetString("Account",account);
			PlayerPrefs.SetString("Password",password);
			_netService.SendMsg(new NetMsg()
			{
				loginReq=new LoginReq()
				{
					account =account,
					password =password
				},
				cmd = (int)TransCode.LoginReq
                    
			});
		}
	}
}



