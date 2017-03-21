using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;

public class AnimationCtr : XMonoBehaviour {

	public readonly int WALK = Animator.StringToHash("Walk");
	public readonly int RUN = Animator.StringToHash("Run");
	public readonly int DASH = Animator.StringToHash("Dash");
	public readonly int DIE = Animator.StringToHash("Die");
	public readonly int TALKING = Animator.StringToHash("Talking");
	public readonly int CRYING = Animator.StringToHash("Crying");
	public readonly int TAKE_DAMAGE = Animator.StringToHash("Take Damage");
	//--Melee--//
	public readonly int MELEE_RIGHT_ATTACK_01 = Animator.StringToHash("Melee Right Attack 01");
	public readonly int MELEE_RIGHT_ATTACK_02 = Animator.StringToHash("Melee Right Attack 02");
	public readonly int MELEE_RIGHT_ATTACK_03 = Animator.StringToHash("Melee Right Attack 03");
	public readonly int MELEE_JUMP_RIGHT_ATTACK_03 = Animator.StringToHash("Melee Right Attack 03");
	//--Crossbow--//
	public readonly int CROSSBOW_IDLE = Animator.StringToHash("Crossbow Aim");
	public readonly int CROSSBOW_RELOAD = Animator.StringToHash("Crossbow Reload");
	public readonly int CROSSBOW_LEFT_ATTACK = Animator.StringToHash("Crossbow Shoot Attack");
	public readonly int CROSSBOW_RIGHT_ATTACK = Animator.StringToHash("Crossbow Right Shoot Attack");

	//--Fly Mode--//
	public readonly int FLY_IDLE = Animator.StringToHash("Fly Idle");
	public readonly int FLY_FORWARD = Animator.StringToHash("Fly Forward");
	public readonly int FLY_RIGHT_ATTACK_01 = Animator.StringToHash("Fly Melee Right Attack 01");
	public readonly int FLY_RIGHT_ATTACK_02 = Animator.StringToHash("Fly Melee Right Attack 02");
	public readonly int FLY_RIGHT_ATTACK_03 = Animator.StringToHash("Fly Melee Right Attack 03");
	public readonly int FLY_RIGHT_LEFT_ATTACK_01 = Animator.StringToHash("Fly Melee Left Attack 01");
	public readonly int FLY_DIE = Animator.StringToHash("Fly Die");
	//--Spear Model--//
	public readonly int SPEAR_IDLE = Animator.StringToHash("Spear Idle");
	public readonly int SPEAR_WALK = Animator.StringToHash("Spear Walk");
	public readonly int SPEAR_RUN = Animator.StringToHash("Spear Run");
	public readonly int SPEAR_DASH = Animator.StringToHash("Spear Dash");
	public readonly int SPEAR_DIE = Animator.StringToHash("Spear Die");
	public readonly int SPEAR_MELEE_ATTACK_01 = Animator.StringToHash("Spear Melee Attack 01");
	public readonly int SPEAR_MELEE_ATTACK_02 = Animator.StringToHash("Spear Melee Attack 02");

	//--TH Sword--//
	public readonly int TH_SWORD_IDLE = Animator.StringToHash("TH Sword Idle");
	public readonly int TH_SWORD_WALK = Animator.StringToHash("TH Sword Walk");
	public readonly int TH_SWORD_RUN = Animator.StringToHash("TH Sword Run");
	public readonly int TH_SWORD_DASH = Animator.StringToHash("TH Sword Dash");
	public readonly int TH_SWORD_JUMP = Animator.StringToHash("TH Sword Dash");
	public readonly int TH_SWORD_ATTACK_01 = Animator.StringToHash("TH Sword Melee Attack 01");
	public readonly int TH_SWORD_ATTACK_02 = Animator.StringToHash("TH Sword Melee Attack 02");
	public readonly int TH_SWORD_CAST_SPELL = Animator.StringToHash("TH Sword Cast Spell");
	public readonly int TH_SWORD_DIE = Animator.StringToHash("TH Sword Die");

	public readonly int CHOP_TREE = Animator.StringToHash("Chop Tree");

	public readonly string LEFT_HAND = "RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigLArm1/RigLArm2/RigLArmPalm/Dummy Prop Left";
	public readonly string RIGHT_HAND = "RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigRArm1/RigRArm2/RigRArmPalm/Dummy Prop Right";

	private Animator _animator = null;
	public WeaponType weaponType = WeaponType.NONE;

	public Human human = null;
	void Start()
	{
		if (!isFnish) {
			Prepare ();
		}
	}
	private bool isFnish = false;
	public void Prepare()
	{
		
		_animator = GetComponent<Animator> ();
		var modelCtr = gameObject.AddComponent<ModelCtr> ();
		modelCtr.animCtr = this;

		BindSkill ();
		isFnish = true;
	}
	int idlePara = 0;
	int lastBoolPara = 0;

	[X]
	public void Idle()
	{
		if (idlePara != 0) {
			_animator.SetBool (idlePara, false);
		}
		DisableAnimation ();
		switch (weaponType) {
		case WeaponType.NONE:
			idlePara = 0;
			break;
		case WeaponType.CROSSBOW:
			idlePara = 0;
			break;
		case WeaponType.MELEE:
			idlePara = 0;
			break;
		case WeaponType.FLY:
			idlePara = FLY_IDLE;
			break;
		case WeaponType.SPEAR:
			idlePara = SPEAR_IDLE;
			break;
		case WeaponType.SWORD:
			idlePara = TH_SWORD_IDLE;
			break;
		default:
			break;
		}

		if (idlePara != 0) {
			_animator.SetBool (idlePara, true);
		}
	}
	public void CancelIdle()
	{
		if (idlePara != 0) {
			_animator.SetBool (idlePara, false);
		}
		DisableAnimation ();
		switch (weaponType) {
		case WeaponType.NONE:
			idlePara = 0;
			break;
		case WeaponType.CROSSBOW:
			idlePara = 0;
			break;
		case WeaponType.MELEE:
			idlePara = 0;
			break;
		case WeaponType.FLY:
			idlePara = FLY_IDLE;
			break;
		case WeaponType.SPEAR:
			idlePara = SPEAR_IDLE;
			break;
		case WeaponType.SWORD:
			idlePara = TH_SWORD_IDLE;
			break;
		default:
			break;
		}

		if (idlePara != 0) {
			_animator.SetBool (idlePara, false);
		}
	}
	[X]
	public void HoldonSkill(bool isBegin)
	{
		if (isBegin) {
			CancelIdle ();
			EnableAnimationBool (CHOP_TREE);
		} else {
			Idle ();
		}
	}

	[X]
	public void Walk()
	{
		int animPara = 0;

		switch (weaponType) {
		case WeaponType.NONE:
		case WeaponType.CROSSBOW:
		case WeaponType.MELEE:
			animPara = WALK;
			break;
		case WeaponType.FLY:
			animPara = FLY_FORWARD;
			break;
		case WeaponType.SPEAR:
			animPara = SPEAR_WALK;
			break;
		case WeaponType.SWORD:
			animPara = TH_SWORD_WALK;
			break;
		default:
			break;
		}
		EnableAnimationBool (animPara);
	}
	[X]
	public void Run()
	{
		int animPara = 0;

		switch (weaponType) {
		case WeaponType.NONE:
		case WeaponType.CROSSBOW:
		case WeaponType.MELEE:
			animPara = RUN;
			break;
		case WeaponType.FLY:
			animPara = FLY_FORWARD;
			break;
		case WeaponType.SPEAR:
			animPara = SPEAR_RUN;
			break;
		case WeaponType.SWORD:
			animPara = TH_SWORD_RUN;
			break;
		default:
			break;
		}
		EnableAnimationBool (animPara);
	}
	[X]
	public void Attack()
	{
		int animPara = 0;

		switch (weaponType) {
		case WeaponType.NONE:
			{
				XLogger.Log ("The Animator's weapon was null!");
			}
			break;
		case WeaponType.MELEE:
			animPara = RandomInArray(MELEE_RIGHT_ATTACK_01, MELEE_RIGHT_ATTACK_02, MELEE_RIGHT_ATTACK_03);
			break;
		case WeaponType.CROSSBOW:
			animPara = CROSSBOW_LEFT_ATTACK;
			break;
		case WeaponType.FLY:
			animPara = RandomInArray(FLY_RIGHT_ATTACK_01, FLY_RIGHT_ATTACK_02, FLY_RIGHT_ATTACK_03);
			break;
		case WeaponType.SPEAR:
			animPara = SPEAR_MELEE_ATTACK_01;
			break;
		case WeaponType.SWORD:
			animPara = TH_SWORD_ATTACK_01;
			break;
		default:
			break;
		}
		EnableAnimationTrigger (animPara);
	}
	[X]
	public void Die()
	{
		int animPara = 0;

		switch (weaponType) {
		case WeaponType.NONE:
			{
				XLogger.Log ("The Animator's weapon was null!");
			}
			break;
		case WeaponType.MELEE:
			animPara = DIE;
			break;
		case WeaponType.CROSSBOW:
			animPara = DIE;
			break;
		case WeaponType.FLY:
			animPara = FLY_DIE;
			break;
		case WeaponType.SPEAR:
			animPara = SPEAR_DIE;
			break;
		case WeaponType.SWORD:
			animPara = TH_SWORD_DIE;
			break;
		default:
			break;
		}
		EnableAnimationTrigger (animPara);
	}

	[X]
	public void Reborn()
	{
		_animator.Play ("Idle");
	}
	[X]
	public void AttackCri()
	{
		int animPara = 0;

		switch (weaponType) {
		case WeaponType.NONE:
			{
				XLogger.Log ("The Animator's weapon was null!");
			}
			break;
		case WeaponType.MELEE:
			animPara = MELEE_JUMP_RIGHT_ATTACK_03;
			break;
		case WeaponType.CROSSBOW:
			animPara = CROSSBOW_RIGHT_ATTACK;
			break;
		case WeaponType.FLY:
			animPara = FLY_RIGHT_LEFT_ATTACK_01;
			break;
		case WeaponType.SPEAR:
			animPara = SPEAR_MELEE_ATTACK_02;
			break;
		case WeaponType.SWORD:
			animPara = TH_SWORD_ATTACK_02;
			break;
		default:
			break;
		}
		EnableAnimationTrigger (animPara);
	}

	int RandomInArray(params int[] array)
	{
		int result = 0;
		int index = Random.Range (0, array.Length);
		if (array.Length > index) {
			return array[index];
		}
		return result;
	}
	[X]
	public void Play(AnimationType animationType)
	{
		string std = animationType.ToString ();
		std = std.Replace ("_", " ");
		_animator.Play (std);
	}
	void EnableAnimationBool(int animPara)
	{
		DisableAnimation ();
		_animator.SetBool (animPara, true);
		lastBoolPara = animPara;
	}
	void DisableAnimation()
	{
		if (lastBoolPara != 0) {
			_animator.SetBool (lastBoolPara, false);
			lastBoolPara = 0;
		}
	}
	void EnableAnimationTrigger(int animPara)
	{
		_animator.SetTrigger (animPara);
	}

	public Transform GetSkeleton(HandsType handType)
	{
		if (handType == HandsType.LEFT) {
			return transform.Find (LEFT_HAND);
		}
		return transform.Find (RIGHT_HAND);
	}


	public void TriggerSkill()
	{
		human.EmitSkill ();
	}

	public void BindSkill()
	{
		RuntimeAnimatorController rac = _animator.runtimeAnimatorController;
		AnimationClip[] clips = rac.animationClips;
		int clipsCount = clips.Length;
		var count = PrototypeManager.instance.totalDictionary.Count;

		for (int i = 0; i < clipsCount; ++i) {
			AnimationClip c = clips[i];
			c.events = null;
			var clipName = c.name;
			if (PrototypeManager.instance.totalDictionary.ContainKey (clipName)) {
				XLogger.Log (clipName);
				var time = PrototypeManager.instance.totalDictionary [clipName];

				AnimationEvent ae = new AnimationEvent();
				ae.time = Mathf.Min( c.length, time );
				ae.functionName = "TriggerSkill";
				c.AddEvent (ae);
			}
		}
	}
}
