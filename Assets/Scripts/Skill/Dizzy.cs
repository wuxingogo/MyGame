using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dizzy : MonoBehaviour {
	
	public float time = 5;
	public Human human = null;

	void Update()
	{
		time -= Time.deltaTime;
		if (time <= 0) {
			Destroy (this);
		}
	}
}
