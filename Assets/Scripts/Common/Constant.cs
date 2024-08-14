namespace Common
{
    public class Constant
    {
        #region Scene

        public const string SceneLogin = "SceneLogin";
        public const string SceneMainCity = "SceneMainCity";
        public const string SceneOrg = "SceneOrge";
        public const int MainCityID = 10000;

        #endregion

        #region Screen

        public const int ScreenStandardWidth = 1334;
        public const int ScreenStandardHeight = 750;

        public const int ScreenOpDis = 85;
        

        #endregion
        #region Audio

        public const string AudioPathPrefix = "ResAudio/";
        //Bg
        public const string AudioBgLogin = "bgLogin";
        public const string AudioBgMainCity = "bgMainCity";
        public const string AudioBgHuangYe = "bgHuangYe";
        //Other
        public const string AudioUIClickBtn = "uiClickBtn";
        public const string AudioUILoginBtn="uiLoginBtn";
        public const string AudioUIExtenBtn= "uiExtenBtn";
        public const string AudioUIOpenPage = "uiOpenPage";
        public const string AudioUICloseBtn= "uiCloseBtn";
        public const string AudioPlayerHit= "assassin_Hit";
        #endregion

        #region ConfigPath
        public const string ConfigPathPrefix = "ResConfig/";
        public const string RdName = "rdname";
        public const string Map = "map";
        public const string AutoGuide = "AutoGuide";
        public const string Strong = "strong";
        public const string TaskReward = "taskreward";
        public const string Skill = "skill";
        public const string SkillMove = "skillMove";
        public const string SkillAction = "skillAction";
        public const string Monster = "monster";
        #endregion

        #region Speed

        public const int playerMoveSpeed = 5;
        public const int monsterMoveSpeed = 3;

        public const int playerMoveAccelerateSpeed = 5;
        #endregion

        #region AnimatorParam

        public const int playerIdleBlend = 0;
        public const int playerRunBlend = 1;


        #endregion

        #region PlayerAndMonsterPath

        public const string AssassinCityPath = "PrefabPlayer/AssassinCity";
        public const string AssassinBattlePath = "PrefabPlayer/AssassinBattle";
        
        public const string ItemEntityHpPath = "PrefasUI/ItemEntityHp";


        #endregion

        #region NPC

        public const string taskHeadPath = "ResImages/task";
        public const string wiseManHeadPath = "ResImages/wiseman";
        public const string generalHeadPath = "ResImages/general";
        public const string artisanHeadPath = "ResImages/artisan";
        public const string traderHeadPath = "ResImages/trader";
        
        public const string selfIconPath = "ResImages/assassin";
        public const string wiseManIconPath = "ResImages/npc0";
        public const string generalIconPath = "ResImages/npc1";
        public const string artisanIconPath = "ResImages/npc2";
        public const string traderIconPath = "ResImages/npc3";
        public const string defaultIconPath = "ResImages/npcguide";
        
        public const int npcWiseMan = 0;
        public const int npcGeneral = 1;
        public const int npcArtisan = 2;
        public const int npcTrader = 3;
        
        #endregion

        #region Color

        public const string redColor = "<color=#FF0000>";
        public const string yellowColor="<color=#FFE901>";
        public const string blueColor="<color=#1809FF>";
        public const string whiteColor="<color=#FFFFFF>";
        public const string blackColor="<color=#000000>";

        public const string colorEnd = "</color>";


        #endregion

        #region EquipUIPath

        public const string equipHeadPath = "ResImages/toukui";
        public const string equipBodyPath = "ResImages/body";
        public const string equipWaistPath = "ResImages/yaobu";
        public const string equipHandPath = "ResImages/hand";
        public const string equipLegPath = "ResImages/leg";
        public const string equipFootPath = "ResImages/foot";

        public const string selectStartPath = "ResImages/star2";
        public const string unSelectStartPath = "ResImages/star1";

        #endregion

        #region ChatUIPath

        public const string defaultChatPath = "ResImages/btntype2";
        public const string selectedChatPath = "ResImages/btntype1";
        
        #endregion

        #region taskReward

        public const string taskItemPath = "PrefasUI/taskItem";
        public const string taskRewardCan = "ResImages/box2";
        public const string taskRewardNotCan = "ResImages/box1";

        #endregion

        #region Action
        public const int defaultAction = -1;
        public const int bornAction = 0;
        public const int dieAction = 101;
        public const int hitAction = 100;
        #endregion

        public const int comboSpace = 5000;
    }

    public enum Color
    {
        White,
        Black,
        Blue,
        Yellow,
        Red
    }
}