using System;
using Common;
using Config;
using DarkGod.UIWindow;
using Map;
using Protocal;
using Service;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using Wwc.Cfg;
using Color = Common.Color;

namespace DarkGod.System
{
    public class MainCitySys : SystemRoot<MainCitySys>
    {
        public MainCityWnd mainCityWnd;
        public PlayerInfoWnd playerInfoWnd;
        public GuideWnd guideWnd;
        public StrongWnd strongWnd;
        public ChatWnd chatWnd;
        public BuyWnd buyWnd;
        public TaskRewardWnd taskRewardWnd;
        public BagWnd bagWnd;
        public ShopWnd shopWnd;
        private PlayerController _playerController;
        private Transform charCamTrans;
        [HideInInspector] public float charCamAngleWithPlayer;

        private AutoGuideConfig currentAutoTask;

        private MapRoot mapRoot;

        private NavMeshAgent nav;

        private bool isNavGuide;

        public override void InitSystem()
        {
            base.InitSystem();
            NetCommon.Log("MainCitySys Init Done");
        }

        public void EnterMainCity()
        {
            MapData mainCityMap = resService.GetMapConfig(Constant.MainCityID);
            resService.ASyncLoadScene(mainCityMap.sceneName, () =>
            {
                GameRoot.Instance.AddTips("欢迎进入主城");
                //打开主城面板
                mainCityWnd.SetWndState();
                //播放主城音乐
                aduioService.PlayBgAudio(Constant.AudioBgMainCity);
                //获取NPC的 地址
                mapRoot = GameObject.FindGameObjectWithTag("MapRoot").GetComponent<MapRoot>();
                //初始化角色面板
                mainCityWnd.RefreshUI();
                //加载角色信息
                SetPlayer(mainCityMap);
                //角色相机
                charCamTrans = GameObject.FindGameObjectWithTag("CharCam").transform;
                charCamTrans.gameObject.SetActive(false);
            });
        }

        public void CloseMainCity()
        {
            mainCityWnd.SetWndState(false);
        }

        public void SetPlayer(MapData mainCityConfig)
        {
            GameObject assassin = resService.GetGameObject(CfgUtility.RoleConfigs[GameDataUtility.roleid].PrefabAsset);
            //设置人物的位置以及旋转
            assassin.transform.position = mainCityConfig.playerBornPos;
            assassin.transform.eulerAngles = mainCityConfig.playerBornRote;
            //设置摄像机的位置以及旋转
            Camera.main.transform.position = mainCityConfig.mainCamPos;
            Camera.main.transform.eulerAngles = mainCityConfig.mainCamRote;
            
            mainCityWnd.playerCt = assassin.GetComponent<PlayerController>();
            _playerController = assassin.GetComponent<PlayerController>();
            _playerController.Init();
            nav = assassin.GetComponent<NavMeshAgent>();
        }

        #region PlayerInfoWnd

        public void SetDir(Vector2 dir)
        {
            if (isNavGuide)
            {
                StopAutoTask();
            }

            if (dir != Vector2.zero)
            {
                _playerController.SetBlend(Constant.playerRunBlend);
            }
            else
            {
                _playerController.SetBlend(Constant.playerIdleBlend);
            }

            _playerController.Dir=dir;
        }

        public void ShowUIPlayerInfo()
        {
            if (isNavGuide)
            {
                StopAutoTask();
            }

            aduioService.PlayUIAudio(Constant.AudioPathPrefix + Constant.AudioUIOpenPage);
            playerInfoWnd.SetWndState();

            var playTransform = _playerController.transform;
            var position = playTransform.position;
            charCamTrans.localPosition = position + playTransform.forward * 2.24f + playTransform.up * 0.65f;
            charCamTrans.localEulerAngles = playTransform.localEulerAngles + new Vector3(0, 180, 0);
            charCamTrans.gameObject.SetActive(true);
            charCamAngleWithPlayer = Vector3.Angle(playTransform.forward, charCamTrans.position - position);
        }

        public void ClosePlayerInfoPage()
        {
            aduioService.PlayUIAudio(Constant.AudioPathPrefix + Constant.AudioUICloseBtn);
            playerInfoWnd.SetWndState(false);
            charCamTrans.gameObject.SetActive(false);
        }

        public void CharCamAroundPlayerRotate(float angle)
        {
            //  var playerPosition = _playerController.transform.position;
            //  var camPos = charCamTrans.position;
            //  float border1 = Mathf.Cos(charCamAngleWithPlayer) *
            //                  Vector3.Magnitude(camPos - playerPosition);
            //  float y = Mathf.Sin(charCamAngleWithPlayer) * Vector3.Magnitude(camPos - playerPosition);
            //  float x = Mathf.Sin(angle*Mathf.Deg2Rad) * border1;
            //  float z = Mathf.Cos(angle*Mathf.Deg2Rad) * border1;
            //  camPos = playerPosition + new Vector3(x, camPos.y, z);
            //  charCamTrans.position = camPos;
            //  var localEulerAngles = charCamTrans.localEulerAngles;
            //  charCamTrans.localEulerAngles = new Vector3(localEulerAngles.x,localEulerAngles.y-angle,localEulerAngles.z);
            float temp = angle - charCamAngleWithPlayer;
            charCamTrans.RotateAround(_playerController.transform.position, Vector3.up, temp);
            charCamAngleWithPlayer = angle;
        }

        #endregion

        #region AutoTask

        public void RunAutoTask(AutoGuideConfig agc)
        {
            if (agc != null)
            {
                currentAutoTask = agc;
            }

            if (currentAutoTask.npcID != -1)
            {
                //寻路
                nav.enabled = true;
                float dis = Vector3.Distance(_playerController.transform.position,
                    mapRoot.npcTrans[currentAutoTask.npcID].position);
                if (dis < 0.5)
                {
                    StopAutoTask();
                    ShowNPCTalk();
                }
                else
                {
                    isNavGuide = true;
                    _playerController.GetComponent<CharacterController>().enabled = false;
                    nav.enabled = true;
                    nav.speed = Constant.playerMoveSpeed;
                    _playerController.SetBlend(Constant.playerRunBlend);
                    nav.SetDestination(mapRoot.npcTrans[currentAutoTask.npcID].position);
                }
            }
            else
            {
                //TODO:打开NPC对话面板
                ShowNPCTalk();
            }
        }

        public void ShowNPCTalk()
        {
            guideWnd.SetWndState();
        }

        public void StopAutoTask()
        {
            _playerController.SetBlend(Constant.playerIdleBlend);
            _playerController.GetComponent<CharacterController>().enabled = true;
            nav.isStopped = true;
            nav.enabled = false;
            isNavGuide = false;
        }

        public void CheckDistance()
        {
            float dis = Vector3.Distance(_playerController.transform.position,
                mapRoot.npcTrans[currentAutoTask.npcID].position);
            if (dis < 0.5)
            {
                StopAutoTask();
                ShowNPCTalk();
            }
        }

        public AutoGuideConfig GetCurrentAutoTaskConfig()
        {
            return currentAutoTask;
        }

        public void TaskReq(int guideid)
        {
            NetMsg netMsg = new NetMsg()
            {
                cmd = (int)TransCode.TaskReq,
                taskReq = new TaskReq()
                {
                    guideid = guideid
                }
            };
            _netService.SendMsg(netMsg);
        }

        public void TaskRes(NetMsg netMsg)
        {
            GameRoot.Instance.AddTips("奖励已领取Exp:" + currentAutoTask.exp + ",Coin:" + currentAutoTask.coin, Color.Blue);

            switch (currentAutoTask.actID)
            {
                case 0:
                    //TODO:与智者对话
                    break;
                case 1:
                    //TODO:进入副本
                    EnterLeverlPass();
                    break;
                case 2:
                    //TODO：强化界面
                    ShowUIStrong();
                    break;
                case 3:
                    //TODO:体力购买
                    OpenBuyWnd(1);
                    break;
                case 4:
                    //TODO:金币铸造
                    OpenBuyWnd(0);
                    break;
                case 5:
                    //TODO:世界聊天
                    OpenChatWnd();
                    break;
            }

            //更新信息
            GameRoot.Instance.SetPlayerDataByTask(netMsg.taskRes);
            //刷新UI
            mainCityWnd.RefreshUI();
        }

        #endregion

        #region EquipStrong

        public void ShowUIStrong()
        {
            if (isNavGuide)
            {
                StopAutoTask();
            }
            aduioService.PlayUIAudio(Constant.AudioPathPrefix + Constant.AudioUICloseBtn);
            strongWnd.SetWndState();
        }

        public void CloseUIStrong()
        {
            aduioService.PlayUIAudio(Constant.AudioPathPrefix + Constant.AudioUICloseBtn);
            strongWnd.SetWndState(false);
        }

        public void StrongReq(int pos)
        {
            _netService.SendMsg(new NetMsg()
            {
                cmd = (int)TransCode.StrongReq,
                strongReq = new StrongReq()
                {
                    pos = pos
                }
            });
        }

        public void StrongRes(NetMsg netMsg)
        {
            PlayerData pd = GameRoot.Instance.PlayerData;
            StrongRes res = netMsg.strongRes;
            int preFight = NetCommon.GetFight(pd);
            pd.hp = res.hp;
            pd.ad = res.ad;
            pd.ap = res.ap;
            pd.addef = res.addef;
            pd.apdef = res.apdef;
            pd.coin = res.coin;
            pd.crystal = res.crystal;
            pd.strongArr = res.stongArr;
            int nowFight = NetCommon.GetFight(pd);
            GameRoot.Instance.AddTips("战力+" + (nowFight - preFight), Color.Blue);

            //刷新界面
            strongWnd.SetContent(strongWnd.currentClick);
            mainCityWnd.RefreshUI();
        }

        #endregion

        #region ChatWnd

        public void OpenChatWnd()
        {
            if (isNavGuide)
            {
                StopAutoTask();
            }
            aduioService.PlayUIAudio(Constant.AudioPathPrefix + Constant.AudioUIClickBtn);
            chatWnd.SetWndState();
        }

        public void CloseChatWnd()
        {
            aduioService.PlayUIAudio(Constant.AudioPathPrefix + Constant.AudioUIClickBtn);
            chatWnd.SetWndState(false);
        }

        public void ChatReq(string msg)
        {
            _netService.SendMsg(new NetMsg()
            {
                chatReq = new ChatReq()
                {
                    msg = msg
                },
                cmd = (int)TransCode.ChatReq
            });
        }

        public void ChatRes(NetMsg netMsg)
        {
            ChatRes chatres = netMsg.chatRes;
            if (chatres.name.Equals(GameRoot.Instance.PlayerData.name))
            {
                chatWnd.worldMsgList.Add(Constant.blueColor + chatres.name + ":" + Constant.colorEnd + chatres.msg);
            }
            else
            {
                chatWnd.worldMsgList.Add(Constant.blackColor + chatres.name + ":" + Constant.colorEnd + chatres.msg);
            }

            if (chatWnd.GetState())
            {
                chatWnd.RefreshUI();
            }
        }

        #endregion

        #region Buy

        public void OpenBuyWnd(int buyType)
        {
            if (isNavGuide)
            {
                StopAutoTask();
            }
            aduioService.PlayUIAudio(Constant.AudioPathPrefix + Constant.AudioUIOpenPage);
            buyWnd.BuyType = buyType;
            buyWnd.SetWndState();
            buyWnd.RefreshUI();
        }

        public void CloseBuyWnd()
        {
            aduioService.PlayUIAudio(Constant.AudioPathPrefix + Constant.AudioUICloseBtn);
            buyWnd.SetWndState(false);
        }

        public void BuyReq(int buyType, int cost)
        {
            BuyReq buyReq = new BuyReq()
            {
                buyType = buyType,
                cost = cost,
            };
            _netService.SendMsg(new NetMsg()
            {
                buyReq = buyReq,
                cmd = (int)TransCode.BuyReq,
            });
        }

        public void BuyRes(NetMsg netMsg)
        {
            PlayerData pd = GameRoot.Instance.PlayerData;
            BuyRes buyRes = netMsg.buyRes;
            pd.diamond = buyRes.diamond;
            switch (buyRes.buyType)
            {
                case 0:
                    pd.coin = buyRes.coin;
                    break;
                case 1:
                    pd.power = buyRes.power;
                    break;
            }

            mainCityWnd.RefreshUI();
            GameRoot.Instance.AddTips("购买成功");
        }

        #endregion

        #region Repower

        public void RepowerRes(NetMsg msg)
        {
            GameRoot.Instance.PlayerData.power = msg.resumePowerRes.power;
            if (mainCityWnd.GetState())
            {
                mainCityWnd.RefreshUI();
            }
        }

        #endregion

        #region TaskReward

        public void ShowUITaskReward()
        {
            if (isNavGuide)
            {
                StopAutoTask();
            }
            aduioService.PlayUIAudio(Constant.AudioPathPrefix + Constant.AudioUIOpenPage);
            taskRewardWnd.SetWndState();
        }

        public void CloseUITaskReward()
        {
            aduioService.PlayUIAudio(Constant.AudioPathPrefix + Constant.AudioUICloseBtn);
            taskRewardWnd.SetWndState(false);
        }

        public void TaskRewardReq(int tid)
        {
            this.tid = tid;
            _netService.SendMsg(new NetMsg()
            {
                cmd = (int)TransCode.TaskRewardReq,
                taskRewardReq = new TaskRewardReq()
                {
                    tid = tid
                }
            });
        }

        private int tid;
        public void TaskRewardRes(NetMsg netMsg)
        {
            TaskRewardRes taskRewardRes = netMsg.taskRewardRes;
            PlayerData pd = GameRoot.Instance.PlayerData;
            pd.coin = taskRewardRes.coin;
            pd.lv = taskRewardRes.lv;
            pd.exp = taskRewardRes.exp;
            pd.taskReward = taskRewardRes.taskArr;
            taskRewardWnd.RefreshUI();
            mainCityWnd.RefreshUI();
            TaskRewardConfig config= resService.GetTaskRewardConfig(tid);
            GameRoot root=GameRoot.Instance;
            root.AddTips("奖励金币:"+root.GetColorText(config.coin.ToString(),Color.Blue)+"  经验"+root.GetColorText(config.exp.ToString()  ,Color.Blue));
        }

        public void TaskProgRes(NetMsg netMsg)
        {
            TaskProgRes progRes = netMsg.taskProgRes;
            GameRoot.Instance.PlayerData.taskReward = progRes.taskRewardArr;
        }

        #endregion

        #region LevelPass

        public void EnterLeverlPass()
        {
            if (isNavGuide)
            {
                StopAutoTask();
            }
            GameRoot.Instance.EnterLevelPass();
        }


        #endregion

        #region Inventory

        public void ShowBagPackUI()
        {
            InventorySystem.Instance.SetUp();
            
            bagWnd.gameObject.SetActive(true);
        }
        public void CloseBagPackUI()
        {
            bagWnd.gameObject.SetActive(false);
        }

        #endregion
        #region Shop

        public void ShowShopUI()
        {
            InventorySystem.Instance.SetUp();
            
            shopWnd.gameObject.SetActive(true);
        }
        public void CloseShopUI()
        {
            shopWnd.gameObject.SetActive(false);
        }

        public void BuyShopItemReq(ShopItem shopItem)
        {
            NetService.Instance.SendMsg(new NetMsg()
            {
                cmd =(int) TransCode.BuyShopItemReq,
                buyShopItemReq =new BuyShopItemReq()
                {
                    id = shopItem.id,
                    cost = shopItem.cost,
                    type = (int)shopItem.shopType,
                    count= shopItem.count,
                }
            });
        }
        public void BuyShopItemRes(BuyShopItemRes buyShopItemRes)
        {
           GameRoot.Instance.PlayerData.diamond = buyShopItemRes.diamond;
           switch (buyShopItemRes.type)
           {
               case 1:
                   GameDataUtility.AddPack(buyShopItemRes.id,buyShopItemRes.goodsid,buyShopItemRes.count,buyShopItemRes.index);
                   break;
               case 2:
                   GameRoot.Instance.PlayerData.crystal += buyShopItemRes.count;
                   break;
               case 3:
                   GameDataUtility.CalculateExp(GameRoot.Instance.PlayerData,buyShopItemRes.count);
                   break;
               case 4:
                   GameRoot.Instance.PlayerData.coin += buyShopItemRes.count;
                   break;
           }
          RefreshUI();
          GameRoot.Instance.AddTips("购买成功!!!");
        }
       
        #endregion

        private void Update()
        {
            if (isNavGuide)
            {
                _playerController.SetCameraFollow();
                CheckDistance();
            }
        }

        public void RefreshUI()
        {
            mainCityWnd.RefreshUI();
        }
    }
}