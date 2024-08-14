using PENet;
using Server._01Service;
using Server._02System;
using Server.Frame;
using Protocal;
using Server._03Cache;
using Server._04DB;
using Server._01Service._03Timer;

namespace Server._00Common
{
    internal class GameRoot:Singleton<GameRoot>
    {
        public void Init() {
            //TODO:初始化数据库层
            DBManager.Instance.Init();

            //初始化服务层
            CacheService.Instance.Init();
            NetService.Instance.Init();
            CfgService.Instance.Init();
            TimerService.Instance.Init();
          


            //初始化业务层
            LoginSystem.Instance.Init();
            TaskSystem.Instance.Init();
            StrongSystem.Instance.Init();
            ChatSystem.Instance.Init();
            BuySystem.Instance.Init();
            PowerSystem.Instance.Init();
            LevelPassSystem.Instance.Init();
            NetCommon.Log("GameRoot Init Done",NetLogType.Normal);
        }
        public void Update() {
            NetService.Instance.Update();
            TimerService.Instance.Update();
        }
    }
}
