using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public static UIManager Instance = null;
	public Camera UICamera = null;
	void Start()
	{
		Instance = this;
	}
}
