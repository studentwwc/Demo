using System;
using Common;
using DarkGod.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkGod.UIWindow
{
    public class GameOverWnd:WindowRoot
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button gameContinue;

        private void Awake()
        {
            gameContinue.onClick.AddListener(OnGameContinue);
        }

        public void RefreshUI(bool state)
        {
            if (state)
            {
                _text.text = "胜利";
            }
            else
            {
                _text.text = "失败";
            }
            SetWndState();
        }

        public void OnGameContinue()
        {
            BattleSystem.Instance.BackMainCity();
        }

    }
}