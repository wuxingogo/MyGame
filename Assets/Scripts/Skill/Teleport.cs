using UnityEngine;
using System.Collections;

public class Teleport : SkillBase {

	public GameObject teleportStart = null;

	void Start()
	{
		teleportStart = CreatePrefab(teleportStart);
		teleportStart.SetActive (true);
		teleportStart.transform.position = human.transform.position;
		human.transform.position = castPoint;
		DestroyGameObject (teleportStart, 3.0f);

		teleportStart = CreatePrefab(teleportStart);
		teleportStart.SetActive (true);
		teleportStart.transform.position = castPoint;
		DestroyGameObject (teleportStart, 3.0f);

		DestroyGameObject (gameObject, 3.0f);
	}

	
}
