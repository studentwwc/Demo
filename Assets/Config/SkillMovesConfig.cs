using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wwc.Cfg;

[ExcelAsset]
public class SkillMovesConfig : ScriptableObject
{
	public List<SkillMoveConfig> SkillMove; // Replace 'EntityType' to an actual type that is serializable.
}
