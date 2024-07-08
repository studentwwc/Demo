using System;
using System.Collections.Generic;
using Wwc.Cfg;

namespace Config
{
    [Serializable]
    public class RoleConfig:BaseData<RoleConfig>
    {
        public string Name;
        public int Sex;
        public int Type;
        public string Des;
        public string Prop;
        public string DesIconAsset;
        public string IconAsset;
        public string PrefabAsset;
    }
}