using System.Collections;
using System.Collections.Generic;
using Config;
using UnityEngine;

namespace Wwc.Cfg
{
    public static class CfgUtility
    {
        public static Dictionary<int, EquipConfig> EquipConfigs=new Dictionary<int, EquipConfig>();
        public static Dictionary<int, PropConfig> PropConfigs = new Dictionary<int, PropConfig>();
        public static Dictionary<int, RoleConfig> RoleConfigs = new Dictionary<int, RoleConfig>();
        public static void SetUp()
        {
            //初始化配置文件
            InitGoodsConfig();
            InitRolesConfig();
        }

        public static void InitGoodsConfig()
        {
            GoodsConfig goodsConfig= Resources.Load<GoodsConfig>("ResConfig/GoodsConfig");
            for (int i = 0; i < goodsConfig.Equip.Count; i++)
            {
                EquipConfigs.Add(goodsConfig.Equip[i].ID,goodsConfig.Equip[i]);
            }
            for (int i = 0; i < goodsConfig.Prop.Count; i++)
            {
                PropConfigs.Add(goodsConfig.Prop[i].ID,goodsConfig.Prop[i]);
            }
        }

        public static void InitRolesConfig()
        {
            RolesConfig rolesConfig= Resources.Load<RolesConfig>("ResConfig/RolesConfig");
            for (int i = 0; i < rolesConfig.Role.Count; i++)
            {
                RoleConfigs.Add(rolesConfig.Role[i].ID,rolesConfig.Role[i]);
            }
        }
    }

}