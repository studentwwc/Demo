/****************************************************
    文件：ResService.cs
	作者：WWC
    邮箱: 1469359779@qq.com
    日期：2023/4/22 10:3:51
	功能：资源服务模块
*****************************************************/


using System;
using System.Collections.Generic;
using Common;
using Stray;
using Wwc.Cfg;
using UnityEngine;
using UnityEngine.SceneManagement;
using MonsterConfig = Wwc.Cfg.MonsterConfig;

public class ResService : MonoBehaviour
{
    private static ResService instance;
    public Action pgAction;
    private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>(); //音频缓存

    //AutoConfigDic
    private Dictionary<int, AutoGuideConfig> autoGuideDic = new Dictionary<int, AutoGuideConfig>();

    //prefab资源
    public Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();

    //Sprite资源
    public Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();

    //StrongDic
    public Dictionary<int, Dictionary<int, StrongConfig>> strongDic =
        new Dictionary<int, Dictionary<int, StrongConfig>>();

  

    //SkillDic
    private Dictionary<int, SkillData> skillDic = new Dictionary<int, SkillData>();

    //SkillMoveDic
    private Dictionary<int, SkillMoveConfig> skillMoveDic = new Dictionary<int, SkillMoveConfig>();
    //SkillActionDic
    private Dictionary<int, SkillActionConfig> skillActionDic = new Dictionary<int, SkillActionConfig>();
    

    //Monster
    private Dictionary<int, Wwc.Cfg.MonsterConfig> monsterDic = new Dictionary<int, Wwc.Cfg.MonsterConfig>();
    
    public static ResService Instance
    {
        get { return instance; }
    }

    public void ReSetSkillDic()
    {
        skillDic.Clear();
        skillMoveDic.Clear();
        InitSkillMoveConfig();
        InitSkillActionConfig();
        InitSkillConfig();
        NetCommon.Log("ReSet Skill Init");
    }

    public void Init()
    {
        instance = this;

        //InitMonsterConfig
        InitMonsterConfig();
        //初始化随机名配置文件
        InitRDConfig();
        //Map初始化
        InitMapConfig();
        //自动导航初始化
        InitAutoGuideConfig();
        //强化装备
        InitStrongConfig();
        //任务奖励
        InitTaskRewardConfig();
        //InitSkillConfig
        InitSkillConfig();
        //InitSkillMoveConfig
        InitSkillMoveConfig();
        //InitSkillActionConfig
        InitSkillActionConfig();

        NetCommon.Log("Res Service Init");
    }
    //随机名称集合
    private List<string> surnameList = new List<string>();
    private List<string> maleNameList = new List<string>();
    private List<string> femaleNameList = new List<string>();
    public string GetRandomName(bool isMan = true)
    {
        string randomName = surnameList[PETool.GetRandom(0, surnameList.Count)];
        if (isMan)
        {
            randomName += maleNameList[PETool.GetRandom(0, maleNameList.Count)];
        }
        else
        {
            randomName += femaleNameList[PETool.GetRandom(0, femaleNameList.Count)];
        }

        return randomName;
    }

    public Wwc.Cfg.MapData GetMapConfig(int ID)
    {
        Wwc.Cfg.MapData mapConfig = null;
        mapConfigDic.TryGetValue(ID, out mapConfig);
        return mapConfig;
    }
    public TaskRewardConfig GetTaskRewardConfig(int ID)
    {
        TaskRewardConfig res = null;
        taskRewardDic.TryGetValue(ID, out res);
        return res;
    }
    public List<ShopItem> GetShops()
    {
        if (_shopItems == null)
        {
            _shopItems = new List<ShopItem>();
            _shopItems.Add(new ShopItem()
            {
                id = 1,
                cost = 200,
                shopType = ShopItem.ShopType.Coin,
                icon = "ResImages/Shop/coin",
                count = 200,
            });
            _shopItems.Add(new ShopItem()
            {
                id = 2,
                cost = 300,
                shopType = ShopItem.ShopType.Crystal,
                icon = "ResImages/Shop/crystal",
                count = 200,
            });
            _shopItems.Add(new ShopItem()
            {
                id = 3,
                cost = 300,
                shopType = ShopItem.ShopType.Exp,
                icon = "ResImages/Shop/exp",
                count = 50,
            });
            _shopItems.Add(new ShopItem()
            {
                id = 1001,
                cost = 1000,
                shopType = ShopItem.ShopType.Bag,
                icon = "ResImages/Shop/Weapon",
                count = 1
            });
        }

        return _shopItems;
    }
     //Monster
    private void InitMonsterConfig()
    {
        MonstersConfig MonstersConfig = Resources.Load<MonstersConfig>("ResConfig/MonstersConfig");
        foreach (MonsterConfig var in MonstersConfig.Monster)
        {
            MonsterConfig monsterConfig = new MonsterConfig();
            monsterConfig.ID = var.ID;
            monsterConfig.mName = var.mName;
            monsterConfig.resPath = var.resPath;
            monsterConfig.skillID = var.skillID;
            monsterConfig.atkDis = var.atkDis;
            monsterConfig.ad = var.ad;
            monsterConfig.ap = var.ap;
            monsterConfig.addef = var.addef;
            monsterConfig.apdef = var.apdef;
            monsterConfig.critical = var.critical;
            monsterConfig.dodge = var.dodge;
            monsterConfig.pierce = var.pierce;
            monsterConfig.hp = var.hp;
            monsterDic.Add(var.ID,monsterConfig);
        }
    }
    public MonsterConfig GetMonsterConfig(int ID)
    {
        MonsterConfig monsterConfig = null;
        monsterDic.TryGetValue(ID, out monsterConfig);
        return monsterConfig;
    }
    private void InitMapConfig()
    {
       
        MapsConfig mapsConfig = Resources.Load<MapsConfig>("ResConfig/MapsConfig");
        foreach (MapConfig map in mapsConfig.Map)
        {
            MapData mapData = new Wwc.Cfg.MapData();
            mapData.ID = map.ID;
            mapData.power = map.power;
            mapData.mapName = map.mapName;
            mapData.sceneName = map.sceneName;
            mapData.mainCamPos = Utils.StringToVector3(map.mainCamPos);
            mapData.mainCamRote = Utils.StringToVector3(map.mainCamRote);
            mapData.playerBornPos = Utils.StringToVector3(map.playerBornPos);
            mapData.playerBornRote = Utils.StringToVector3(map.playerBornRote);
            mapData.monsters = new List<MonsterData>();
            mapData.monsters = StringToMonsters(map.monsters);
            mapConfigDic.Add(map.ID,mapData);
        }

    }
    public  List<MonsterData> StringToMonsters(string str)
    {
        List<MonsterData> monsterDatas = new List<MonsterData>();
        string []monsterWave=str.Split('#');
        for (int i = 1; i < monsterWave.Length; i++)
        {
            
            string[] index = monsterWave[i].Split('|');
            for (int j = 1; j < index.Length; j++)
            {
                string[] detail = index[j].Split(',');
                MonsterData monsterData = new MonsterData();
                monsterData.batch = i;
                monsterData.index = j;
                monsterData.ID = int.Parse(detail[0]);
                monsterData.pos = Utils.StringToVector3(detail[1] +","+ detail[2] +","+ detail[3]);
                monsterData.rot = new Vector3(0, float.Parse(detail[4]), 0);
                monsterData.lv = int.Parse(detail[5]);
                monsterData._monsterConfig = monsterDic[monsterData.ID];
                BattleData battleData = new BattleData()
                {
                     
                    ad = monsterData._monsterConfig.ad,
                    ap = monsterData._monsterConfig.ap,
                    addef = monsterData._monsterConfig.addef,
                    apdef = monsterData._monsterConfig.apdef,
                    critical = monsterData._monsterConfig.critical,
                    dodge = monsterData._monsterConfig.dodge,
                    pierce = monsterData._monsterConfig.pierce,
                    hp = monsterData._monsterConfig.hp,
                };
                monsterData.battleData = battleData;
                monsterDatas.Add(monsterData);
            }
        }
   
        return monsterDatas;
    }
    //TaskRewardDic
    private Dictionary<int, TaskRewardConfig> taskRewardDic = new Dictionary<int, TaskRewardConfig>();
    //地图配置集合
    private Dictionary<int, Wwc.Cfg.MapData> mapConfigDic = new Dictionary<int,  Wwc.Cfg.MapData>();

    //异步加载场景
    public void ASyncLoadScene(string sceneName, Action action)
    {
        //显示加载页面
        GameRoot.Instance.loadingWnd.SetWndState(); //初始化加载页面

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        pgAction += () =>
        {
            float pg = operation.progress;
            GameRoot.Instance.loadingWnd.SetProgress(pg);
            if (pg == 1)
            {
                if (action != null)
                {
                    action.Invoke();
                }

                pgAction = null;
                operation = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };
    }

    //获取音频资源
    public AudioClip LoadAudio(string audioName, bool isSave = true)
    {
        AudioClip ac;
        if (!audioDic.TryGetValue(audioName, out ac))
        {
            ac = Resources.Load<AudioClip>(Constant.AudioPathPrefix + audioName);
            if (isSave)
            {
                audioDic.Add(audioName, ac);
            }
        }

        return ac;
    }

    //初始化随机名称资源
    private void InitRDConfig()
    {
        RenamesConfig renamesConfig = Resources.Load<RenamesConfig>("ResConfig/RenamesConfig");
        foreach (RenameConfig var in renamesConfig.Rdname)
        {
            if (!string.IsNullOrEmpty(var.surname))
            {
                surnameList.Add(var.surname);
            }
            if (!string.IsNullOrEmpty(var.man))
            {
                maleNameList.Add(var.man);
            }
            if (!string.IsNullOrEmpty(var.woman))
            {
                femaleNameList.Add(var.woman);
            }
        }
    }

   
    
    
    private void InitAutoGuideConfig()
    {
        TaskConfig taskConfig = Resources.Load<TaskConfig>("ResConfig/TaskConfig");
        
        foreach (var var in taskConfig.AutoGuide)
        {
            autoGuideDic.Add(var.ID, var);
        }
    }

    private void InitStrongConfig()
    {
        StrongsConfig strongsConfig = Resources.Load<StrongsConfig>("ResConfig/StrongsConfig");
        foreach (StrongConfig var in strongsConfig.Strong)
        {
            if (strongDic.ContainsKey(var.pos))
            {
                strongDic[var.pos].Add(var.starlv,var);
            }
            else
            {
                strongDic.Add(var.pos,new Dictionary<int, StrongConfig>());
                strongDic[var.pos].Add(var.starlv,var);
            }
        }
    }

    private void InitTaskRewardConfig()
    {
        TaskRewardsConfig taskConfig = Resources.Load<TaskRewardsConfig>("ResConfig/TaskRewardsConfig");
        foreach (TaskRewardConfig var in taskConfig.TaskReward)
        {
            taskRewardDic.Add(var.ID, var);
        }
    }

    public SkillData SkillConfigToData(SkillConfig skillConfig)
    {
        SkillData skillData = new SkillData();
        skillData.ID = skillConfig.ID;
        skillData.skillName = skillConfig.skillName;
        skillData.cdTime = skillConfig.cdTime;
        skillData.skillTime = skillConfig.skillTime;
        skillData.aniAction = skillConfig.aniAction;
        skillData.fx = skillConfig.fx;
        skillData.isCombo = skillConfig.isCombo == 1;
        skillData.isCollide = skillConfig.isCollide == 1;
        skillData.isBreak = skillConfig.isBreak == 1;
        skillData.dmgType = (DamageType)skillConfig.dmgType;
        List<int> skillMoveLst = new List<int>();
        string[] moveTemp = skillConfig.skillMoveLst.Split('|');
        foreach (string var in moveTemp)
        {
            if (string.IsNullOrEmpty(var))
            {
                break;
            }

            skillMoveLst.Add(int.Parse(var));
        }
        skillData.skillMoves = skillMoveLst;
        
        List<int> skillActLst = new List<int>();
        string[] actTemp = skillConfig.skillActionLst.Split('|');
        foreach (string var in actTemp)
        {
            skillActLst.Add(int.Parse(var));
        }
        skillData.skillActions = skillActLst;
        
        List<int> skillDamage = new List<int>();
        string[] damageTemp = skillConfig.skillDamageLst.Split('|');
        foreach (string var in damageTemp)
        {
            skillDamage.Add(int.Parse(var));
        }
        skillData.skillDamageLst = skillDamage;
        return skillData;
    }

    private void InitSkillConfig()
    {
        SkillsConfig skillsConfig = Resources.Load<SkillsConfig>("ResConfig/SkillsConfig");
        foreach (SkillConfig var in skillsConfig.Skill)
        {
            skillDic.Add(var.ID, SkillConfigToData(var));
        }
      
    }

    private void InitSkillMoveConfig()
    {
        SkillMovesConfig skillMovesConfig = Resources.Load<SkillMovesConfig>("ResConfig/SkillMovesConfig");
        foreach (SkillMoveConfig var in skillMovesConfig.SkillMove)
        {
            skillMoveDic.Add(var.ID,var);
        }
       
    }
    private void InitSkillActionConfig()
    {
        SkillActionsConfig skillActionsConfig = Resources.Load<SkillActionsConfig>("ResConfig/SkillActionsConfig");
        foreach (SkillActionConfig var in skillActionsConfig.SkillAction)
        {
            skillActionDic.Add(var.ID,var);
        }

       
    }

   

    public AutoGuideConfig GetAutoGuideConfig(int ID)
    {
        AutoGuideConfig autoGuideConfig = null;
        if (autoGuideDic.TryGetValue(ID, out autoGuideConfig))
        {
            return autoGuideConfig;
        }

        return null;
    }

    public StrongConfig GetStrongConfig(int pos, int starLv)
    {
        StrongConfig res = null;
        Dictionary<int, StrongConfig> configs;
        if (strongDic.TryGetValue(pos, out configs))
        {
            if (configs.ContainsKey(starLv))
            {
                res = configs[starLv];
            }
        }

        return res;
    }

   

    public SkillData GetSkillConfig(int ID)
    {
        SkillData skillData = null;
        skillDic.TryGetValue(ID, out skillData);
        return skillData;
    }

    public SkillMoveConfig GetSkillMoveConfig(int ID)
    {
        SkillMoveConfig skillMoveConfig = null;
        skillMoveDic.TryGetValue(ID, out skillMoveConfig);
        return skillMoveConfig;
    }

   
    public SkillActionConfig GetSkillActionConfig(int ID)
    {
        SkillActionConfig skillActionConfig = null;
        skillActionDic.TryGetValue(ID, out skillActionConfig);
        return skillActionConfig;
    }

    private List<ShopItem> _shopItems;
 

    public GameObject GetGameObject(string path, bool isCache = false)
    {
        GameObject prefab;
        prefabDic.TryGetValue(path, out prefab);
        if (prefab == null)
        {
            prefab = Resources.Load<GameObject>(path);
        }

        GameObject res = null;
        try
        {
            res = Instantiate(prefab);
            if (isCache)
            {
                prefabDic.Add(path, res);
            }
        }
        catch (Exception e)
        {
            NetCommon.Log(e + "资源路径错误,没有加载到PATH:" + path, NetLogType.Error);
        }

        return res;
    }

    public Sprite GetSprite(string path, bool isCache = false)
    {
        Sprite sprite;
        if (!spriteDic.TryGetValue(path, out sprite))
        {
            sprite = Resources.Load<Sprite>(path);
            if (isCache)
            {
                spriteDic.Add(path, sprite);
            }
        }

        return sprite;
    }


    //type:equip hurt hp or def
    public int EquipAccmulation(int pos, int startLv, int type)
    {
        int res = 0;
        Dictionary<int, StrongConfig> configs = null;
        if (strongDic.TryGetValue(pos, out configs))
        {
            StrongConfig strongConfig = null;
            for (int i = 0; i < startLv; i++)
            {
                if (configs.TryGetValue(i + 1, out strongConfig))
                {
                    switch (type)
                    {
                        case 1:
                            res += strongConfig.addhp;
                            break;
                        case 2:
                            res += strongConfig.addhurt;
                            break;
                        case 3:
                            res += strongConfig.adddef;
                            break;
                    }
                }
            }
        }

        return res;
    }

    private void Update()
    {
        //异步加载场景
        if (pgAction != null)
        {
            pgAction();
        }
    }
}