using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Apex.Messages;
using UnityEngine.Networking;
using wuxingogo.Runtime;

public class Boss : Human {


	public List<Player> totalEnemies = new List<Player>();

	public FireBall fireballSkill =null;
	public float attackDistance = 10f;
	public bool isCloseToEnemy = false;
	public NetworkIdentity identity = null;

	public override void OnInit ()
	{
		base.OnInit ();

		totalEnemies = FindObjectsOfType<Player> ().ToList();

		for (int i = 0; i < totalSkill.Count; i++) {
			var skill = totalSkill [i];
			if (skill is FireBall) {
				fireballSkill = skill as FireBall;
				nextSkill = new SkillCastStructure ();
				nextSkill.skillBase = fireballSkill;
			}
		}
		identity = GetComponent<NetworkIdentity> ();
	}

	public void CloseToEnemy(Human human)
	{
		if (!isCloseToEnemy) {
			isCloseToEnemy = true;
			var p1 = transform.position;
			var p2 = human.transform.position;
			var v = p2 - p1;
			Vector3 p = (v.normalized) * attackDistance + transform.position;
			CmdMoveTo (p);
		}
	}
	public override void OnHangsEnded ()
	{
		base.OnHangsEnded ();
		isCloseToEnemy = false;
	}

	public override void OnFinishMove (UnitNavigationEventMessage message)
	{
		base.OnFinishMove (message);
		isCloseToEnemy = false;
	}

	public override void SinglePlayerUpdate()
	{
		base.SinglePlayerUpdate ();
		var enemy = FindClosestEnemy ();

		if (enemy == null || isHangs)
			return;

		var offset = enemy.transform.position - transform.position;
		var distance = offset.magnitude;
		var direction = offset.normalized;

		if (distance < attackDistance) {
			transform.forward = direction;
			currentSkill = fireballSkill;
			fireballSkill.Cast ();
			nextSkill = new SkillCastStructure ();
			nextSkill.skillBase = fireballSkill;
		} else {
			CloseToEnemy (enemy);
		}
	}

	void Update()
	{
		if (!isServer) {
			return;
		}
		SinglePlayerUpdate ();

	}

	public Player FindClosestEnemy()
	{
		Player result = null;
		float distance = 999999;
		for (int i = 0; i < totalEnemies.Count; i++) {
			if (totalEnemies [i].isDead)
				continue;
			float d = (totalEnemies [i].transform.position - transform.position).magnitude;
			if (d < distance) {
				distance = d;
				result = totalEnemies [i];
			}

		}
		return result;
	}
}
