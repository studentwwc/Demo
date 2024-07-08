using System;
using Config;
using Wwc.Cfg;

namespace Ws.Battle
{
    [Serializable]
    public class ActionConfig:BaseData<ActionConfig>
    {
        public float Time;
        public float PreTime;
        public int DamageType;
        public int BaseMagicDamage;
        public int BasePhysicalDamage;
        public int DamagePhysicalProportion;
        public int DamageMagicProportion;
        public string MoveDir;
        public int MoveDistance;
        public float MoveTime;
        public int Effect;
    }
}