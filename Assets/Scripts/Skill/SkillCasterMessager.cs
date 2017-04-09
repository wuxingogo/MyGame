using UnityEngine;
using System.Collections;
using System;

public class SkillCasterMessager : MonoBehaviour {

	public Action onCollisionStay = null;
	public Human human = null;
	public Human targetHuman = null;
	void Start()
	{
		if (targetHuman != null) {
			targetHuman.OnDestroyAction +=()=> Destroy (gameObject);
		}
	}
	void OnTriggerEnter (Collider other)
	{
		if (human != other.gameObject && onCollisionStay != null) {
			onCollisionStay ();
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (human != other.gameObject && onCollisionStay != null) {
			onCollisionStay ();
		}
	}
	void OnTriggerStay(Collider other)
	{
		if (human != other.gameObject && onCollisionStay != null) {
			onCollisionStay ();
		}
	}
	void OnDrawGizmos ()
	{
		var c = GetComponent<Collider> ();
		GizmoUtlis.color = new Color (0, 0.5f, 0, 0.5f);
		GizmoUtlis.DrawCollider (c);

	}
	void Update()
	{
		if (targetHuman != null) {
			transform.position = targetHuman.transform.position;
		}
	}
}
