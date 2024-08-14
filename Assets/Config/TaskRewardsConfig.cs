using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wwc.Cfg;

[ExcelAsset]
public class TaskRewardsConfig : ScriptableObject
{
	public List<TaskRewardConfig> TaskReward; // Replace 'EntityType' to an actual type that is serializable.
}
