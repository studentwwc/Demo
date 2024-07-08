using UnityEngine;

namespace Config
{
    public class RoleData
    {
        public RoleConfig RoleConfig;
        public Sprite Sprite;

        public RoleData(Sprite sprite,RoleConfig roleConfig)
        {
            Sprite = sprite;
            RoleConfig = roleConfig;
        }
    }
}