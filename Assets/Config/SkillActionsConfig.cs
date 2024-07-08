using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wwc.Cfg;

[ExcelAsset]
public class SkillActionsConfig : ScriptableObject
{
	public List<SkillActionConfig> SkillAction; // Replace 'EntityType' to an actual type that is serializable.
}
