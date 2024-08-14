using Server.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Xml;

namespace Server._01Service
{
    public class CfgService : Singleton<CfgService>
    {
        private Dictionary<int, AutoGuideConfig> taskDic;
        //StrongDic
        public Dictionary<int, Dictionary<int, StrongConfig>> strongDic =
            new Dictionary<int, Dictionary<int, StrongConfig>>();
        private Dictionary<int, TaskRewardConfig> taskRewardDic = new Dictionary<int, TaskRewardConfig>();
        private Dictionary<int, MapConfig> mapConfigDic = new Dictionary<int, MapConfig>();
        public void Init()
        {
            taskDic = new Dictionary<int, AutoGuideConfig>();
            InitConfig(@"D:\mystudy\unityProgram\DarkGod\Assets\Resources\ResConfig\AutoGuide.xml", InitTaskCfg);
            InitConfig(@"D:\mystudy\unityProgram\DarkGod\Assets\Resources\ResConfig\strong.xml", InitStrongConfig);
            InitConfig(@"D:\mystudy\unityProgram\DarkGod\Assets\Resources\ResConfig\taskreward.xml", InitTaskRewardConfig);
            InitConfig(@"D:\mystudy\unityProgram\DarkGod\Assets\Resources\ResConfig\map.xml", InitMapConfig);
            NetCommon.Log("CfgServer Init Done");
        }
        private void InitConfig(string path, Action<XmlNodeList, int> config)
        {
          
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("root").ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    XmlElement xmlElement = xmlNodeList[i] as XmlElement;
                    if (xmlElement.GetAttributeNode("ID") == null)
                    {
                        continue;
                    }

                    int ID = Convert.ToInt32(xmlElement.GetAttributeNode("ID").InnerText);
                    config(xmlNodeList[i].ChildNodes, ID);
                }
           }
        private void InitTaskCfg(XmlNodeList xmlNodeList, int ID)
        {
           
                AutoGuideConfig autoGuideConfig = new AutoGuideConfig();
                autoGuideConfig.ID = ID;
                foreach (XmlElement xml in xmlNodeList)
                {
                    switch (xml.Name)
                    {
                        case "coin":
                            {
                                autoGuideConfig.coin = int.Parse(xml.InnerText);
                            }
                            break;
                        case "exp":
                            {
                                autoGuideConfig.exp = int.Parse(xml.InnerText);
                            }
                            break;
                    }
                }
                taskDic.Add(ID, autoGuideConfig);

        }
        private void InitStrongConfig(XmlNodeList xmlNodeList, int ID)
        {
            StrongConfig strongConfig = new StrongConfig();
            strongConfig.ID = ID;
            foreach (XmlElement xml in xmlNodeList)
            {
                int val = int.Parse(xml.InnerText);
                switch (xml.Name)
                {
                    case "pos":
                        strongConfig.pos = val;
                        break;
                    case "starlv":
                        strongConfig.starlv = val;
                        break;
                    case "addhp":
                        {
                            strongConfig.addhp = val;
                        }
                        break;
                    case "addhurt":
                        {
                            strongConfig.addhurt = val;
                        }
                        break;
                    case "adddef":
                        {
                            strongConfig.adddef = val;
                        }
                        break;
                    case "minlv":
                        strongConfig.minlv = val;
                        break;
                    case "coin":
                        strongConfig.coin = val;
                        break;
                    case "crystal":
                        strongConfig.crystal = val;
                        break;

                }
            }

            Dictionary<int, StrongConfig> configs;
            if (!strongDic.TryGetValue(strongConfig.pos, out configs))
            {
                configs = new Dictionary<int, StrongConfig>();
                strongDic.Add(strongConfig.pos, configs);
            }
            configs.Add(strongConfig.starlv, strongConfig);

        }
        private void InitTaskRewardConfig(XmlNodeList xmlNodeList, int ID)
        {
            TaskRewardConfig taskRewardConfig = new TaskRewardConfig();
            taskRewardConfig.ID = ID;
            foreach (XmlElement xml in xmlNodeList)
            {
                switch (xml.Name)
                {
                    case "count":
                        taskRewardConfig.count = int.Parse(xml.InnerText);
                        break;
                    case "exp":
                        taskRewardConfig.exp = int.Parse(xml.InnerText);
                        break;
                    case "coin":
                        taskRewardConfig.coin = int.Parse(xml.InnerText);
                        break;
                }
            }
            taskRewardDic.Add(ID, taskRewardConfig);
        }
        private void InitMapConfig(XmlNodeList xmlNodeList, int ID)
        {
            MapConfig mapConfig = new MapConfig();
            mapConfig.ID = ID;
            foreach (XmlElement xml in xmlNodeList)
            {
                switch (xml.Name)
                {
                    case "power":
                        mapConfig.power = int.Parse(xml.InnerText);
                        break;
                }
            }
            mapConfigDic.Add(ID, mapConfig);
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
        public AutoGuideConfig getTaskConfig(int ID)
        {
            return taskDic[ID];
        }
        public TaskRewardConfig GetTaskRewardConfig(int ID)
        {
            TaskRewardConfig res = null;
            taskRewardDic.TryGetValue(ID, out res);
            return res;
        }
       
        public MapConfig GetMapConfig(int ID)
        {
            MapConfig mapConfig = null;
            mapConfigDic.TryGetValue(ID, out mapConfig);
            return mapConfig;
        }
    }






    public class AutoGuideConfig : BaseData<AutoGuideConfig>
    {
        public int npcID;
        public string dilogArr;
        public int actID;
        public int coin;
        public int exp;
    }
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
    public class TaskRewardConfig : BaseData<TaskRewardConfig>
    {
        public int count;
        public int exp;
        public int coin;
    }
    public class TaskRewardData : BaseData<TaskRewardData> {
        public int prog;
        public bool isGet;
    }
    public class MapConfig : BaseData<MapConfig>
    {
        public int power;
    }
    public class BaseData<T>
    {
        public int ID;
    }
}
