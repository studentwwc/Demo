using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wwc.Cfg;

[ExcelAsset]
public class StrongsConfig : ScriptableObject
{
	public List<StrongConfig> Strong; // Replace 'EntityType' to an actual type that is serializable.
}
