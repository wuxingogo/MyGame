using UnityEngine;
using System.Collections;
using System;

public class UnityExtendCurve : MonoBehaviour {

	public AnimationCurve xCurve = AnimationCurve.Linear(0,0,1,0);
	public AnimationCurve yCurve = AnimationCurve.Linear(0,0,1,0);
	public AnimationCurve zCurve = AnimationCurve.Linear(0,0,1,10);

	public float elapseTimer = 0f;
	public float scale = 1f;
	public bool isChangeMatrix = false;
	public Matrix4x4 recordMatrix;
	public Matrix4x4 inverseMatrix;
	public Action onFinish = null;
	public float lastTime = 0.0f;
	void Start()
	{
		recordMatrix = transform.localToWorldMatrix;
		inverseMatrix = recordMatrix.inverse;

		int last = zCurve.keys.Length - 1;
		var frame = zCurve.keys[last];
		lastTime = frame.time;
	}
	public Vector3 Evaluate(float time)
	{
		float x = xCurve.Evaluate (time);
		float y = yCurve.Evaluate (time);
		float z = zCurve.Evaluate (time);
		return new Vector3 (x, y, z);
	}
	public Vector3 gizmoSize = Vector3.one;
	void OnDrawGizmos ()
	{
		int last = zCurve.keys.Length - 1;
		var frame = zCurve.keys[last];
		float endTime = frame.time;

		var m = Gizmos.matrix;
		if (Application.isPlaying)
			Gizmos.matrix = recordMatrix;
		else
			Gizmos.matrix = transform.localToWorldMatrix;

		for (float i = 0.0f; i < endTime; i += 0.01f) {
			var p = Evaluate (i);
			Gizmos.DrawCube (p, gizmoSize);
		}

		Gizmos.matrix = m;


	}
	void Update()
	{
		var v = Evaluate (elapseTimer);
		v = recordMatrix.MultiplyPoint (v);
		var direction = (v - transform.position).normalized;
		if(direction != Vector3.zero)
			transform.forward = direction;
		transform.position = v;

		elapseTimer += Time.deltaTime * scale;
		if (elapseTimer > lastTime) {
			if (onFinish != null)
				onFinish ();
		}
	}
}
