using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;
using System.Collections.Generic;
using System;

public class SkillBase : XMonoBehaviour {
	[Flags]
	public enum TargetType{
		Self,
		Other,
		Enemy,
		Friends,
	}
	public TargetType targetType = TargetType.Other;
	public CastType caseType = CastType.Immatie;
	[SerializeField]
	private int skillID = 0;

	public virtual int SkillID{
		get{
			return skillID;
		}
	}
	public Human human = null;
	public List<GameObject> totalDestroy = new List<GameObject> ();
	public Vector3 castPoint = Vector3.zero;
	public Human targetHuman = null;

	public KeyCode keyCode = KeyCode.B;
	public float castRange = 0.0f;
	public GameObject castCollider = null;
	public float CDTime = 10f;
	private float coolingTime = 0f;
	public float holdOnTime = 0f;
	public bool isCacheSkill = false;
	public bool InCD {
		get{
			return coolingTime < CDTime;
		}
	}

	void OnDrawGizmosSelected()
	{
		Color r = Gizmos.color;
		Gizmos.color = new Color (0.5f, 0.5f, 0, 0.5f);

		Matrix4x4 g = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;

		Gizmos.DrawSphere (Vector3.zero, castRange);

		Gizmos.matrix = g;
		Gizmos.color = r;

		Gizmos.DrawSphere (castPoint, 1);
	}
	[X("Immatiely Cast Skill")]
	/// <summary>
	/// On Shot key was press
	/// </summary>
	public void CastDirection(Vector3 direction)
	{
		switch (caseType) {
		case CastType.Immatie:
			if (CanRelease ()) {
				human.transform.forward = direction;
				human.Animator.Attack ();
			}
			break;
		case CastType.Point:
			
			break;
		case CastType.Target:
			break;
		default:
			break;
		}
	}
	[X]
	public void CastPoint(int currentSkillIndex, Vector3 point)
	{
		if (CanReleaseAtPos (point)) {
			castPoint = point;
			CreateCastRangeCollider (currentSkillIndex, point);

		}

	}
	[X]
	public virtual void CastHuman(int currentSkillIndex, Human other)
	{
		targetHuman = other;
		CreateCastHuman (currentSkillIndex, other);
	}
	public void PrepareCancel()
	{
		if (castCollider != null) {
			Destroy (castCollider);
			castCollider = null;
		}
	}

	public void CreateCastHuman(int currentSkillIndex, Human other)
	{
		human.Follow (other);
		if (castCollider == null) {
			castCollider = new GameObject ("SkillCastRanger");
			var sphere = castCollider.AddComponent<SphereCollider> ();
			castCollider.transform.position = other.transform.position;
			sphere.radius = castRange;
			sphere.isTrigger = true;
			var skillCollider = castCollider.AddComponent<SkillCasterMessager> ();
			skillCollider.human = human;
			skillCollider.targetHuman = other;
			skillCollider.onCollisionStay = () => {

				NetworkListener.Instance.CmdCastSkillWithTarget (human.netId.Value, currentSkillIndex, other.netId.Value);
				DestroyGameObject(castCollider, 0.0f);
				castCollider = null;
				skillCollider.onCollisionStay = null;
			};
		}
	}

	public void CreateCastRangeCollider(int currentSkillIndex, Vector3 point)
	{
//		human.CmdMoveTo (point);
		var p = MathUtils.NearlyPoint(human.transform.position, point, castRange - 1);
		human.CmdMoveTo (p);
		if (castCollider == null) {
			castCollider = new GameObject ("SkillCastRanger");
			var sphere = castCollider.AddComponent<SphereCollider> ();
			castCollider.transform.position = p;
			sphere.radius = castRange;
			sphere.isTrigger = true;
			var skillCollider = castCollider.AddComponent<SkillCasterMessager> ();
			skillCollider.human = human;
			skillCollider.onCollisionStay = () => {
				NetworkListener.Instance.CmdCastSkillWithPoint (human.netId.Value, currentSkillIndex, point);
				DestroyGameObject(castCollider, 0.0f);
				castCollider = null;
				skillCollider.onCollisionStay = null;
			};
		}
	}

	public void DestroyGameObject(GameObject go, float time)
	{
		if(!totalDestroy.Contains(go))
			Destroy (go, time);
	}

	public GameObject CreatePrefab(GameObject prefab)
	{
		return CreatePrefab (null, prefab, prefab.layer, human.TeamTag);
	}
	public T CreateComponent<T>(Component component) where T : Component
	{
		var go = CreatePrefab (null, component.gameObject, component.gameObject.layer, human.TeamTag);
		var t = go.GetComponent<T> ();
		return t;
	}

	public GameObject CreatePrefab(GameObject prefab, string tag)
	{
		return CreatePrefab (null, prefab, prefab.layer, tag);
	}

	public GameObject CreatePrefab(GameObject prefab, int layer, string tag)
	{
		return CreatePrefab (null, prefab, layer, tag);
	}

	public GameObject CreatePrefab(Transform parent,GameObject prefab, int layer, string tag)
	{
		if( prefab == null )
			return null;
		GameObject go = (GameObject)GameObject.Instantiate(prefab);


		go.transform.localScale = Vector3.one;
		go.transform.parent = parent;
		go.transform.localPosition = Vector3.zero;
		go.layer = layer;
		go.tag = tag;
		return go;
	}
	public GameObject CreatePrefab(Transform parent,GameObject prefab)
	{
		if( prefab == null )
			return null;
		GameObject go = (GameObject)GameObject.Instantiate(prefab);

		go.transform.localScale = Vector3.one;
		go.transform.parent = parent;
		go.transform.localPosition = Vector3.zero;
		return go;
	}

	[X]
	public bool CanTarget(Human goal)
	{
//		XLogger.Log ("DestSelect", this.gameObject);
		if (targetType == TargetType.Self && human == goal) {
			return true;
		} else if (targetType == TargetType.Other && human != goal) {
			return true;
		} else if (targetType == TargetType.Enemy && !TeamManager.instance.GetRelation (human.teamCatriay, goal.teamCatriay)) {
			return true;
		} else if (targetType == TargetType.Friends && TeamManager.instance.GetRelation (human.teamCatriay, goal.teamCatriay)) {
			return true;
		} 
		return false;

	}


	public SkillColliderMessager FireCastRange()
	{
		var colliderGo = new GameObject ();
		colliderGo.transform.position = human.transform.position;
		var collider = colliderGo.AddComponent<SphereCollider> ();
		collider.radius = castRange;
		collider.isTrigger = true;
		var messager = colliderGo.AddComponent<SkillColliderMessager> ();
		return messager;
	}
	public virtual bool CanRelease()
	{
		if (coolingTime > CDTime && !human.isHangs) {
			return true;
		}
		return false;
	}
	public virtual bool CanReleaseAtPos(Vector3 position)
	{
		if (CanRelease ()) {
			return true;
		}
		return false;
	}
	public virtual bool CanReleaseAtHuman(Human human)
	{
		if (CanRelease () && !human.IsSkillImmunited() && CanTarget (human)) {
			return true;
		}
		return false;
	}
	public void Cooling()
	{
		coolingTime = 0;
	}
	public void ResetCD()
	{
		coolingTime = CDTime;
	}

	public void Update()
	{
		//UpdateCoolCD ();
		OnUpdate ();
	}

	public void UpdateCoolCD()
	{
		coolingTime += Time.deltaTime;
	}
	public virtual void OnUpdate()
	{
	}
}
