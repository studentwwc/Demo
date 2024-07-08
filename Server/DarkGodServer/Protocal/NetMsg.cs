using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PENet;
namespace Protocal
{
    [Serializable]
    public class NetMsg : PEMsg
    {
        public LoginReq loginReq;
        public LoginRes loginRes;
        public RenameReq renameReq;
        public RenameRes renameRes;
        public TaskReq taskReq;
        public TaskRes taskRes;
        public StrongReq strongReq;
        public StrongRes strongRes;
        public ChatReq chatReq;
        public ChatRes chatRes;
        public BuyReq buyReq;
        public BuyRes buyRes;
        public RePowerRes resumePowerRes;
        public TaskRewardReq taskRewardReq;
        public TaskRewardRes taskRewardRes;
        public TaskProgRes taskProgRes;
        public LevelPassReq levelPassReq;
        public LevelPassRes levelPassRes;
        public LevelPassStateReq levelPassStateReq;
        public LevelPassStateRes levelPassStateRes;
        public AllPackReq allPackReq;
        public AllPackRes allPackRes;
        public UpdatePackStateReq updatePackStateReq;
        public UpdateUserPropReq updateUserPropReq;
        public BuyShopItemReq buyShopItemReq;
        public BuyShopItemRes buyShopItemRes;
    }
    public class IPCfg {
        public const string srv = "127.0.0.1";
        public const int port = 17666;
    }
    #region 登录相关
    [Serializable]
    public class LoginReq
    {
        public string account;
        public string password;
    }
    [Serializable]
    public class LoginRes
    {
        public PlayerData playerData;
        public int roleid;
    }

    #endregion

    #region 重命名
    [Serializable]
    public class RenameReq {
        public string name;
        public int roleid;
        public string prop;
    }
    [Serializable]
    public class RenameRes {

    }
    #endregion

    #region 任务
    [Serializable]
    public class TaskReq
    {
        public int guideid;
    }
    [Serializable]
    public class TaskRes
    {
        public int guideid;
        public int coin;
        public int exp;
        public int lv;
    }
    #endregion
    #region 强化
    [Serializable]
    public class StrongReq {
        public int pos;
    }
    [Serializable]
    public class StrongRes {
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int coin;
        public int crystal;
        public int[] stongArr;
    }
    #endregion
    #region Chat相关
    [Serializable]
    public class ChatReq {
        public string msg;
    }
    [Serializable]
    public class ChatRes {
        public string name;
        public string msg;
    }
    #endregion
    #region 购买相关
    [Serializable]
    public class BuyReq {
        public int buyType;
        public int cost;
    }
    [Serializable]
    public class BuyRes { 
        public int buyType;
        public int diamond;
        public int coin;
        public int power;
    }
    #endregion
    #region 恢复体力
    [Serializable]
    public class RePowerRes {
        public int power;
    }
    #endregion
    #region 任务奖励
    [Serializable]
    public class TaskRewardReq {
        public int tid;
    }
    [Serializable]
    public class TaskRewardRes {
        public int lv;
        public int exp;
        public int coin;
        public string[] taskArr;
    }
    [Serializable]
    public class TaskProgRes {
        public string[] taskRewardArr;
    }
    #endregion
    #region 副本战斗
    [Serializable]
    public class LevelPassReq {
        public int lid;
    }
    [Serializable]
    public class LevelPassRes {
        public int lid;
        public int power;
    }
    [Serializable]
    public class LevelPassStateReq {
        public int lid;
        public bool state;
    }
    [Serializable]
    public class LevelPassStateRes {
        public int lid;
    }
    #endregion

    #region 背包系统
    [Serializable]
    public class AllPackReq {
        public int userid;
    }
    [Serializable]
    public class AllPackRes {
        public List<PlayerPack> packs;
        public AllPackRes(List<PlayerPack> packs)
        {
            this.packs = packs;
        } 
    }
    [Serializable]
    public class UpdatePackStateReq {
        public int packId;
        public int opt;//1更换位置  2售卖物品 3使用消耗品
        public int changeIndex;
        public int coin;
        public int exp;
    }
    [Serializable]
    public class UpdateUserPropReq
    {
        public int userid;
        public int ad;
        public int ap;
        public int addef;
        public int hp;
        public int dodge;
        public int critical;
    }
    #endregion

    #region 商城系统
    [Serializable]
    public class BuyShopItemReq {
        public int id;
        public int type;
        public int cost;
        public int count;
    }
    [Serializable]
    public class BuyShopItemRes
    {
        public int id;
        public int diamond;
        public int count;
        public int type;
        public int index;
        public int goodsid;
    }
    #endregion
    public enum TransCode { 
       None=0,

       LoginReq=100, //关于登陆的100开始
       LoginRes=101,
       RenameReq=102,
       RenameRes=103,

       TaskReq=201,  //主城
       TaskRes=202,

       StrongReq=203,
       StrongRes=204,

       ChatReq=205,
       ChatRes=206,

       BuyReq=207,
       BuyRes=208,

       RepowerRes=209,

       TaskRewardReq=210,
       TaskRewardRes=211,

       TaskProgRes=212,


       LevelPassReq=301,
       LevelPassRes = 302,
       LevelPassStateReq=303,
       LevelPassStateRes = 304,

        AllPackReq =401,
       AllPackRes=402,

       UpdatePackStateReq=403,
       UpdateUserPropReq= 404,

       BuyShopItemReq=501,
       BuyShopItemRes = 502,
    }
    public enum NetErrorCode { 
       None=0,
       AccOnLine,//用户已经在线上
       ErrorPassword,//密码错误
       NameExisted,
       UpdateFail,
       UnMatch,
       LackLevel,
       LackCoin,
       LackCrystal,
       LackDiamond,
       LackPower,
    }
    #region 玩家信息
    [Serializable]
    public class PlayerData
    {
        public int id;//玩家号
        public string name;//玩家名字
        public int lv;//玩家等级
        public int exp;//玩家经验值
        public int power;//玩家体力
        public int coin;//玩家金币
        public int diamond;//玩家钻石
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;//闪避概率
        public int pierce;//穿透比例
        public int critical;//暴击概率
        public int guideid;//自动任务
        //0#0#0#0#0#0 下标代表装备的位置 数字代表当前位置的星级
        public int[] strongArr;
        public long repower;
        //1|0|0#1|0|0#1|0|0#1|0|0#1|0|0#1|0|0 #分割位置第一个数字代表位置第二个代表当前完成的数字第三个代表是否是否领取奖励
        public string[] taskReward;
        public int levelpass;
    }
    [Serializable]
    public class PlayerPack {
        public int id;
        public int userid;
        public int goodsid;
        public int count;
        public int packindex;
    }
    #endregion
}
