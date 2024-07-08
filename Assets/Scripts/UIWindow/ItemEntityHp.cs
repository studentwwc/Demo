using System;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkGod.UIWindow
{
    public class ItemEntityHp:MonoBehaviour
    {
        [SerializeField] private Image fgGray;
        [SerializeField] private Image fgRed;

        [SerializeField] private TMP_Text txtCritical;
        [SerializeField] private Animation animCritical;
        
        [SerializeField] private TMP_Text txtDodge;
        [SerializeField] private Animation animDodge;
        
        [SerializeField] private TMP_Text txtDamage;
        [SerializeField] private Animation animDamage;

        private int hp;
        private int currentHp;
        private int pre;
        private float screenRate;
        public Transform hpTrans;
        

        private int HP
        {
            get => hp;
            set
            {
                hp = value;
                currentHp = hp;
            }
        }

        public void Init(int hp,Transform hpTrans)
        {
            this.hpTrans = hpTrans;
            HP = hp;
            pre = hp;
            fgGray.fillAmount = 1;
            fgRed.fillAmount = 1;
            txtCritical.alpha = 0;
            txtDamage.alpha = 0;
            txtDodge.alpha = 0;
        }

        public void SetCritical(int cri)
        {
            animCritical.Stop();
            txtCritical.text = "暴击 " + cri;
            animCritical.Play();
        }
        public void SetDodge()
        {
            
            animDodge.Stop();
            txtCritical.text = "闪避";
            animDodge.Play();
        }
        public void SetDamage(int hurt)
        {
            animDamage.Stop();
            txtCritical.text = "- "+ hurt;
            animDamage.Play();
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.A))
            // {
            //     SetCritical(666);
            // }if (Input.GetKeyDown(KeyCode.D))
            // {
            //     SetDamage(123);
            // }if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     SetDodge();
            // }
            screenRate = Screen.height*1.0f / Constant.ScreenStandardHeight;
            Vector3 pos = Camera.main.WorldToScreenPoint(hpTrans.position);
            transform.position = pos * screenRate;
            if (pre !=currentHp)
            {
                BlendHp();
            }
        }

        
        public void SetHpChange(int oldHp, int newHp)
        {
            
            pre = oldHp;
            currentHp = newHp;
            fgRed.fillAmount = currentHp*1.0f / hp;
        }

        private void BlendHp()
        {
            if (Mathf.Abs(pre - currentHp) < Time.deltaTime*1000)
            {
                pre = currentHp;
            }
            else
            {
                pre -= (int)(Time.deltaTime*1000);
            }
            fgGray.fillAmount = pre*1.0f / hp;
            //NetCommon.Log(pre+"    "+currentHp);
        }

    }
}