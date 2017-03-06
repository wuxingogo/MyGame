using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;

public class AirPlane : XMonoBehaviour {

	public bool rollback = false;
	public Vector3 speed = Vector3.zero;
	public Vector3 initSpeed = new Vector3(0.1f, 0);
	public Vector3 targetSpeed = Vector3.zero;
	public float rollbackSpeed = 0.5f;
	void Start () {
		speed = initSpeed;
	}
	[X]
	void Rollback()
	{
		targetSpeed = -initSpeed;
		initSpeed = -initSpeed;
		rollback = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (rollback) {
			speed = Vector3.Lerp (speed, targetSpeed, Time.deltaTime * rollbackSpeed);
			if((targetSpeed - speed).magnitude < 0.001f)
			{
				rollback = false;
			}
			transform.position += speed;
		} else {
			transform.position += speed;
		}	
	}
}
