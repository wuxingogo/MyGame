using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;
using System.Collections.Generic;
using System;


[CreateAssetMenu(fileName = "PrototypeManager")]
public class PrototypeManager : GameManager<PrototypeManager> {
	public List<ConstructionStruct> totalConstruction = new List<ConstructionStruct> ();
	public Dictionary<ConstructType, List<ConstructionStruct>> Construction = new Dictionary<ConstructType, List<ConstructionStruct>>();

	public void Sort()
	{
		Construction.Clear ();
		for (int i = 0; i < totalConstruction.Count; i++) {
			if(!Construction.ContainsKey(totalConstruction [i].type))
				Construction.Add (totalConstruction [i].type, new List<ConstructionStruct>());
			Construction [totalConstruction [i].type].Add (totalConstruction [i]);
			
		}
	}

	public AnimationEventDictionary totalEvent;

	[X]
	public void ImportAnimation()
	{
		totalEvent = Create<AnimationEventDictionary> (this);
		var values = Enum.GetValues(typeof(AnimationType));
		for (int i = 0; i < values.Length; i++) {
			var animationType = (AnimationType)values.GetValue (i);
			var typeName = animationType.ToString ().Replace("_", " ");
			if(typeName.Contains("Attack"))
				totalEvent.Add (typeName, 0.3f);
		}
	}
}
