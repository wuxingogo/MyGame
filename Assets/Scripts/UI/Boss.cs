using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boss : Human {


	public List<Player> totalEnemies = new List<Player>();

	public FireBall fireballSkill =null;
	public override void OnInit ()
	{
		base.OnInit ();

		totalEnemies = FindObjectsOfType<Player> ().ToList();

		for (int i = 0; i < totalSkill.Count; i++) {
			var skill = totalSkill [i];
			if (skill is FireBall) {
				fireballSkill = skill as FireBall;
				currentSkill = fireballSkill;
			}
		}
	}

	public override void OnUpdate()
	{
		var enemy = FindClosestEnemy ();

		transform.forward = (enemy.transform.position - transform.position).normalized;
		currentSkill = fireballSkill;
		fireballSkill.Prepare ();
	}

	public Player FindClosestEnemy()
	{
		Player result = null;
		float distance = 999999;
		for (int i = 0; i < totalEnemies.Count; i++) {
			float d = (totalEnemies [i].transform.position - transform.position).magnitude;
			if (d < distance) {
				distance = d;
				result = totalEnemies [i];
			}

		}
		return result;
	}
}
