using UnityEngine;
using System.Collections;

public class DynamicObstacle : MonoBehaviour {

	// Use this for initialization
	public float xoffset = 1.0f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x - xoffset, transform.position.y);
	}
}
