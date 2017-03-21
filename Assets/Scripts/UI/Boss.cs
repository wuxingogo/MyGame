using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Apex.Messages;
using UnityEngine.Networking;
using wuxingogo.Runtime;

public class Boss : Human {

	public List<Player> totalEnemies = new List<Player>();


	public float attackDistance = 10f;
	public bool isCloseToEnemy = false;
	public NetworkIdentity identity = null;
	public float skillWaitingTime = 1f;

	public FireBall fireballSkill =null;
	public int fireBallIndex = 0;
	public SunStrike sunStrikeSkill = null;
	public int sunStrikeIndex = 1;

	public int skillIndex = 0;
	public int[] skillExcuteOrder = new int[]{
		0,1,1,0,0,0,1,0,1
	};
	public override void OnInit ()
	{
		base.OnInit ();

		identity = GetComponent<NetworkIdentity> ();
	}

	public void CloseToEnemy(Human human)
	{

		var p = MathUtils.NearlyPoint(transform.position, human.transform.position, attackDistance - 2);

		CmdMoveTo (p);

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

	public void ExcuteSkill(int index, Human closetEnemy)
	{
		index = skillExcuteOrder [index];
		var skill = totalSkill [index];
		currentSkill = skill;
		if (skill.caseType == CastType.Immatie && skill.CanRelease()) {
			var direction = closetEnemy.transform.position - transform.position;
			NetworkListener.Instance.CmdCastSkillWithDirection (this.netId.Value, index, direction);

			skillIndex = (skillIndex == skillExcuteOrder.Length -1) ? 0: skillIndex+1;
		}
	
	}

	public override void SinglePlayerUpdate()
	{
		
		var enemy = FindClosestEnemy ();

		if (enemy == null || isHangs)
			return;

		var offset = enemy.transform.position - transform.position;
		var distance = offset.magnitude;


		if (distance < attackDistance && skillWaitingTime > 1) {
			skillWaitingTime = 0f;
			ExcuteSkill (skillIndex, enemy);
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
		totalEnemies = Player.allPlayer.ToList();
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
