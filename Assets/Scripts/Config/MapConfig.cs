using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wwc.Cfg
{
    [Serializable]
    public class MapConfig:BaseData<MapConfig>
    {
        public string mapName;
        public string sceneName;
        public int power;
        public string mainCamPos;
        public string mainCamRote;
        public string playerBornPos;
        public string playerBornRote;
        public string monsters;
    }

    public class MapData:BaseData<MapConfig>
    {
        public string mapName;
        public string sceneName;
        public int power;
        public Vector3 mainCamPos;
        public Vector3 mainCamRote;
        public Vector3 playerBornPos;
        public Vector3 playerBornRote;
        public List<MonsterData> monsters;
    }

    [Serializable]
    public class AutoGuideConfig : BaseData<AutoGuideConfig>
    {
        public int npcID;
        public string dilogArr;
        public int actID;
        public int coin;
        public int exp;
    }
    [Serializable]
    public class StrongConfig : BaseData<AutoGuideConfig>
    {
        public int pos;
        public int starlv;
        public int addhp;
        public int addhurt;
        public int adddef;
        public int minlv;
        public int coin;
        public int crystal;
    }
    [Serializable]
    public class TaskRewardConfig : BaseData<TaskRewardConfig>
    {
        public string taskName;
        public int count;
        public int exp;
        public int coin;
    }
    [Serializable]
    public class TaskRewardData:BaseData<TaskRewardData>
    {
        public int prog;
        public bool isGeted;
    }
    [Serializable]
    public class SkillData:BaseData<MapConfig>
    {
        public string skillName;
        public float skillTime;
        public DamageType dmgType;
        public int aniAction;
        public int cdTime;
        public bool isCollide;
        public bool isBreak;
        public bool isCombo;
        public string fx;
        public List<int> skillMoves;
        public List<int> skillActions;
        public List<int> skillDamageLst;
    }
    [Serializable]
    public class SkillMoveConfig:BaseData<MapConfig>
    {
       
        public int moveTime;
        public float moveDis;
        public int delayTime;
    }
    [Serializable]
    public class SkillActionConfig : BaseData<MapConfig>
    {
        public int delayTime;
        public float radius;
        public float angle;
    }
    [Serializable]
    public class MonsterConfig : BaseData<MapConfig>
    {
        public string mName;
        public string resPath;
        public int skillID;
        public float atkDis;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;
        public int pierce;
        public int critical;
    }
    [Serializable]
    public class MonsterData: BaseData<MapConfig>
    {
        public int batch;
        public int index;
        public MonsterConfig _monsterConfig;
        public BattleData battleData;
        public Vector3 pos;
        public Vector3 rot;
        public int lv;
    }
    [Serializable]
    public class BattleData
    {
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;
        public int pierce;
        public int critical;
        
    }

    [Serializable]
    public class RenameConfig:BaseData<RenameConfig>
    {
        public string surname;
        public string man;
        public string woman;
    }

    [Serializable]
    public class SkillConfig : BaseData<SkillConfig>
    {
        public string skillName;
        public float skillTime;
        public int dmgType;
        public int aniAction;
        public int cdTime;
        public int isCollide;
        public int isBreak;
        public int isCombo;
        public string fx;
        public string skillMoveLst;
        public string skillActionLst;
        public string skillDamageLst;
    }

    public class Icp : IComparer<TaskRewardData>
    {
        public int Compare(TaskRewardData x, TaskRewardData y)
        {
            return x.isGeted.CompareTo(y.isGeted);
        }
    }
  
    public class BaseData<T>
    {
        public int ID;
    }

    public enum DamageType
    {
        AD=1,
        AP=2,
        
    }
}