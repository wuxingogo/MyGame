using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.Steering;
using wuxingogo.Runtime;
using Apex;

[ApexComponent("X Human Mover")]
public class HumanMover : XMonoBehaviour, IMoveUnits {

	public Human human = null;
	public float moveOffset = 1;
	public float rotateOffset = 1;
	void OnHumanFinish(Human human)
	{
		this.human = human;
	}

	#region IMoveUnits implementation

	public void Move(Vector3 velocity, float deltaTime)
	{
		var position = transform.position + (velocity * deltaTime);

		Vector2 velocity2D = new Vector2(velocity.x,velocity.z);
//		_currentSpeed = velocity2D.magnitude;
		if (velocity2D != Vector2.zero) {
			float angleDirection = TurnDir (transform.forward, velocity);

			Vector2 forward2D = new Vector2 (transform.forward.x, transform.forward.z);

			float angle = Vector2.Angle (forward2D, velocity2D) * angleDirection;
			if( Mathf.Abs(angle) > 179f )
			{
				//				Debug.Log("Angle out of range" + " " + angle.ToString());
				return;
			}

			transform.Rotate (new Vector3 (0f, angle, 0f));	
		}

		transform.position = position;
	}

	private static float TurnDir(Vector3 p1, Vector3 p2)
	{
		return Mathf.Sign((p1.z * p2.x) - (p1.x * p2.z));
	}

	public void NetMove(Vector3 position)
	{
		transform.position = position;
	}

	public void Rotate(Vector3 targetOrientation, float angularSpeed, float deltaTime)
	{
//		var targetRotation = Quaternion.LookRotation(targetOrientation);
//		var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * Mathf.Rad2Deg * deltaTime);
//		var offset = Quaternion.Angle(transform.rotation, rotation);
//		if (human != null && human.isServer && offset > rotateOffset) {
//			NetworkListener.Instance.CmdRotation (human.netId.Value, rotation);

//		}
//		var targetRotation = Quaternion.LookRotation(targetOrientation);
//		human.transform.rotation = Quaternion.RotateTowards(human.transform.rotation, targetRotation, angularSpeed * Mathf.Rad2Deg * deltaTime);
//		human.transform.rotation = targetRotation;
	}

	public void NetRotate(Quaternion rotation)
	{
		transform.rotation = rotation;
	}

	public void Stop()
	{
		/* NOOP */
	}

	#endregion
}