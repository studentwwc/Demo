using Common;
using DarkGod.System;
using Protocal;
using UnityEngine;

namespace DarkGod.UIWindow
{
    public class LevelPassWnd:WindowRoot
    {
        protected override void InitWnd()
        {
            base.InitWnd();
            RefreshUI();
        }

        public void OnClickClose()
        {
            LevelSystem.Instance.OnCloseLevelPass();
        }

        public void OnClickLevel(int lid)
        {
            //进入副本关卡
            LevelSystem.Instance.LevelPassReq(lid);
        }
        public Transform pointer;
        public Transform[] levelpass;
        public void RefreshUI()
        {
            PlayerData pd = GameRoot.Instance.PlayerData;
            Debug.Log("wwc------>"+pd.levelpass+ "     "+levelpass.Length);
            for (int i = 0; i < levelpass.Length; i++)
            {
                if (pd.levelpass%1000>i)
                {
                    levelpass[i].gameObject.SetActive(true);
                }
                else
                {
                    levelpass[i].gameObject.SetActive(false);
                }
            }
            pointer.SetParent(levelpass[pd.levelpass%1000]);
            pointer.transform.localPosition = new Vector3(50,100);
        }
    }
}