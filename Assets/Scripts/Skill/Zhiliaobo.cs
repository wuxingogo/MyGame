using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;
using System.Collections.Generic;

public class Zhiliaobo : SkillTargetBase {
	public TrailRenderer trailRenderer = null;
	[Range(1,20)]
	public int maxCount = 10;
	public bool CanRetarget = true;
	public List<Human> antiqueGoals = new List<Human>();
	public GameObject reEmitCollider = null;
	public float destroyTime = 5f;
	public UnityFollowTargetCurve followTarget = null;

	public GameObject hpEffect = null;
	void Start()
	{
		
		if (maxCount > 1) {
			var messager = FireCastRange ();
			reEmitCollider = messager.gameObject;
			reEmitCollider.gameObject.SetActive (false);
			messager.onCollisionEnter = OnColliderEnter;
		}

		Emit();

	}
	[X]
	void Emit()
	{
		if (maxCount > 0) {
			maxCount--;
			trailRenderer.gameObject.SetActive (true);
			//		trailRenderer.transform.position = targetHuman.transform.position;
			followTarget.target = targetHuman.transform;

			followTarget.ResetTarget ();
			followTarget.enabled = true;	

			if (CanRetarget) {
				// 禁止寻找到同一个物体
				antiqueGoals.Add (targetHuman);
			} else {
				antiqueGoals.Add (targetHuman);
			}
			followTarget.OnHitEvent = () => {
				OnSkillFinish(targetHuman);
				reEmitCollider.transform.position = targetHuman.transform.position;
				reEmitCollider.SetActive (true);
				followTarget.enabled = false;
			};
		} else {
			DestroyGameObject (reEmitCollider, 1f);
			DestroyGameObject (gameObject, 1f);
		}

		destroyTime += 5f;
	}

	public void OnColliderEnter(SkillColliderMessager messager, Collider collider)
	{
		var target = collider.GetComponent<Human> ();
		followTarget.enabled = true;
		if(target!= null && !antiqueGoals.Contains(target) && !target.IsSkillImmunited()){
			if (CanTarget (target)) {
				
				followTarget.gameObject.SetActive(true);
				reEmitCollider.gameObject.SetActive (false);
				if (CanRetarget) {
					antiqueGoals.RemoveAt (0);
					followTarget.transform.position = targetHuman.transform.position;
					targetHuman = target;
					Emit ();
				}
				else {
					
					followTarget.transform.position = targetHuman.transform.position;
					targetHuman = target;
					Emit ();
				}
			} 
		}
	}
	public void OnSkillFinish(Human targetHuman)
	{
		XLogger.Log (targetHuman.name);

		targetHuman.dataSystem.RecoveryHP (50);

		var go = CreatePrefab (targetHuman.transform, hpEffect);
		Destroy (go, 5);
	}

	public override void OnUpdate ()
	{
		if (destroyTime > 0) {
			destroyTime -= Time.deltaTime;
			if (destroyTime < 0) {
				DestroyGameObject (reEmitCollider, 1f);
				DestroyGameObject (gameObject, 0);
			}
		}
	}
}
