using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ws.Battle;

[ExcelAsset]
public class PlayerSkill : ScriptableObject
{
	public List<SkillConfig> Skill; // Replace 'EntityType' to an actual type that is serializable.
	public List<ActionConfig> Action; // Replace 'EntityType' to an actual type that is serializable.
	public List<EffectConfig> Effect; // Replace 'EntityType' to an actual type that is serializable.
}
