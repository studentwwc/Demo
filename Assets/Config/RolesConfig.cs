using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using UnityEngine;

[ExcelAsset]
public class RolesConfig : ScriptableObject
{
	public List<RoleConfig> Role; // Replace 'EntityType' to an actual type that is serializable.
}
