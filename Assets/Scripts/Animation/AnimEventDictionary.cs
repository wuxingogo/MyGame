using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wuxingogo.Runtime;
using System.Diagnostics;

[CreateAssetMenu(fileName = "AnimEventDictionary")]
public class AnimEventDictionary : UnityDictionary<string,float> {
	[X]
	public bool isPlaying
	{
		get{
			return Application.isPlaying;
		}
	}
	[X]
	public void Add(AnimationType animationType, float delta)
	{
		var typeName = animationType.ToString ().Replace("_", " ");
		this [typeName] = delta;
	}
	[X]
	public void Delete(AnimationType animationType, float delta)
	{
		var typeName = animationType.ToString ().Replace("_", " ");
		this.Remove(typeName, delta);
	}
}
