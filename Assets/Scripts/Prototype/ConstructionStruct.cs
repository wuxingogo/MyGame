using UnityEngine;
using System.Collections;
[System.Serializable]
public class ConstructionStruct {

	public ConstructType type = ConstructType.Airfield;
	public GameObject gameObject = null;
	public int level = 1;
}

public enum ConstructType
{
	Airfield = 10,
	CannonField = 20,
	Factory = 30,
	Fortress = 40,
	MainBase = 50
}
