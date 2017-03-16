using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketGroup : SkillHoldOn {

	public int count = 10;
	public float delta = 0.5f;
	public float time = 0f;
	public GameObject effect = null;
	public GameObject explosion = null;
	public UnityExtendCurveTemplate template = null;
	public float range = 0;
	public float hangTime = 1f;
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
		var v = GameObjectUtil.CreatePrefab (null, effect);
		v.transform.position = transform.position;
		v.gameObject.SetActive (true);
		var c = v.GetComponent<UnityFollowTargetCurve> ();


		var pos = Random.onUnitSphere * range;
		pos = new Vector3 (pos.x, 0, pos.z) + castPoint;
		c.targetPos = pos;

		c.OnHitEvent += () => {
			var e = GameObjectUtil.CreatePrefab(null, explosion);
			e.transform.position = pos;
			var boxCollider = e.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
			boxCollider.size = Vector3.one * 3;
			var collider = e.AddComponent<SkillColliderMessager>();
			collider.onCollisionEnter += (m, t)=> {
				var human = t.GetComponent<Human>();
				if(human != null)
				{
					human.Hangs(hangTime);
				}
			};
			Destroy(e, 5);
			Destroy(v, 5);
		};

	}

}
