using System;
using System.Collections.Generic;
using Battle.Entity;
using Battle.FSM;
using battle.mgr;
using Common;
using Wwc.Cfg;
using DarkGod.System;
using Protocal;
using Service;
using UnityEngine;

namespace Battle
{
    public class BattleMgr : MonoBehaviour
    {
        private SkillMgr _skillMgr;
        public PlayerEntity playerEntity;
        private StateMgr _stateMgr;
        private MapMgr _mapMgr;
        private MapData _mapConfig;
        private List<MonsterEntity> _monsterEntities=new List<MonsterEntity>();
        private int currentBatch = 0;
        private int maxBatch = 0;
        
        public void Init(MapData mapdata)
        {
            _mapConfig = mapdata;
            _skillMgr = transform.gameObject.AddComponent<SkillMgr>();
            _stateMgr = transform.gameObject.AddComponent<StateMgr>();
            _skillMgr.Init();
            _stateMgr.Init();
            ResService.Instance.ASyncLoadScene(mapdata.sceneName, () =>
            {
                AudioService.Instance.PlayBgAudio(Constant.AudioPathPrefix + Constant.AudioBgHuangYe);
                Transform mapRoot = GameObject.FindGameObjectWithTag("MapRoot").transform;
                _mapMgr = mapRoot.GetComponent<MapMgr>();
                _mapMgr.Init(this);
                mapRoot.position = Vector3.zero;
                mapRoot.localScale = Vector3.one;
                var camTran = Camera.main.transform;
                camTran.localPosition = mapdata.mainCamPos;
                camTran.localEulerAngles = mapdata.mainCamRote;
                //TODO:设置人物参数
                LoadPlayerInit(mapdata);
                SetActiveCurrentMonster();
                playerEntity.Idle();
                
                BattleSystem.Instance.OpenPlayerCtWnd();
            });
        }

        private void Update()
        {
            for (int i = 0; i < _monsterEntities.Count; i++)
            {
                _monsterEntities[i].TickAiLogic();
            }
        }

        private void LoadPlayerInit(MapData data)
        {
            GameObject player = ResService.Instance.GetGameObject(Constant.AssassinBattlePath);
            player.transform.localPosition = data.playerBornPos;
            player.transform.localEulerAngles = data.playerBornRote;
            player.name = "Assain";
            PlayerData pd = GameRoot.Instance.PlayerData;
            BattleData bd = new BattleData()
            {
                hp=pd.hp,
                ad= pd.ad,
                ap= pd.ap,
                addef= pd.addef,
                apdef= pd.apdef,
                pierce= pd.pierce,
                dodge= pd.dodge,
                critical= pd.critical,
                
            };
            playerEntity = new PlayerEntity()
            {
                stateMgr = _stateMgr,
                skillMgr = _skillMgr,
                battleMgr = this,
            };
            playerEntity.entityName = player.name;
            playerEntity.SetBattleData(bd);
            playerEntity.audioSource = player.GetComponent<AudioSource>();
            PlayerController _playerController = player.GetComponent<PlayerController>();
            playerEntity.SetController(_playerController);
            _playerController.Init();
        }

        private int currentMonsterCount = 0;

        public int CurrentMonsterCount
        {
            get
            {
             return   currentMonsterCount;
            }
            set
            {
                currentMonsterCount=value;
                if (currentMonsterCount <= 0)
                {
                    if (currentBatch == maxBatch)
                    {
                        BattleSystem.Instance.GameOver(true);
                        return;
                    }
                    _mapMgr.OpenDoor(currentBatch-1);
                }
            }
        }

        public void LoadMonsterByBatch(int batch)
        {
            currentBatch = batch;
            Debug.Log("Begin Create Monster Batch:"+batch);
            List<MonsterData>monsters =_mapConfig.monsters;
            for (int i = 0; i <monsters.Count; i++)
            {
                MonsterData monsterData = monsters[i];
                maxBatch = Math.Max(maxBatch, monsterData.batch);
                if (batch == monsterData.batch)
                {
                    GameObject go = ResService.Instance.GetGameObject(monsterData._monsterConfig.resPath);
                    go.transform.localPosition = monsterData.pos;
                    go.transform.localEulerAngles = monsterData.rot;
                    go.name = monsterData._monsterConfig.mName+i;
                    MonsterController monsterController= go.GetComponent<MonsterController>();
                    monsterController.Init();
                    MonsterEntity monsterEntity = new MonsterEntity()
                    {
                        stateMgr = _stateMgr,
                        skillMgr = _skillMgr,
                        battleMgr = this,
                    };
                    monsterEntity.attackDis = monsterData._monsterConfig.atkDis;
                    monsterEntity.skillID = monsterData._monsterConfig.skillID;
                    monsterEntity.entityName = go.name;
                    monsterEntity.SetController(monsterController);
                    monsterEntity.lv = monsterData.lv;
                    monsterEntity.SetBattleData(monsterData.battleData);
                    monsterEntity.CurrentHp = monsterEntity.Props.hp;
                    _monsterEntities.Add(monsterEntity);
                    monsterEntity.SetActive();
                    GameRoot.Instance.SetItemEntityHp(go.name,monsterEntity.CurrentHp,monsterController.hpTrans);
                    Debug.Log("Create Monster Position:"+ monsterData.pos);
                    currentMonsterCount++;
                }
            }
        }

        public void SetActiveCurrentMonster()
        {
            TimerService.Instance.AddTimerTask((tid) =>
            {
                for (var i = 0; i < _monsterEntities.Count; i++)
                {
                    MonsterEntity monsterEntity = _monsterEntities[i];
                    monsterEntity.SetActive();
                    monsterEntity.Born();
                }
            },500);
        }

        public void SetMoveDir(Vector2 dir)
        {
            if(!playerEntity.isCanMove)return;
            if (dir != Vector2.zero)
            {
                playerEntity.Move();
                playerEntity.SetMoveDir(dir);
            }
            else
            {
                playerEntity.Idle();
                playerEntity.SetMoveDir(dir);
            }
        }

        #region About Skill

        public void SetSkill(int index)
        {
            switch (index)
            {
                case 0:
                    SetNormalSkill();
                    break;
                case 1:
                    SetSkill1();
                    break;
                case 2:
                    SetSkill2();
                    break;
                case 3:
                    SetSkill3();
                    break;
            }
        }

        private int[] attacks = {111,112,113,114,115};
        private int index = 0;
        public float preTime = 0;
        private void SetNormalSkill()
        {
            NetCommon.Log("普通攻击");
            if (playerEntity.currentState == State.Idle || playerEntity.currentState == State.Move)
            {
                index = 0;
                preTime = Time.time;
                playerEntity.Attack(attacks[index]);
            }else if (playerEntity.currentSkill.isCombo)
            {
                if (Time.time - preTime <= Constant.comboSpace)
                {
                    index= (index+1) %attacks.Length;
                    playerEntity.comboQueue.Enqueue(attacks[index]);
                    preTime = Time.time;
                }
                else
                {
                    index = 0;
                    preTime = Time.time;
                    playerEntity.Attack(attacks[index]);
                }
            }
        }

        private void SetSkill1()
        {
            NetCommon.Log("技能一");
            playerEntity.isCanSkill = false;
            playerEntity.Attack(101);
            
        }

        private void SetSkill2()
        {
            NetCommon.Log("技能二");
            playerEntity.isCanSkill = false;
            playerEntity.Attack(102);
        }

        private void SetSkill3()
        {
            NetCommon.Log("技能三");
            playerEntity.isCanSkill = false;
            playerEntity.Attack(103);
        }

        #endregion

        public Vector2 GetCurrentDir()
        {
            return BattleSystem.Instance.GetCurrentDir();
        }

        public bool isCanSkill()
        {
            return playerEntity.isCanSkill;
        }

        public List<MonsterEntity> GetAllMonster()
        {
            return _monsterEntities;
        }

        public void RemoveMonster(EntityBase entityBase)
        {
            if (_monsterEntities.Contains(entityBase as MonsterEntity))
            {
                _monsterEntities.Remove(entityBase as MonsterEntity);
                GameRoot.Instance.dynamicWnd.ItemEntityHpInvalid(entityBase.entityName);
            }
        }

        // public void SetState(State state)
        // {
        //     switch (state)
        //     {
        //         case State.Idle:
        //             playerEntity.Idle();
        //             break;
        //         case State.Move:
        //             playerEntity.Move();
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(state), state, null);
        //     }
        // }
        public void BattleEnd()
        {
            _skillMgr = null;
            _mapConfig = null;
            _mapMgr = null;
            _stateMgr = null;
            _monsterEntities.Clear();
            _monsterEntities = null;
        }
    }
}