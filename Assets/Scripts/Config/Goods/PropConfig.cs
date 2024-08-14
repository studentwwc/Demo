using System;

namespace Config
{
    [Serializable]
    public class PropConfig:GoodsData
    {
        public string Name;
        public int Type;
        public int Count;
    }
}