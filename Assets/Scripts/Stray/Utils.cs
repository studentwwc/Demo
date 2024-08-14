using System.Collections.Generic;
using UnityEngine;
using Wwc.Cfg;

namespace Stray
{
    public class Utils
    {
        public static Transform GetTransformByName(Transform transform,string name)
        {
         
            Transform res = transform.Find(name);
            if (res!=null)
            {
                return res;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                res= GetTransformByName(transform.GetChild(i),name);
                if (res != null)
                {
                    return res;
                }
            }

            return res;
        }

        public static Vector3 StringToVector3(string str)
        {
            string[] ver=str.Split(',');
            Vector3 temp=Vector3.zero;
            for (int i = 0; i < ver.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        temp.x = float.Parse(ver[0]);
                        break;
                    case 1:
                        temp.y = float.Parse(ver[1]);
                        break;
                    case 2:
                        temp.z = float.Parse(ver[2]);
                        break;
                }
            }

            return temp;
        }
        
    }
}