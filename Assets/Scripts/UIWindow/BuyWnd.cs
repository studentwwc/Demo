using Common;
using DarkGod.System;
using Protocal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkGod.UIWindow
{
    public class BuyWnd:WindowRoot
    {
        public int buyType;
        [SerializeField]private TMP_Text txtBuyInfo;

        public int BuyType
        {

            get
            {
                return buyType;
            }
            set
            {
                buyType = value;
            }
        }

        protected override void InitWnd()
        {
            base.InitWnd();
        }

        public void RefreshUI()
        {
            switch (buyType)
            {
                case 0:
                    txtBuyInfo.text = "是否花费<color=red>10钻石</color>购买<color=green>1000金币</color>？";
                    break;
                case 1:
                    txtBuyInfo.text = "是否花费<color=red>10钻石</color>购买<color=green>100体力</color>？";
                    break;
            }
        }

        #region ClickEvent

        public void OnClickCloseBtn()
        {
            MainCitySys.Instance.CloseBuyWnd();
        }
        public void OnClickBuyBtn()
        {
            if (GameRoot.Instance.PlayerData.diamond < 10)
            {
                GameRoot.Instance.AddTips("钻石不足");
            }
            else
            {
                MainCitySys.Instance.BuyReq( buyType,10);
            }
        }


        #endregion
    }
}