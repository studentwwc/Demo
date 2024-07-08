using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wwc.Cfg;

[ExcelAsset]
public class TaskConfig : ScriptableObject
{
	public List<AutoGuideConfig> AutoGuide; // Replace 'EntityType' to an actual type that is serializable.
}
