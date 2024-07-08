using System.Collections.Generic;
using System.Text;
using Common;
using DarkGod.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkGod.UIWindow
{
    public class ChatWnd:WindowRoot
    {
        private int indexPan;
        public Image[] bgs;
        public TMP_InputField inputSend;
        public TMP_Text txtChat;
        [HideInInspector]
        public List<string> worldMsgList=new List<string>();
        private List<string> insMsgList=new List<string>();
        private List<string> friendList=new List<string>();
        protected override void InitWnd()
        {
            base.InitWnd();
            RefreshUI();
        }

        private string GetMsgByList(List<string>strs)
        {
            StringBuilder sb = new StringBuilder();
            if (strs != null)
            {
                for (int i = 0; i < strs.Count; i++)
                {
                    if (i != strs.Count - 1)
                    {
                        sb.Append(strs[i] + "\n");
                    }
                    else
                    {
                        sb.Append(strs[i]);
                    }
                }
            }
            return sb.ToString();
        }
        public void RefreshUI(int index=0)
        {
            //将以前的设为默认背景,将现在的设为选择背景
            SetSprite(bgs[indexPan],Constant.defaultChatPath);
            SetSprite(bgs[index],Constant.selectedChatPath);
            indexPan = index;
            StringBuilder sb = new StringBuilder();
            //设置信息显示
            switch (index)
            {
                case 0:
                    if (worldMsgList == null)
                    {
                        sb.Append("世界暂无消息");
                    }
                    else
                    {
                        sb.Append(GetMsgByList(worldMsgList));
                    }

                    break;
                case 1:
                    if (insMsgList.Count<1)
                    {
                        sb.Append("公会暂无消息");
                    }
                    else
                    {
                        sb.Append(GetMsgByList(insMsgList));
                    }
                    break;
                case 2:
                    if (friendList.Count<1)
                    {
                        sb.Append("好友暂无消息");
                    }
                    else
                    {
                        sb.Append(GetMsgByList(friendList));
                    }
                    break;
            }

            txtChat.text = sb.ToString();
        }

        public void OnClickClose()
        {
            MainCitySys.Instance.CloseChatWnd();
        }

        #region ClickEvent

        public void OnClickWorldBtn()
        {
            RefreshUI();
        }
        public void OnClickInsBtn()
        {
            RefreshUI(1);
        }
        public void OnClickFriendBtn()
        {
            RefreshUI(2);
        }

        public void OnClickSendMsgBtn()
        {
            if (string.IsNullOrEmpty(inputSend.text))
            {
                GameRoot.Instance.AddTips("请输入消息");
            }else if (inputSend.text.Length>10)
            {
                GameRoot.Instance.AddTips("输入的消息过长");
            }
            else
            {
                MainCitySys.Instance.ChatReq(inputSend.text);
                inputSend.text = "";
            }
        }

        #endregion
    }
}