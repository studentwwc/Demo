using Common;
using Config;
using DarkGod.System;
using Protocal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wwc.Cfg;

namespace DarkGod.UIWindow
{
    public class GuideWnd:WindowRoot
    {
        [SerializeField] private TMP_Text txtName;
        [SerializeField] private TMP_Text txtDialog;
        [SerializeField] private Image imgIcon;
        private PlayerData pd;
        private AutoGuideConfig _autoGuideConfig;
        private string[] dialogs;
        private int indexDialog;
        protected override void InitWnd()
        {
            base.InitWnd();
            pd = GameRoot.Instance.PlayerData;
            _autoGuideConfig = MainCitySys.Instance.GetCurrentAutoTaskConfig();
            dialogs = _autoGuideConfig.dilogArr.Split("#");
            indexDialog = 1;
            SetTalk();
        }

        private void SetTalk()
        {
            string []dialogInfo=dialogs[indexDialog].Split("|");
            if (dialogInfo[0].Equals("0"))//self
            {
                SetText(txtName,pd.name);
                SetText(txtDialog,dialogInfo[1].Replace("$name",pd.name));
                SetSprite(imgIcon,Constant.selfIconPath);
            }
            else
            {
                switch (_autoGuideConfig.npcID)
                {
                    case Constant.npcWiseMan:
                        SetText(txtName,"智者");
                        SetText(txtDialog,dialogInfo[1]);
                        SetSprite(imgIcon,Constant.wiseManIconPath);
                        break;
                    case Constant.npcGeneral:
                        SetText(txtName,"将军");
                        SetText(txtDialog,dialogInfo[1]);
                        SetSprite(imgIcon,Constant.generalIconPath);
                        break;
                    case Constant.npcArtisan:
                        SetText(txtName,"工匠");
                        SetText(txtDialog,dialogInfo[1]);
                        SetSprite(imgIcon,Constant.artisanIconPath);
                        break;
                    case Constant.npcTrader:
                        SetText(txtName,"商人");
                        SetText(txtDialog,dialogInfo[1]);
                        SetSprite(imgIcon,Constant.traderIconPath);
                        break;
                    default:
                        SetText(txtName,"导师");
                        SetText(txtDialog,dialogInfo[1]);
                        SetSprite(imgIcon,Constant.defaultIconPath);
                        break;
                }
            }

            imgIcon.SetNativeSize();
        }

        public void SetNextDialog()
        {
            indexDialog++;
            if (indexDialog == dialogs.Length)
            {
                SetWndState(false);
                //TODO:完成奖励
                // _netService.SendMsg(new NetMsg()
                // {
                //     cmd = (int)TransCode.TaskReq,
                //     taskReq = new TaskReq()
                //     {
                //         guideid = _autoGuideConfig.ID
                //     }
                // });
                MainCitySys.Instance.TaskReq(_autoGuideConfig.ID);
            }
            else
            {
                SetTalk();
            }
        }
    }
}