using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Articles : MonoBehaviour {
	public float rotateSpeed = 1f;
	[Range(3, 10)]
	public float beatingRange = 1f;
	public float beatingSpeed = 1f;

	Vector3 startPosition = Vector3.zero;
	
	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float delta = Mathf.PingPong (Time.time * beatingSpeed, beatingRange);
		transform.position = new Vector3 (startPosition.x, delta + startPosition.y, startPosition.z);
		transform.Rotate (new Vector3 (0, rotateSpeed, 0));
	}
}
