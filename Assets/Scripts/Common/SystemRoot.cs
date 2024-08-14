using Service;
using UnityEngine;

namespace Common
{
    public class SystemRoot<T>:MonoBehaviour where T:SystemRoot<T>
    {
        private static T instance;

        public static T Instance
        {
            get { return instance; }
        }
        protected AudioService aduioService;
        protected ResService resService;
        protected NetService _netService;
        public virtual void InitSystem()
        {
            instance = this as T;
            aduioService=AudioService.Instance;
            resService=ResService.Instance;
            _netService=NetService.Instance;
        }
    }
}