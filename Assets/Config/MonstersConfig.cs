using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wwc.Cfg;

[ExcelAsset]
public class MonstersConfig : ScriptableObject
{
	public List<MonsterConfig> Monster; // Replace 'EntityType' to an actual type that is serializable.
}
