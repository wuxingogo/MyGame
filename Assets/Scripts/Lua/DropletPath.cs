using System;
using wuxingogo.Runtime;
using UnityEngine;


public class DropletPath : XMonoBehaviour
{
	public Vector3 size = Vector3.one;
	public float length = 1.0f;
	public float a = 1;
	public float b = 5 / 2.0f;
	void OnDrawGizmos()
	{
		for (float i = 0.0f; i < length; i += 0.01f) {
			var x = a * (1 - Mathf.Sin (i)) * Mathf.Cos (i);
			var y = b * (Mathf.Sin(i) - 1);
			Gizmos.DrawCube (new Vector3 (x, y), size);
		}
	}

	float currentStep = 0.0f;
	public float step = 0.1f;
	public bool isBegin = false;
	[X]
	void DoPath()
	{
		isBegin = true;
		currentStep = 14;
	}
	void Update()
	{
		if (!isBegin)
			return;
		if (currentStep < 20.5) {
			
			MoveTo (currentStep);
			currentStep += step;
		} else {
			isBegin = false;
		}
	}
	private float _step = 0.0f;
	[X]
	float percent
	{
		set{
			_percent = value;
			var x = a * (1 - Mathf.Sin (_percent)) * Mathf.Cos (_percent);
			var y = b * (Mathf.Sin (_percent) - 1);
			transform.position = new Vector3 (x, y);
		}
		get{
			return _percent;
		}
	}
	private float _percent = 0.0f;
	[X]
	void MoveTo(float step)
	{
		percent = step;
	}
}


