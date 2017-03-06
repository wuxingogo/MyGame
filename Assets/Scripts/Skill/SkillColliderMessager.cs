using System;
using UnityEngine;
using wuxingogo.Runtime;

public class SkillColliderMessager : XMonoBehaviour
{
	public Action<SkillColliderMessager, Collider> onCollisionEnter = null;
	public Action<SkillColliderMessager, Collider> onCollisionExit = null;
	public Action<SkillColliderMessager, Collider> onCollisionStay = null;
	void OnTriggerEnter (Collider other)
	{
		if ( onCollisionEnter != null) {
			onCollisionEnter (this, other);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (onCollisionExit != null) {
			onCollisionExit (this, other);
		}
	}
	void OnTriggerStay(Collider other)
	{
		if (onCollisionStay != null) {
			onCollisionStay (this, other);
		}
	}

}


