using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverHPBox : Articles {
	// percent
	public float HP = 10;
	public GameObject hpEffect = null;
	public override void OnResume (Player player)
	{
		base.OnResume (player);

		float t = (player.dataSystem.MaxHP * HP / 100);
		player.dataSystem.RecoveryHP (t);
		var go = GameObjectUtil.CreatePrefab (player.transform, hpEffect);
		Destroy (go, 5);
	}
}
