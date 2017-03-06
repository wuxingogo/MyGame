using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPanel : MonoBehaviour {

	public static HPPanel Instance = null;

	void Awake()
	{
		Instance = this;
	}
}
