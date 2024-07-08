using System;
using Config;
using Wwc.Cfg;

namespace Ws.Battle
{
    [Serializable]
    public class SkillConfig:BaseData<SkillConfig>
    {
        public string Name;
        public string Des;
        public int Cd;
        public int Action;
    }
}