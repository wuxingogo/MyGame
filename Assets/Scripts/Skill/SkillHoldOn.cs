using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillHoldOn : SkillBase {
	public Action onSkillBegin = null;
	public Action onSkillEnd = null;

	public virtual void OnSkillBegin()
	{
		human.Animator.HoldonSkill (true);
		human.holdOnSkill = this;
		if (onSkillBegin != null) {
			onSkillBegin ();
		}
	}
	public virtual void OnSkillEnd()
	{
		human.Animator.HoldonSkill (false);
		human.holdOnSkill = null;
		if (onSkillEnd != null) {
			onSkillEnd ();
		}
		Destroy (gameObject);
	}

	public void ImmediatelyFinish()
	{
		holdOnTime = 0;
	}

}
