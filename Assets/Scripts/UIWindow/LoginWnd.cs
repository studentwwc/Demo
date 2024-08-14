using Common;
using DarkGod.System;
using Protocal;
using Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkGod.UIWindow
{
    public class LoginWnd:WindowRoot
    {
        public TMP_InputField iptAcc;
        public TMP_InputField iptPass;
        public Button btnLogin;
        public Button btnNotice;

        protected override void InitWnd()
        {
            base.InitWnd();
            AudioService.Instance.PlayBgAudio(Constant.AudioBgLogin);
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Account")) &&
                !string.IsNullOrEmpty(PlayerPrefs.GetString("Password")))
            {
                iptAcc.text = PlayerPrefs.GetString("Account");
                iptPass.text = PlayerPrefs.GetString("Password");
            }
            else
            {
                iptAcc.text = "";
                iptPass.text = "";
            }
            
            btnLogin.onClick.AddListener(OnLoginClick);
            btnNotice.onClick.AddListener(OnNoticeClick);
         
        }

        public void OnLoginClick()
        {
            _audioService.PlayUIAudio(Constant.AudioUILoginBtn);
            if (string.IsNullOrEmpty(iptAcc.text)||string.IsNullOrEmpty(iptPass.text))
            {
                GameRoot.Instance.AddTips("请输入完整信息");
                return;
            }
            LoginSys.Instance.LoginReq(iptAcc.text,iptPass.text);
        }

        private void OnNoticeClick()
        {
            _audioService.PlayUIAudio(Constant.AudioUIClickBtn);
            GameRoot.Instance.AddTips("功能正在开发....");
        }
    }
}