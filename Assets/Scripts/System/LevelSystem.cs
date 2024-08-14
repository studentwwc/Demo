using Common;
using Config;
using DarkGod.UIWindow;
using Protocal;
using Service;
using UnityEngine;

namespace DarkGod.System
{
    public class LevelSystem:SystemRoot<LevelSystem>
    {
        public LevelPassWnd levelPassWnd;
        public override void InitSystem()
        {
            base.InitSystem();
            NetCommon.Log("LevelSystem Init Done");
        }

        public void LevelPassReq(int lid)
        {
            aduioService.PlayUIAudio(Constant.AudioPathPrefix+Constant.AudioUIClickBtn);
            _netService.SendMsg(new NetMsg()
            {
                cmd = (int)TransCode.LevelPassReq,
                levelPassReq =new LevelPassReq(){
                    lid = lid,
                },
            });
        }
        public void LevelPassRes(NetMsg netMsg)
        {
            PlayerData pd=GameRoot.Instance.PlayerData;
            LevelPassRes res= netMsg.levelPassRes;
            pd.power = res.power;
            MainCitySys.Instance.CloseMainCity();
            OnCloseLevelPass();
            BattleSystem.Instance.StartEnterBattle(res.lid);
        }

        public void OnEnterLevelPass()
        {
            levelPassWnd.SetWndState();
        }

        public void OnCloseLevelPass()
        {
            aduioService.PlayUIAudio(Constant.AudioPathPrefix+Constant.AudioUICloseBtn);
            levelPassWnd.SetWndState(false);
        }

        public void HandleLevelPassStateRes(NetMsg netMsg)
        {
            LevelPassStateRes levelPassStateRes=netMsg.levelPassStateRes;
            if (levelPassStateRes != null)
            {
                GameRoot.Instance.PlayerData.levelpass = levelPassStateRes.lid;
                Debug.Log("HandleLevelPassStateRes Success");
            }
        }
    }
}