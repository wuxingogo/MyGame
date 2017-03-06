using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeCycle : MonoBehaviour {

	public float lifecycle = 0f;

	public List<Human> targets = new List<Human>();

	public virtual void OnRemoveFromScene()
	{
		Destroy (gameObject);
	}
}
