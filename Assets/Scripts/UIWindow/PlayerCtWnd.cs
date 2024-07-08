using System;
using Common;
using DarkGod.System;
using Protocal;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DarkGod.UIWindow
{
    public class PlayerCtWnd:WindowRoot
    {
        [SerializeField] private Image imgTouch;
        [SerializeField] private Image imgDirBg;
        [SerializeField] private Image imgDirPointer;
        
        [SerializeField] private TMP_Text txtHp;
        [SerializeField] private Image imgHp;
        [SerializeField] private TMP_Text txtLv;
        [SerializeField] private GridLayoutGroup _gridLayout;
        [SerializeField] private TMP_Text txtExp;
        private Vector2 defaultImgDirBgPostion=Vector2.zero;
        private Vector2 startPointerPos=Vector2.zero;
        private int oprationDistance = 0;
        public Vector2 currentDir;

        private int Hp;
       
        protected override void InitWnd()
        {
            base.InitWnd();
            SetActive(imgDirPointer,false);
            RegisterImgTouch();
            RefreshUI();
            defaultImgDirBgPostion = imgDirBg.transform.position;
            oprationDistance = (int)( Constant.ScreenOpDis*1.0f*Screen.height / Constant.ScreenStandardHeight);
           
            skill1CdTime = _resService.GetSkillConfig(101).cdTime/1000.0f;
            imgSkill1Cd.fillAmount = 0;
            txtSkill1Cd.text = "";
            
            skill2CdTime = _resService.GetSkillConfig(102).cdTime/1000.0f;
            imgSkill2Cd.fillAmount = 0;
            txtSkill2Cd.text = "";
            
            skill3CdTime = _resService.GetSkillConfig(103).cdTime/1000.0f;
            imgSkill3Cd.fillAmount = 0;
            txtSkill3Cd.text = "";

            Hp = GameRoot.Instance.PlayerData.hp;
        }

        public void RefreshUI()
        {
            PlayerData pd = GameRoot.Instance.PlayerData;
            SetText(txtLv, pd.lv);
            int exp = (int)(Math.Round(pd.exp * 1.0 / NetCommon.GetExpLimit(pd.lv), 2) * 100);
            SetText(txtExp, exp + "%");

            #region Exp

            //经验条自适应
            float ratio = Screen.height * 1.0f / Constant.ScreenStandardHeight;
            float screenWidth = Constant.ScreenStandardWidth / ratio;
            float expCell = (screenWidth - 180) / 10.0f;
            _gridLayout.cellSize = new Vector2(expCell, 12);
            //设置经验条的值
            for (int i = 0; i < _gridLayout.transform.childCount; i++)
            {
                if (i < exp / 10)
                {
                    _gridLayout.transform.GetChild(i).GetComponent<Image>().fillAmount = 1;
                }
                else if (i == exp / 10)
                {
                    _gridLayout.transform.GetChild(i).GetComponent<Image>().fillAmount = (exp % 10) / 10.0f;
                }
                else
                {
                    _gridLayout.transform.GetChild(i).GetComponent<Image>().fillAmount = 0;
                }
            }

            #endregion
        }

        #region RegisetEvent

        public void RegisterImgTouch()
        {
            OnClickDownPEListener(imgTouch.gameObject, (PointerEventData ped) =>
            {
                imgDirBg.transform.position = ped.position;
                imgDirPointer.transform.position = ped.position;
                SetActive(imgDirPointer);
                startPointerPos = ped.position;
            });
            OnClickUpPEListener(imgTouch.gameObject, (ped) =>
            {
                SetActive(imgDirPointer, false);
                imgDirBg.transform.position = defaultImgDirBgPostion;
                imgDirPointer.transform.position = Vector3.zero;
                currentDir=Vector2.zero;
                //playerCt.Dir = Vector2.zero;
                //playerCt.SetBlend(Constant.playerIdleBlend);
                BattleSystem.Instance.SetPlayerMoveDir(Vector2.zero);


            });
            OnDragPEListener(imgTouch.gameObject, (ped) =>
            {
                Vector2 dir = ped.position - startPointerPos;
                if (dir.magnitude < oprationDistance)
                {
                    imgDirPointer.transform.position = ped.position;
                }
                else
                {
                    Vector2 tar = Vector2.ClampMagnitude(dir, oprationDistance);
                    imgDirPointer.transform.position = startPointerPos + tar;
                }
                //MainCitySys.Instance.SetDir(dir);
                currentDir = dir.normalized;
                BattleSystem.Instance.SetPlayerMoveDir(currentDir);
            });
        }

        #endregion

        #region ClickEvent
        public void OnClickNormalSkill()
        {
            BattleSystem.Instance.SetPlayerSkill(0);
        }

        #region skill1

        [SerializeField] private Image imgSkill1Cd;
        [SerializeField] private TMP_Text txtSkill1Cd;
        private bool isSkill1 = false;
        private float skill1CdTime;
        private float remainSkill1CdTime;
        public void OnClickSkill1()
        {
            if (!isSkill1&&BattleSystem.Instance.isCanSkill())
            {
                BattleSystem.Instance.SetPlayerSkill(1);
                isSkill1 = true;
                remainSkill1CdTime = skill1CdTime;
            }
        }

        #endregion

        #region skill2
        [SerializeField] private Image imgSkill2Cd;
        [SerializeField] private TMP_Text txtSkill2Cd;
        private bool isSkill2 = false;
        private float skill2CdTime;
        private float remainSkill2CdTime;
        public void OnClickSkill2()
        {
            if (!isSkill2&&BattleSystem.Instance.isCanSkill())
            {
                BattleSystem.Instance.SetPlayerSkill(2);
                isSkill2 = true;
                remainSkill2CdTime = skill2CdTime;
            }
        }

        #endregion

        #region skill3

        [SerializeField] private Image imgSkill3Cd;
        [SerializeField] private TMP_Text txtSkill3Cd;
        private bool isSkill3 = false;
        private float skill3CdTime;
        private float remainSkill3CdTime;
        public void OnClickSkill3()
        {
            if (!isSkill3&&BattleSystem.Instance.isCanSkill())
            {
                BattleSystem.Instance.SetPlayerSkill(3);
                isSkill3 = true;
                remainSkill3CdTime = skill3CdTime;
            }
        }
        #endregion

        public void OnClickReSetSkill()
        {
            _resService.ReSetSkillDic();
        }

        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnClickNormalSkill();
            }

            if (isSkill1)
            {
                remainSkill1CdTime -= Time.deltaTime;
                txtSkill1Cd.text = ((int)remainSkill1CdTime+1).ToString();
                if (remainSkill1CdTime <= 0)
                {
                    remainSkill1CdTime = 0;
                    isSkill1 = false;
                    txtSkill1Cd.text ="";
                }
                imgSkill1Cd.fillAmount = remainSkill1CdTime / skill1CdTime;
            }
            if (isSkill2)
            {
                remainSkill2CdTime -= Time.deltaTime;
                txtSkill2Cd.text = ((int)remainSkill2CdTime+1).ToString();
                if (remainSkill2CdTime <= 0)
                {
                    remainSkill2CdTime = 0;
                    isSkill2 = false;
                    txtSkill2Cd.text ="";
                }
                imgSkill2Cd.fillAmount = remainSkill2CdTime / skill2CdTime;
            }
            if (isSkill3)
            {
                remainSkill3CdTime -= Time.deltaTime;
                txtSkill3Cd.text = ((int)remainSkill3CdTime+1).ToString();
                if (remainSkill3CdTime <= 0)
                {
                    remainSkill3CdTime = 0;
                    isSkill3 = false;
                    txtSkill3Cd.text ="";
                }
                imgSkill3Cd.fillAmount = remainSkill3CdTime / skill3CdTime;
            }
        }

        public void SetSelfHp(int currentHp)
        {
            SetText(txtHp,currentHp+"/"+Hp);
            imgHp.fillAmount = currentHp * 1.0f / Hp;
        }
    }
}