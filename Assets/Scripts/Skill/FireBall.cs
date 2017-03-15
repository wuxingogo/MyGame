using UnityEngine;
using System.Collections;

public class FireBall : SkillBase {

	public GameObject throwBall = null;
	public GameObject collision = null;
	public float energy = 10;
	void Start()
	{
		var skelegon = human.GetSkeleton (HandsType.RIGHT);
		throwBall = CreatePrefab(throwBall);
		throwBall.SetActive (true);
		throwBall.transform.position = skelegon.transform.position;
		throwBall.transform.forward = human.transform.forward;
		var curve = throwBall.GetComponent<UnityExtendCurve> ();
		curve.onFinish = () =>{ 
			DestroyGameObject(throwBall, 0.0f);
			DestroyGameObject (gameObject, 0.1f);
		};

		var messager = throwBall.AddComponent<SkillColliderMessager> ();
		messager.onCollisionEnter = OnBallCollision;
	}

	void OnBallCollision(SkillColliderMessager messager, Collider other)
	{
		var targetHuman = other.GetComponent<Human> ();
		if (targetHuman != null && CanTarget (targetHuman)) {

			//targetHuman.dataSystem.Damage (energy);
			NetworkListener.Instance.CmdDamage(targetHuman.netId.Value, energy);
			if (collision != null) {
				var go = CreatePrefab (collision);
				go.SetActive (true);
				go.transform.position = other.transform.position;

				DestroyGameObject (go, 5);
			}
//			XLogger.Log ("OnBallCollision");
		}
	}




}
