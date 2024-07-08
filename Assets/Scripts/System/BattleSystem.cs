using Battle;
using Battle.FSM;
using Common;
using Config;
using DarkGod.UIWindow;
using Protocal;
using Service;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace DarkGod.System
{
    public enum GameState
    {
        None,
        Begin,
        End
    }

    public class BattleSystem : SystemRoot<BattleSystem>
    {
        [SerializeField] public PlayerCtWnd _playerCtWnd;
        private BattleMgr battleMgr;
        [SerializeField] public GameOverWnd _gameOverWnd;
        private int currentLevelId;
        public GameState gameState; 
        public override void InitSystem()
        {
            base.InitSystem();
            NetCommon.Log("BattleSystem Init Done");
        }

        public void StartEnterBattle(int battleId)
        {
            GameObject go = new GameObject()
            {
                name = "BattleRoot"
            };
            battleMgr = go.AddComponent<BattleMgr>();
            go.transform.SetParent(GameRoot.Instance.transform);
            Wwc.Cfg.MapData data = resService.GetMapConfig(battleId);
            currentLevelId = battleId;
            battleMgr.Init(data);
            gameState = GameState.Begin;
        }

        public void SetPlayerMoveDir(Vector2 dir)
        {
            battleMgr.SetMoveDir(dir);
        }

        public void SetPlayerSkill(int index)
        {
            battleMgr.SetSkill(index);
        }

        public void OpenPlayerCtWnd()
        {
            _playerCtWnd.SetWndState(true);
        }

        public Vector2 GetCurrentDir()
        {
            return _playerCtWnd.currentDir;
        }
        public bool isCanSkill()
        {
            return battleMgr.isCanSkill();
        }

        public void KillMonster()
        {
            battleMgr.CurrentMonsterCount-=1;
        }

        public void GameOver(bool state)
        {
            if (state)
            {
                NetService.Instance.SendMsg(new NetMsg()
                {
                    cmd = (int)TransCode.LevelPassStateReq,
                    levelPassStateReq = new LevelPassStateReq()
                    {
                        lid = currentLevelId,
                        state = true
                    }
                });
            }

            _gameOverWnd.RefreshUI(state);
        }

        public void BackMainCity()
        {
            _gameOverWnd.SetWndState(false);
            battleMgr.BattleEnd();
            Destroy(battleMgr);
            GameRoot.Instance.dynamicWnd.Clear();
            _playerCtWnd.SetWndState(false);
            MainCitySys.Instance.EnterMainCity();
        }
    }
}