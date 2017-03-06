using UnityEngine;
using System.Collections;

public class ThirdPersonCtr : MonoBehaviour {

	public AnimationCtr _animatorCtr = null;
	public CharacterController controller = null;

	void Start()
	{
		_animatorCtr = GetComponent<AnimationCtr> ();
		controller = GetComponent<CharacterController> ();
	}
	public void Move(Vector3 targetPoint)
	{
	}

}
