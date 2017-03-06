using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;

public class GetAllAnimation : XBaseEditor {

	[MenuItem("Tools/GetAllAnimation")]
	static void Excute()
	{
		var o = Selection.objects [0];
		var animator = o as RuntimeAnimatorController;

		var allClips = animator.animationClips;
		string n = "";
		for (int i = 0; i < allClips.Length; i++) {
			var s = allClips [i].name;
			s = s.Replace (" ", "_");
			n +=  s + " = " + i + ",\n";
		}
		XLogger.Log (n);
	}
}
