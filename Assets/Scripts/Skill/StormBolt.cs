using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBolt : SkillTargetBase {

	public GameObject stormFlyGo = null;
	public GameObject stormHitGo = null;
	public UnityFollowTargetCurve followTarget = null;
	public float hangTime = 3;

	void Start()
	{
		followTarget.target = targetHuman.transform;
		followTarget.gameObject.SetActive (true);
		followTarget.ResetTarget ();

		followTarget.OnHitEvent = OnHitHuman;
	}

	void OnHitHuman()
	{
		targetHuman.Hangs (hangTime);

		if (stormHitGo != null) {
			stormHitGo = GameObjectUtil.CreatePrefab (null, stormHitGo);
			Destroy (stormHitGo, 3f);
		}
		Destroy (gameObject);
	}
}
