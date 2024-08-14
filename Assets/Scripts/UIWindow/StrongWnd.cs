using System;
using Common;
using Config;
using DarkGod.System;
using Protocal;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Wwc.Cfg;

namespace DarkGod.UIWindow
{
    public class StrongWnd:WindowRoot
    {
        [SerializeField] private RectTransform equipItemParent;
        [SerializeField] private Image[]unSelectBg;
        [SerializeField] private Image[] selectedBg;

        #region ContentUi

        [SerializeField] private Image imgIcon;
        [SerializeField] private TMP_Text txtStartLv;
        [SerializeField] private RectTransform starsParent;
        [SerializeField] private TMP_Text txtAddHpPre;
        [SerializeField] private TMP_Text txtAddHurtPre;
        [SerializeField] private TMP_Text txtAddDefPre;
        
        [SerializeField] private TMP_Text txtAddHpAf;
        [SerializeField] private TMP_Text txtAddHurtAf;
        [SerializeField] private TMP_Text txtAddDefAf;

        [SerializeField] private RectTransform constranitInfoTrans;
        [SerializeField] private TMP_Text txtNeedLv;
        [SerializeField] private TMP_Text txtNeedCoin;
        [SerializeField] private TMP_Text txtNeedCrystal;

        [SerializeField] private TMP_Text txtCurrentCoin;

        private StrongConfig nextStartConfig;
        private PlayerData pd;

        #endregion
        
        
        public int currentClick=0;
        protected override void InitWnd()
        {
            base.InitWnd();
            RegisterClickEvent();
            InitImage();
            OnClickHandle(0);
        }
        //初始化装备列表
        private void InitImage()
        {
            for (int i = 0; i < selectedBg.Length; i++)
            {
                if (i != 0)
                {
                    selectedBg[i].enabled = false;
                }
                else
                {
                    selectedBg[i].enabled = true;
                }
            }
            for (int i = 0; i < selectedBg.Length; i++)
            {
                if (i != 0)
                {
                    unSelectBg[i].enabled = true;
                }
                else
                {
                    unSelectBg[i].enabled = false;
                }
            }
        }
        //点击图片的回调函数
        private void OnClickHandle(object i)
        {
            int index = (int)i;
            if (index != currentClick)
            {
                selectedBg[index].enabled = true;
                unSelectBg[index].enabled = false;
                if (currentClick >= 0)
                {
                    unSelectBg[currentClick].enabled = true;
                    selectedBg[currentClick].enabled = false;
                }
                currentClick = index;
            }
            SetContent(index);
           
        }

        public void SetContent(int i)
        {
            pd=GameRoot.Instance.PlayerData;
            //StrongConfig strongConfig = _resService.GetStrongConfig(i, pd.strongArr[i]);
            nextStartConfig=_resService.GetStrongConfig(i, pd.strongArr[i]+1);
            int startLv = pd.strongArr[i];
            //设置Icon
            switch (i)
            {
                case 0:
                    SetSprite(imgIcon,Constant.equipHeadPath);
                    break;
                case 1:
                    SetSprite(imgIcon,Constant.equipBodyPath);
                    break;
                case 2:
                    SetSprite(imgIcon,Constant.equipWaistPath);
                    break;
                case 3:
                    SetSprite(imgIcon,Constant.equipHandPath);
                    break;
                case 4:
                    SetSprite(imgIcon,Constant.equipLegPath);
                    break;
                case 5:
                    SetSprite(imgIcon,Constant.equipFootPath);
                    break;
                    
            }
            //设置TXT星级
            switch (startLv)
            {
                case 0:
                    SetText(txtStartLv,"0星级");
                    break;
                case 1:
                    SetText(txtStartLv,"一星级");
                    break;
                case 2:
                    SetText(txtStartLv,"二星级");
                    break;
                case 3:
                    SetText(txtStartLv,"三星级");
                    break;
                case 4:
                    SetText(txtStartLv,"四星级");
                    break;
                case 5:
                    SetText(txtStartLv,"五星级");
                    break;
                case 6:
                    SetText(txtStartLv,"六星级");
                    break;
                case 7:
                    SetText(txtStartLv,"七星级");
                    break;
                case 8:
                    SetText(txtStartLv,"八星级");
                    break;
                case 9:
                    SetText(txtStartLv,"九星级");
                    break;
                case 10:
                    SetText(txtStartLv,"十星级");
                    break;
                    
            }
            //设置StartImg
              for (int n = 0; n < starsParent.childCount; n++)
              {
                  Image img = starsParent.GetChild(n).GetComponent<Image>();
                  if (n < pd.strongArr[i])
                  {
                      SetSprite(img,Constant.selectStartPath);
                  }
                  else
                  {
                      SetSprite(img,Constant.unSelectStartPath);
                  }
              }
            
            int preHp = _resService.EquipAccmulation(i, pd.strongArr[i],1);
            int preHurt = _resService.EquipAccmulation(i, pd.strongArr[i],2);
            int preDef = _resService.EquipAccmulation(i, pd.strongArr[i],3);
            SetText(txtAddHpPre,"生命 +"+preHp);
            SetText(txtAddHurtPre,"伤害 +"+preHurt);
            SetText(txtAddDefPre,"防御 +"+preDef);
            if (nextStartConfig != null)
            {
                SetActive(constranitInfoTrans);
                SetActive(txtAddHpAf);
                SetActive(txtAddHurtAf);
                SetActive(txtAddDefAf);
                int afHp = nextStartConfig.addhp;
                int afHurt =nextStartConfig.addhurt;
                int afDef = nextStartConfig.adddef;
                SetText(txtAddHpAf,"强化后 +"+afHp);
                SetText(txtAddHurtAf,"强化后 +"+afHurt);
                SetText(txtAddDefAf,"强化后 +"+afDef);
                SetText(txtNeedLv,nextStartConfig.minlv);
                SetText(txtNeedCoin,nextStartConfig.coin);
                SetText(txtNeedCrystal,pd.crystal+"/"+nextStartConfig.crystal);
            }
            else
            {
                SetActive(constranitInfoTrans,false);
                SetActive(txtAddHpAf,false);
                SetActive(txtAddHurtAf,false);
                SetActive(txtAddDefAf,false);
            }

            //升级要求
            
            
            //当前金币
            SetText(txtCurrentCoin,pd.coin);
        }

        #region ClickEvents

        public void OnClickClose()
        {
            MainCitySys.Instance.CloseUIStrong();
        }

        public void OnClickStrong()
        {
            _audioService.PlayUIAudio(Constant.AudioPathPrefix+Constant.AudioUIClickBtn);
            if (nextStartConfig==null)
            {
                GameRoot.Instance.AddTips("已满级");
            }
            else if (pd.lv < nextStartConfig.minlv)
            {
                GameRoot.Instance.AddTips("等级不够");
            }
            else if (pd.coin < nextStartConfig.coin)
            {
                GameRoot.Instance.AddTips("金币不足");
            }
            else if (pd.crystal < nextStartConfig.crystal)
            {
                GameRoot.Instance.AddTips("水晶不足");
            }
            else
            {
                MainCitySys.Instance.StrongReq(currentClick);
            }

        }

        public void RegisterClickEvent()
        {
            for (int i = 0; i < equipItemParent.childCount; i++)
            {
                OnClickPEListener(equipItemParent.GetChild(i).gameObject, (i) =>
                {
                   OnClickHandle(i);
                },i);
            }
        }
        #endregion
    }
}