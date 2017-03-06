using UnityEngine;
using System.Collections;
using Apex.Messages;
using Apex.Services;
using Apex.Units;
using wuxingogo.Runtime;
using System.Collections.Generic;
using System;
using HighlightingSystem;


public class Human : XMonoBehaviour, IHandleMessage<UnitNavigationEventMessage>{

	public GameObject rendererable = null;
	AnimationCtr _animationCtr = null;
	public AnimationCtr Animator{
		get{
			return _animationCtr;
		}
	}
	IUnitFacade facade = null;
	public string TeamTag = "Units_0";
	public TeamCatriay teamCatriay = TeamCatriay.Team1;
	public bool isMine = false;
	public Human followTarget = null;
	public virtual void OnInit()
	{
		
	}
	void Start()
	{
		rendererable = GameObjectUtil.CreatePrefab (transform, rendererable);
		_animationCtr = rendererable.AddComponent<AnimationCtr> ();
		_animationCtr.human = this;
		_animationCtr.weaponType = WeaponType.SWORD;
		_animationCtr.Prepare ();
		_animationCtr.Idle ();
		facade = GameServices.gameStateManager.GetUnitFacade (gameObject);
		facade.SetPreferredSpeed (30);

		highlighterConstant = GetComponent<HighlighterConstant> ();
		highlighter = GetComponent<Highlighter> ();
		HighlightingOff();

		CacheSkill ();

		OnInit ();
	}

	public Transform GetSkeleton(HandsType handsType)
	{
		return _animationCtr.GetSkeleton (handsType);
	}

	void OnEnable()
	{
		GameServices.messageBus.Subscribe( this );
	}
	void OnDisable()
	{
		GameServices.messageBus.Unsubscribe( this );
	}

	#region IHandleMessage implementation

	public void Handle (UnitNavigationEventMessage message)
	{
		if (message.entity != gameObject ) {
			return;
		}
		switch( message.eventCode )
		{
		case UnitNavigationEventMessage.Event.BeginPath:
			_animationCtr.Walk ();
			break;
		case UnitNavigationEventMessage.Event.DestinationReached:
		case UnitNavigationEventMessage.Event.WaypointReached:
		case UnitNavigationEventMessage.Event.NodeReached:
			{

				//XLogger.Log( string.Format( "Unit '{0}' ({1}) reports: {2} at position: {3}.", message.entity.name, message.entity.transform.position, message.eventCode, message.destination ) );
				_animationCtr.Idle ();
				break;
			}

		case UnitNavigationEventMessage.Event.StoppedDestinationBlocked:
		case UnitNavigationEventMessage.Event.StoppedNoRouteExists:
		case UnitNavigationEventMessage.Event.StoppedRequestDecayed:
		case UnitNavigationEventMessage.Event.Stuck:
			{
				_animationCtr.Idle ();
				break;
			}
		}

	}

	#endregion



	public SkillBase currentSkill = null;
	public List<SkillBase> totalSkill = new List<SkillBase>();
	void Update()
	{
		
		if (isMine) {
			var point = GetMousePoint();
			for (int i = 0; i < totalSkill.Count; i++) {
				if (Input.GetKeyUp (totalSkill [i].keyCode) && totalSkill [i].CanRelease() ) {
					
					currentSkill = totalSkill [i];
					currentSkill.Prepare ();
					InputManager.Instance.SetSelected ();
				}
			}

			if (Input.GetButtonUp("Fire1")) {
				
				if (currentSkill != null) {
					if (currentSkill.caseType == CastType.Point) {
						castSkillPoint = point;
						currentSkill.CastPoint (point);
					} else if (currentSkill.caseType == CastType.Target) {
						currentSkill.CastHuman (clickedHuman);
					}
					InputManager.Instance.SetUnSelected ();
				} 
			} else if(Input.GetButtonUp ("Fire2")){
				CancelFollow ();

				if (clickedHuman != null && clickedHuman != this) {
					Follow (clickedHuman);
					clickedHuman = null;
				} else {
					MoveTo (point);
				}
			}

			if (Input.GetKeyUp (KeyCode.S)) {
				StopForIdle ();
			}
		}
		if (followTarget != null)
			MoveTo (followTarget.transform.position);
		for (int i = 0; i < totalSkill.Count; i++) {
			totalSkill [i].UpdateCoolCD ();
		}
		OnUpdate ();
	}
	public virtual void OnUpdate()
	{
	}
	public Vector3 castSkillPoint = Vector3.zero;
	[X]
	public void StopForIdle()
	{
		CancelFollow ();
		FinishSkill ();
		Stop ();
	}
	public void MoveTo(Vector3 point)
	{
		facade.MoveTo (point, false);
	}
	public void Stop()
	{
		facade.Stop ();
		_animationCtr.Idle ();
	}
	public void FaceTo(Vector3 point)
	{
		CancelFollow();
		transform.forward = (point - transform.position).normalized;
	}
	public Human clickedHuman = null;
	Vector3 GetMousePoint()
	{
		var p = Input.mousePosition;
		var ray = Camera.main.ScreenPointToRay(p);
		RaycastHit hit;//
		int terrianAndUnits = (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Units"));
		if(Physics.Raycast(ray, out hit, 1000, terrianAndUnits))
		{
			var v = hit.point;//得到碰撞点的坐标

			if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Units")) {
				if (clickedHuman != null) {
					clickedHuman.HighlightingOff ();
				}
				clickedHuman = hit.transform.gameObject.GetComponent<Human> ();
				if (clickedHuman == this)
					clickedHuman.HighlightingOn (Color.green);
				else {
					var isFriends = TeamManager.instance.GetRelation (this.teamCatriay, clickedHuman.teamCatriay);
					if (isFriends) {
						clickedHuman.HighlightingOn (Color.yellow);
					} else {
						clickedHuman.HighlightingOn (Color.red);
					}
				}

			}
			return v;
		}
		return Vector3.zero;
	}
	public void FinishSkill()
	{
		if (currentSkill != null) {
			currentSkill.PrepareCancel ();
			currentSkill = null;
			InputManager.Instance.SetUnSelected ();
		}
	}
	public Action OnDestroyAction = null;
	void DestroyMyself()
	{
		StopForIdle ();
		if (OnDestroyAction != null) {
			OnDestroyAction ();
		}
		XLogger.Log ("Destroy Myself");
	}
	
	void CacheSkill()
	{
		for (int i = 0; i < totalSkill.Count; i++) {
			var currentSkill = totalSkill [i];
			var go = Instantiate (currentSkill.gameObject);
			go.tag = TeamTag;
			currentSkill = go.GetComponent<SkillBase>();
			currentSkill.human = this;
			currentSkill.transform.SetParent (transform);
			currentSkill.gameObject.SetActive (false);
			currentSkill.ResetCD ();
			totalSkill [i] = currentSkill;
		}

	}

	public void Follow(Human human)
	{
		CancelFollow ();
		followTarget = human;
		followTarget.OnDestroyAction += OnFollowTargetDestroy;
	}
	public void CancelFollow()
	{
		
		if (followTarget != null) {
			followTarget.OnDestroyAction -= OnFollowTargetDestroy;
		}
		followTarget = null;
	}
	void OnFollowTargetDestroy()
	{
		followTarget = null;
	}
	public void CastSkill()
	{
		if (currentSkill != null) {
			var newSkill = currentSkill.CreatePrefab(transform,currentSkill.gameObject);
			newSkill.gameObject.tag = TeamTag;
			newSkill.transform.parent = null;
			newSkill.gameObject.SetActive (true);
			var skillBase = newSkill.GetComponent<SkillBase> ();
			skillBase.targetHuman = clickedHuman;
			FinishSkill ();
		}
	}
	private HighlighterConstant highlighterConstant = null;
	private Highlighter highlighter = null;

	public void HighlightingOnMouseHover()
	{
		if (isMine) {
			HighlightingOn (Color.green);
		} else {
			var isFriends = TeamManager.instance.GetRelation (this.teamCatriay, clickedHuman.teamCatriay);
			if (isFriends) {
				HighlightingOn (Color.yellow);
			} else {
				HighlightingOn (Color.red);
			}
		}
	}
	[X]
	public void HighlightingOn(Color color)
	{
		highlighterConstant.color = color;
		highlighterConstant.enabled = false;
		highlighterConstant.enabled = true;
		highlighter.On (color);
	}
	[X]
	public void HighlightingOff()
	{
		highlighter.Off ();
	}

	public DataSystem dataSystem = null;
	private bool _isDead = false;
	public bool isDead{
		get{
			return _isDead;
		}
		set{
			_isDead = value;
			DisableCollision (_isDead);
		}
	}
	public void DisableCollision(bool disable)
	{
		GetComponent<Collider> ().enabled = !disable;
	}
	public bool IsSkillImmunited()
	{
		return !dataSystem.SpellImmunity || !dataSystem.Immunity;
	}
}
