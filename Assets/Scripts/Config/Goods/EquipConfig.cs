using System;
using System.Collections.Generic;

namespace Config
{
    [Serializable]
    public class EquipConfig:GoodsData
    {
        public string Name;
        public int EquipType;
        public int EquipAdaptType;
        public string Prop;
        public string Introduce;
        public string AssetIcon;
        public string AssetPrefab;
        
    }

}