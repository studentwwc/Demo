using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wwc.Cfg;

[ExcelAsset]
public class RenamesConfig : ScriptableObject
{
	public List<RenameConfig> Rdname; // Replace 'EntityType' to an actual type that is serializable.
}
