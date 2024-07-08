/****************************************************
    文件：MainCityWnd.cs
	作者：WWC
    邮箱: 1469359779@qq.com
    日期：2023/4/27 21:54:49
	功能：主城界面
*****************************************************/


using System;
using Common;
using Config;
using DarkGod.System;
using Map;
using Protocal;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;
using Wwc.Cfg;

public class MainCityWnd : WindowRoot
{
    [SerializeField] private TMP_Text txtNick;
    [SerializeField] private TMP_Text txtLv;
    [SerializeField] private TMP_Text txtFight;
    [SerializeField] private TMP_Text txtPower;
    [SerializeField] private Image imgPower;
    [SerializeField] private TMP_Text txtExp;
    [SerializeField] private Text txtDiamond;
    [SerializeField] private Text txtCoin;
    [SerializeField] private Text txtCrystal;
    
    [SerializeField] private GridLayoutGroup _gridLayout;
    [SerializeField] private Animation aniMenu;

    [SerializeField] private Image imgTouch;
    [SerializeField] private Image imgDirBg;
    [SerializeField] private Image imgDirPointer;
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image imgAutoGuide;
 
    private bool menuState = true;
    private Vector2 defaultImgDirBgPostion=Vector2.zero;
    private Vector2 startPointerPos=Vector2.zero;
    private int oprationDistance = 0;

    private AutoGuideConfig currentAutoTask;

    public PlayerController playerCt;

   
    protected override void InitWnd()
    {
        base.InitWnd();
        SetActive(imgDirPointer,false);
        RegisterImgTouch();
        RefreshUI();
        defaultImgDirBgPostion = imgDirBg.transform.position;
        oprationDistance = (int)( Constant.ScreenOpDis*1.0f*Screen.height / Constant.ScreenStandardHeight);
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtNick, pd.name);
        SetText(txtLv, pd.lv);
        SetText(txtPower, "体力:" + pd.power + "/" + NetCommon.GetPowerLimit(pd.lv));
        SetText(txtFight, NetCommon.GetFight(pd));
        imgPower.fillAmount = pd.power*1.0f / NetCommon.GetPowerLimit(pd.lv);
        // txtExp.text = Math.Round(pd.exp * 1.0 / NetCommon.GetExpLimit(pd.lv), 2) * 100 + "%";\
        int exp =(int) (Math.Round(pd.exp * 1.0 / NetCommon.GetExpLimit(pd.lv), 2) * 100);
        SetText(txtExp,exp + "%");
        SetText(txtCoin, pd.coin);
        SetText(txtDiamond, pd.diamond);
        SetText(txtCrystal, pd.crystal);

        #region Exp

        //经验条自适应
        float ratio = Screen.height * 1.0f / Constant.ScreenStandardHeight;
        float screenWidth = Constant.ScreenStandardWidth / ratio;
        float expCell = (screenWidth - 180) / 10.0f;
        _gridLayout.cellSize = new Vector2(expCell, 12);
        //设置经验条的值
        for (int i = 0; i < _gridLayout.transform.childCount; i++)
        {
            if (i < exp / 10)
            {
                _gridLayout.transform.GetChild(i).GetComponent<Image>().fillAmount = 1;
            }else if (i == exp / 10)
            {
                _gridLayout.transform.GetChild(i).GetComponent<Image>().fillAmount = (exp%10)/10.0f;
            }
            else
            {
                _gridLayout.transform.GetChild(i).GetComponent<Image>().fillAmount = 0;
            }
        }

        #endregion
        
        //设置自动导航的图标
        currentAutoTask = _resService.GetAutoGuideConfig(pd.guideid);

        string path = "";
        if (currentAutoTask != null)
        {
            switch (currentAutoTask.npcID)
            {
                case Constant.npcWiseMan:
                    path = Constant.wiseManHeadPath;
                    break;
                case Constant.npcGeneral:
                    path = Constant.generalHeadPath;
                    break;
                case Constant.npcArtisan:
                    path = Constant.artisanHeadPath;
                    break;
                case Constant.npcTrader:
                    path = Constant.traderHeadPath;
                    break;
                default:
                    path = Constant.taskHeadPath;
                    break;
            }
        }
        else
        {
            path = Constant.taskHeadPath;
        }
        SetSprite(imgAutoGuide,path);
        RoleConfig roleConfig;
        CfgUtility.RoleConfigs.TryGetValue(GameDataUtility.roleid,out roleConfig);
        if (roleConfig != null)
        {
            SetSprite(playerIcon,roleConfig.IconAsset);
        }
    }
    

    #region ClickEvents

    public void OnClickMenu()
    {
        menuState = !menuState;
        _audioService.PlayUIAudio(Constant.AudioUIExtenBtn);
        aniMenu.clip = null;
        AnimationClip clipState = null;
        if (menuState)
        {
            clipState = aniMenu.GetClip("MainCityOpenSkill");
        }
        else
        {
            clipState = aniMenu.GetClip("MainCityCloseSkill");
        }

        aniMenu.clip = clipState;
        aniMenu.Play();
    }

    public void OnClickAutoTask()
    {
        _audioService.PlayUIAudio(Constant.AudioPathPrefix+Constant.AudioUIClickBtn);
        if (currentAutoTask != null)
        { 
            MainCitySys.Instance.RunAutoTask(currentAutoTask);
        }
        else
        {
            GameRoot.Instance.AddTips("所有任务已完成");
        }
    }
    public void OnClickStrong()
    {
       MainCitySys.Instance.ShowUIStrong();
        
    }
    public void OnClickHead()
    {
        MainCitySys.Instance.ShowUIPlayerInfo();
    }

    public void OnClickChat()
    {
        MainCitySys.Instance.OpenChatWnd();
    }

    public void OnClickMKCoin()
    {
        MainCitySys.Instance.OpenBuyWnd(0);
    }

    public void OnClickProPower()
    {
        MainCitySys.Instance.OpenBuyWnd(1);
    }

    public void OnClickReWard()
    {
        MainCitySys.Instance.ShowUITaskReward();
    }

    public void OnClickLevelPass()
    {
        MainCitySys.Instance.EnterLeverlPass();
    }

    public void OnClickBagPack()
    {
        MainCitySys.Instance.ShowBagPackUI();
    }

    public void OnClickShop()
    {
        MainCitySys.Instance.ShowShopUI();
    }

    public void RegisterImgTouch()
    {
       OnClickDownPEListener(imgTouch.gameObject, (PointerEventData ped) =>
       {
           imgDirBg.transform.position = ped.position;
           SetActive(imgDirPointer);
           startPointerPos = ped.position;
       });
       OnClickUpPEListener(imgTouch.gameObject, (ped) =>
       {
           SetActive(imgDirPointer,false);
           imgDirBg.transform.position = defaultImgDirBgPostion;
           imgDirPointer.transform.position=Vector3.zero;
           
           // playerCt.Dir=Vector2.zero;
           // playerCt.SetBlend(Constant.playerIdleBlend);
           MainCitySys.Instance.SetDir(Vector2.zero);
       });
       OnDragPEListener(imgTouch.gameObject, (ped) =>
       {
           Vector2 dir = ped.position - startPointerPos;
           if (dir.magnitude < oprationDistance)
           {
               imgDirPointer.transform.position = ped.position;
           }
           else
           {
               Vector2 tar= Vector2.ClampMagnitude(dir,oprationDistance);
               imgDirPointer.transform.position = startPointerPos+tar;
           }
           MainCitySys.Instance.SetDir(dir);
       });
    }

    #endregion

}