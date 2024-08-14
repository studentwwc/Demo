using System.Collections.Generic;
using Common;
using Config;
using DarkGod.System;
using Protocal;
using Stray;
using UnityEngine;
using UnityEngine.UI;
using Wwc.Cfg;

namespace DarkGod.UIWindow
{
    public class TaskRewardWnd:WindowRoot
    {
        private PlayerData pd;
        private List<TaskRewardData> taskRewardList = new List<TaskRewardData>();
        [SerializeField] private Transform taskItemParentTrans;
        protected override void InitWnd()
        {
            base.InitWnd();
            RefreshUI();
        }

        public void OnClickClose()
        {
            MainCitySys.Instance.CloseUITaskReward();
        }

        public void RefreshUI()
        {
            pd = GameRoot.Instance.PlayerData;
            string []taskRewards = pd.taskReward;   
            taskRewardList.Clear();
            for (int i = 0; i < taskRewards.Length; i++)
            {
                string[]temps=taskRewards[i].Split('|');
                taskRewardList.Add(new TaskRewardData()
                {
                    ID = int.Parse(temps[0]),
                    prog = int.Parse(temps[1]),
                    isGeted = int.Parse(temps[2])==1,
                });
            }
            taskRewardList.Sort(new Icp());
            //删除所有子节点
            for (int i = 0; i < taskItemParentTrans.childCount; i++)
            {
                Destroy(taskItemParentTrans.GetChild(i).gameObject);
            }

            for (int i = 0; i < taskRewardList.Count; i++)
            {
                TaskRewardConfig config = _resService.GetTaskRewardConfig(taskRewardList[i].ID);
                GameObject go = _resService.GetGameObject(Constant.taskItemPath);
                go.transform.SetParent(taskItemParentTrans);
                //设置Item的值
                SetText(Utils.GetTransformByName(go.transform, "txtName"),config.taskName);
                SetText(Utils.GetTransformByName(go.transform, "txtProg"),taskRewardList[i].prog+"/"+config.count);
                SetText(Utils.GetTransformByName(go.transform, "txtExp"),config.exp);
                SetText(Utils.GetTransformByName(go.transform, "txtCoin"),config.coin);
                Utils.GetTransformByName(go.transform, "imgRate").GetComponent<Image>().fillAmount =
                    taskRewardList[i].prog*1.0f / config.count;
                Image isGeted = Utils.GetTransformByName(go.transform, "imgGeted").GetComponent<Image>();
                Image imgButtom = Utils.GetTransformByName(go.transform, "btnBox").GetComponent<Image>();
                Button btnBox = imgButtom.transform.GetComponent<Button>();
                btnBox.onClick.AddListener(() =>
                {
                    OnClickRewardCallBack(config.ID);
                });
                if (taskRewardList[i].isGeted)
                {
                    isGeted .enabled = true;
                    SetSprite(imgButtom,Constant.taskRewardNotCan);
                    btnBox.interactable = false;
                }
                else
                {
                    isGeted.enabled = false;
                    if (taskRewardList[i].prog == config.count)
                    {
                        SetSprite(imgButtom,Constant.taskRewardCan);
                        btnBox.interactable = true;
                        
                    }
                    else
                    {
                        SetSprite(imgButtom,Constant.taskRewardNotCan);
                        btnBox.interactable = true;
                    }
                }
            }
        }

        public void OnClickRewardCallBack(int ID)
        {
            MainCitySys.Instance.TaskRewardReq(ID);
        }
    }
}