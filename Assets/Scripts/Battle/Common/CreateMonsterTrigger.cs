using System;
using battle.mgr;
using UnityEngine;

namespace Battle.Common
{
    public class CreateMonsterTrigger:MonoBehaviour
    {
        [SerializeField] private MapMgr _mapMgr;
        [SerializeField] private int batch;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                if (_mapMgr == null)
                {
                    _mapMgr = GetComponentInParent<MapMgr>();
                }
                _mapMgr.LoadMonster(batch);
                Destroy(this.gameObject);
            }
        }
    }
}