using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.Steering;
using wuxingogo.Runtime;
using Apex;

[ApexComponent("X Network Mover")]
public class NetworkMover : XMonoBehaviour, IMoveUnits {

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
		var offset = transform.position - position;
		if( human != null && human.isServer && offset.magnitude > moveOffset)
			NetworkListener.Instance.CmdMove (human.netId.Value, position);
	}
	public void NetMove(Vector3 position)
	{
		transform.position = position;
	}

	public void Rotate(Vector3 targetOrientation, float angularSpeed, float deltaTime)
	{
		var targetRotation = Quaternion.LookRotation(targetOrientation);
		var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * Mathf.Rad2Deg * deltaTime);
		var offset = Quaternion.Angle(transform.rotation, rotation);
		if( human != null && human.isServer && offset > rotateOffset)
			NetworkListener.Instance.CmdRotation (human.netId.Value, rotation);

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