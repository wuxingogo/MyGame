using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalFog : SkillHoldOn {

	public int count = 10;
	public float delta = 0.5f;
	public float time = 0f;
	public GameObject effect = null;
	public UnityExtendCurveTemplate template = null;
	public float range = 0;

	void Start()
	{
		OnSkillBegin ();
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();

		time += Time.deltaTime;

		if (time > delta && count > 0) {
			time = 0;
			count -= 1;
			CreateEffect ();
		} else if(count == 0){
			ImmediatelyFinish ();

		}
	}

	void CreateEffect()
	{
		var v = GameObjectUtil.CreatePrefab (transform, effect);
		v.transform.position = transform.position;
		v.gameObject.SetActive (true);
		var c = v.GetComponent<UnityFollowTargetCurve> ();


		var pos = Random.onUnitSphere * range;
		pos = new Vector3 (pos.x, 0, pos.z) + castPoint;
		c.targetPos = pos;
	}

}
