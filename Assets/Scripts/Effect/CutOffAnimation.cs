using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutOffAnimation : MonoBehaviour {

	public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
	public Material mat;
	float animationLength = 0;
	float time = 0;
	void Start()
	{
		mat = GetComponent<MeshRenderer> ().material;


		animationLength = curve.keys [curve.length - 1].time;
	}

	void Update()
	{
		animationLength = Mathf.Min (animationLength, time);

		float v = curve.Evaluate (time);
		time += Time.deltaTime;

		mat.SetFloat ("_Cutoff", v);

		if (animationLength == time) {
			enabled = false;
		}
	}
}
