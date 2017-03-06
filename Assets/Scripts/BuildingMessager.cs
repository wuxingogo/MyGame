using UnityEngine;
using System.Collections;

public class BuildingMessager : MonoBehaviour {

	public void BuildingFinish()
	{
		var target = transform.Find ("Container").GetChild (0);
		target.SetParent (null);
	}
}
