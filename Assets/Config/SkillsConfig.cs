using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wwc.Cfg;


[ExcelAsset]
public class SkillsConfig : ScriptableObject
{
	public List<SkillConfig> Skill; // Replace 'EntityType' to an actual type that is serializable.
}
