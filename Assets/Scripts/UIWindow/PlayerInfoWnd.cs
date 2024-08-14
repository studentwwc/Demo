/****************************************************
    文件：PlayerInfoWnd.cs
	作者：WWC
    邮箱: 1469359779@qq.com
    日期：2023/5/5 21:43:17
	功能：Nothing
*****************************************************/


using Common;
using DarkGod.System;
using Protocal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoWnd : WindowRoot
{
	[SerializeField] private RawImage charRender;
	[SerializeField] private TMP_Text txtNickAndLv;
	[SerializeField] private TMP_Text txtExp;
	[SerializeField] private Image imgExp;
	[SerializeField] private TMP_Text txtPower;
	[SerializeField] private Image imgPower;
	[SerializeField] private TMP_Text txtProfession;
	[SerializeField] private TMP_Text txtFight;
	[SerializeField] private TMP_Text txtHp;
	[SerializeField] private TMP_Text txtDamage;
	[SerializeField] private TMP_Text txtDefense;
	
	//detailProp
	[SerializeField] private Transform detailTrans;
	[SerializeField] private TMP_Text deTxtHp;
	[SerializeField] private TMP_Text deTxtAd;
	[SerializeField] private TMP_Text deTxtAp;
	[SerializeField] private TMP_Text deTxtAdDef;
	[SerializeField] private TMP_Text deTxtApDef;
	[SerializeField] private TMP_Text deTxtAvoid;
	[SerializeField] private TMP_Text deTxtPierce;
	[SerializeField] private TMP_Text deTxtCritical;

	
	
	//人物旋转相关参数
	private Vector2 startPos;
	protected override void InitWnd()
	{
		base.InitWnd();
		RefreshUI();
		RegisterTouch();
	}

	private void RefreshUI()
	{
		PlayerData pd= GameRoot.Instance.PlayerData;
		SetText(txtNickAndLv,pd.name + " Lv." + pd.lv);
		SetText(txtExp,pd.exp+"/"+NetCommon.GetExpLimit(pd.lv));
		imgExp.fillAmount = pd.exp * 1.0f / NetCommon.GetExpLimit(pd.lv);
		SetText(txtPower,pd.power+"/"+NetCommon.GetPowerLimit(pd.lv));
		imgPower.fillAmount = pd.power * 1.0f / NetCommon.GetPowerLimit(pd.lv);
		SetText(txtProfession,"暗夜刺客");
		SetText(txtFight,NetCommon.GetFight(pd));
		SetText(txtHp,pd.hp);
		SetText(txtDamage,pd.ad+pd.ap);
		SetText(txtDefense,pd.apdef+pd.addef);
		detailTrans.gameObject.SetActive(false);
	}
	private void RefreshDetail()
	{
		PlayerData pd = GameRoot.Instance.PlayerData;
		SetText(deTxtHp,pd.hp);
		SetText(deTxtAd,pd.ad);
		SetText(deTxtAp,pd.ap);
		SetText(deTxtAdDef,pd.addef);
		SetText(deTxtApDef,pd.apdef);
		SetText(deTxtAvoid,pd.dodge+"%");
		SetText(deTxtPierce,pd.pierce+"%");
		SetText(deTxtCritical,pd.critical+"%");
	}

	private void RegisterTouch()
	{
		OnClickDownPEListener(charRender.gameObject, pd =>
		{
			startPos = pd.position;
			MainCitySys.Instance.charCamAngleWithPlayer = 0;
		});
		OnDragPEListener(charRender.gameObject, pd =>
		{
			float angle = (pd.position.x - startPos.x)*0.1f;
			MainCitySys.Instance.CharCamAroundPlayerRotate(angle);
		});
	}

	public void ClosePage()
	{
		MainCitySys.Instance.ClosePlayerInfoPage();
	}

	public void OpenDetail()
	{
		_audioService.PlayUIAudio(Constant.AudioPathPrefix+Constant.AudioUIClickBtn);
		detailTrans.gameObject.SetActive(true);
		RefreshDetail();
	}

	public void CloseDetail()
	{
		_audioService.PlayUIAudio(Constant.AudioPathPrefix+Constant.AudioUIClickBtn);
		detailTrans.gameObject.SetActive(false);
	}

}



