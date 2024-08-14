using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkGod.UIWindow
{
    public class LoadingWnd:WindowRoot
    {
        public TMP_Text tips;
        public Image loadingFg;
        public Image loadingPoint;
        public TMP_Text proportion;

        private float loadingStartPos;

        protected override void InitWnd()
        {
            base.InitWnd();
            loadingStartPos =- loadingFg.rectTransform.sizeDelta.x/2;
            SetText(tips,"Tips:带有霸体状态的技能在施放时可以规避控制");
            SetText(proportion,0+"%");
            loadingFg.fillAmount = 0;
            loadingPoint.rectTransform.localPosition = new Vector3(loadingStartPos,0,0);
        }

        public void SetProgress(float pg)
        {
            loadingFg.fillAmount = pg;
            SetText(proportion, (int)(pg * 100) + "%");
            loadingPoint.rectTransform.localPosition = new Vector3(-loadingStartPos * 2 * pg+loadingStartPos, 0, 0);
        }
    }
}