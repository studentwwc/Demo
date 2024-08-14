using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace battle.mgr
{
    public class MapMgr:MonoBehaviour
    {
        private BattleMgr _battleMgr;
        [SerializeField]
        private List<GameObject> doors;
        public void Init(BattleMgr battleMgr)
        {
            _battleMgr = battleMgr;
            LoadMonster();
        }

        public void OpenDoor(int index)
        {
            if (index < 0 || index >= doors.Count) return;
            doors[index].gameObject.SetActive(false);
        }

        public void LoadMonster(int monsterBatch=1)
        {
            Debug.Log("Game Will Create Monster Batch:"+monsterBatch);
            _battleMgr.LoadMonsterByBatch(monsterBatch);
        }
    }
}