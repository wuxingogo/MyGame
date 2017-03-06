using UnityEngine;
using System.Collections;
using wuxingogo.Editor;
using UnityEditor;
using System;


[CustomEditor(typeof(TeamManager), true)]
public class TeamManagerEditor : XMonoBehaviourEditor {

	TeamManager teamManager = null;
	void OnEnable()
	{
		teamManager = target as TeamManager;
	}
	public override void OnXGUI ()
	{
		base.OnXGUI ();

		var catriay = typeof(TeamCatriay);
		var array = Enum.GetValues (catriay);
		int arrayLenght = array.Length;

		string[] title = new string[arrayLenght];
		for (int i = 0; i < title.Length; i++) {
			title[i] = array.GetValue (i).ToString();
		}

		UnityMatrixEditorGUI.DoGUI("Team Relation", ref this.show, ref this.scrollPos,title, new UnityMatrixEditorGUI.GetValueFunc(this.GetValue), new UnityMatrixEditorGUI.SetValueFunc(this.SetValue));
	

	}

	private Vector2 scrollPos;

	private bool show = true;

	private bool GetValue(int layerA, int layerB)
	{
		var index = layerA * teamManager.relationCount + layerB;
		return teamManager.teamRelations[index];
	}

	private void SetValue(int layerA, int layerB, bool val)
	{
		var index = layerA * teamManager.relationCount + layerB;
		teamManager.teamRelations [index] = val;
	}
}
