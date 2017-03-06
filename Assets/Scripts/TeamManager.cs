using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TeamManager")]
public class TeamManager : GameManager<TeamManager> {
	[HideInInspector]
	public List<int> teamCategory = new List<int>();
	[HideInInspector]
	public List<bool> teamRelations = new List<bool>();
	[HideInInspector]
	public int relationCount = 0;
	[X]
	public void Calculate()
	{
		var array = Enum.GetValues (typeof(TeamCatriay));
		relationCount = array.Length;
		teamCategory.Clear ();
		teamRelations.Clear ();
		for (int i = 0; i < relationCount; i++) {
			int enumValue = (int)array.GetValue (i);

			for (int j = 0; j < relationCount; j++) {
				int otherEnumValue = (int)array.GetValue (j);
				int key = enumValue | otherEnumValue;
	
				teamCategory.Add (key);
				teamRelations.Add (true);

			}
		}
	}
	[X]
	public bool GetRelation(TeamCatriay lhs, TeamCatriay rhs)
	{
		var key = (int)lhs | (int)rhs;
		var index = teamCategory.IndexOf (key);
		var result = teamRelations [index];
		//XLogger.Log (lhs.ToString () + "," + rhs.ToString () + " : " + result);
		return result;
	}

}
