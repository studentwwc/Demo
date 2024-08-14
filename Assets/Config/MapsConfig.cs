using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wwc.Cfg;

[ExcelAsset]
public class MapsConfig : ScriptableObject
{
	public List<MapConfig> Map; // Replace 'EntityType' to an actual type that is serializable.
}
