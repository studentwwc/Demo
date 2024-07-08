using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using UnityEngine;

[ExcelAsset]
public class GoodsConfig : ScriptableObject
{
	public List<EquipConfig> Equip; // Replace 'EntityType' to an actual type that is serializable.
	public List<PropConfig> Prop; // Replace 'EntityType' to an actual type that is serializable.
}
