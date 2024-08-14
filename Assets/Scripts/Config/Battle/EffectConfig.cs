using System;
using Config;
using Wwc.Cfg;

namespace Ws.Battle
{
    [Serializable]
    public class EffectConfig:BaseData<EffectConfig>
    {
        public int Type;
        public int Part;
        public string StartPos;
        public string Prefab;
    }
}