using wuxingogo.Runtime;

public class AnimationEventDictionary : UnityDictionary<string,float>{
	[X]
	public void AddAnimationEvent(AnimationType animationEvent)
	{
		var name = animationEvent.ToString ();
		Add (name, 0.0f);
	}
	[X]
	public void RemoveAnimationEvent(AnimationType animationEvent)
	{
		var name = animationEvent.ToString ();
		if(ContainKey(name))
			Remove (name, 0.0f);
	}
}