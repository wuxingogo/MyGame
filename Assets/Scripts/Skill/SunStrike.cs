using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunStrike : SkillBase {

	public GameObject prepare = null;
	public int maxCount = 5;
	public float time = 1.5f;
	public GameObject collision = null;
	public float energy = 10;
	void Start()
	{
		var allPlayer = Player.allPlayer;
		maxCount = Mathf.Min (allPlayer.Length, maxCount);
		for (int i = 0; i < maxCount; i++) {
			var p = allPlayer [i];
			var newStrike = GameObjectUtil.CreatePrefab (null, prepare);
			newStrike.SetActive (true);
			newStrike.transform.position = p.transform.position + new Vector3 (0, 2);
			DestroyGameObject (newStrike, time);
			StartCoroutine (CreateExplosion(newStrike.transform.position));
			
		}
	}

	IEnumerator CreateExplosion(Vector3 position)
	{
		yield return new WaitForSeconds (time);
		var explosion = GameObjectUtil.CreatePrefab (null, collision);
		explosion.SetActive (true);
		explosion.transform.position = position;
		var messager = explosion.AddComponent<SkillColliderMessager> ();
		messager.onCollisionEnter = OnBallCollision;

		DestroyGameObject (explosion, time);
		DestroyGameObject (gameObject, time);
	}

	void OnBallCollision(SkillColliderMessager messager, Collider other)
	{
		var targetHuman = other.GetComponent<Human> ();
		if (targetHuman != null && CanTarget (targetHuman)) {

			//targetHuman.dataSystem.Damage (energy);
			NetworkListener.Instance.CmdDamage(targetHuman.netId.Value, energy);
			
		}
	}


}
