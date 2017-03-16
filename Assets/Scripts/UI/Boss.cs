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
	public int skillIndex = 0;
	public float attackDistance = 10f;
	public bool isCloseToEnemy = false;
	public NetworkIdentity identity = null;
	public float skillWaitingTime = 1f;

	public override void OnInit ()
	{
		base.OnInit ();

		//totalEnemies = FindObjectsOfType<Player> ().ToList();

		for (int i = 0; i < totalSkill.Count; i++) {
			var skill = totalSkill [i];
			if (skill is FireBall) {
				fireballSkill = skill as FireBall;
				skillIndex = i;
//				nextSkill = new SkillCastStructure ();
//				nextSkill.skillBase = fireballSkill;
			}
		}
		identity = GetComponent<NetworkIdentity> ();
	}

	public void CloseToEnemy(Human human)
	{
//		if (!isCloseToEnemy) {
//			isCloseToEnemy = true;
//			var p1 = transform.position;
//			var p2 = human.transform.position;

			var p = MathUtils.NearlyPoint(transform.position, human.transform.position, attackDistance - 2);
//			Vector3 p = (v.normalized) * attackDistance + transform.position;
			CmdMoveTo (p);
//		}
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
		
		var enemy = FindClosestEnemy ();

		if (enemy == null || isHangs)
			return;

		var offset = enemy.transform.position - transform.position;
		var distance = offset.magnitude;
		var direction = offset.normalized;

		if (distance < attackDistance && fireballSkill.CanRelease() && skillWaitingTime > 1) {
			skillWaitingTime = 0f;
			currentSkill = fireballSkill;
			NetworkListener.Instance.CmdCastSkillWithDirection (this.netId.Value, skillIndex, direction);
//			fireballSkill.CastDirection (direction);
//			nextSkill = new SkillCastStructure ();
//			nextSkill.skillBase = fireballSkill;
		} else if(distance > attackDistance){
			CloseToEnemy (enemy);
		}
		skillWaitingTime += Time.deltaTime;
	}

	void Update()
	{
		base.SinglePlayerUpdate ();
		if (!isServer) {
			return;
		}
		if(!isLocalInit)
		{
			OnInit ();
			isLocalInit = true;
		}
		SinglePlayerUpdate ();

	}

	public Player FindClosestEnemy()
	{
		Player result = null;
		float distance = 999999;
		totalEnemies = Player.allPlayer;
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
