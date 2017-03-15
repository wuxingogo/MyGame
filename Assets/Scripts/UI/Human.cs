
using UnityEngine;
using System.Collections;
using Apex.Messages;
using Apex.Services;
using Apex.Units;
using wuxingogo.Runtime;
using System.Collections.Generic;
using System;
using HighlightingSystem;
using UnityEngine.Networking;


public class Human : NetworkBehaviour, IHandleMessage<UnitNavigationEventMessage>{

	public GameObject rendererable = null;
	AnimationCtr _animationCtr = null;
	public AnimationCtr Animator{
		get{
			return _animationCtr;
		}
	}
	IUnitFacade facade = null;
	public string TeamTag = "Units_0";
	public int teamID = 0;
	public TeamCatriay teamCatriay = TeamCatriay.Team1;
	[System.NonSerialized]
	[X]
	public bool isMine = false;
	[System.NonSerialized]
	[X]
	public bool isNetInit = false;

	[System.NonSerialized]
	[X]
	public bool isLocalInit = false;
	
	public Human followTarget = null;
	public Vector3 goalPoint = Vector3.zero;
	public SkillCastStructure nextSkill = null;
	public SkillHoldOn holdOnSkill = null;

	public virtual void OnInit()
	{
		gameObject.SendMessage ("OnHumanFinish", this, SendMessageOptions.DontRequireReceiver);
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

	public virtual void OnFinishMove(UnitNavigationEventMessage message){
	}
	public virtual void OnBeginMove(UnitNavigationEventMessage message){
	}
	public void Handle (UnitNavigationEventMessage message)
	{
		if (message.entity != gameObject ) {
			return;
		}
		switch( message.eventCode )
		{
		case UnitNavigationEventMessage.Event.BeginPath:
			_animationCtr.Walk ();
			OnBeginMove (message);
			break;
		case UnitNavigationEventMessage.Event.DestinationReached:
		case UnitNavigationEventMessage.Event.WaypointReached:
		case UnitNavigationEventMessage.Event.NodeReached:
			{

				//XLogger.Log( string.Format( "Unit '{0}' ({1}) reports: {2} at position: {3}.", message.entity.name, message.entity.transform.position, message.eventCode, message.destination ) );
				_animationCtr.Idle ();
				OnFinishMove (message);
				break;
			}

		case UnitNavigationEventMessage.Event.StoppedDestinationBlocked:
		case UnitNavigationEventMessage.Event.StoppedNoRouteExists:
		case UnitNavigationEventMessage.Event.StoppedRequestDecayed:
		case UnitNavigationEventMessage.Event.Stuck:
			{
				_animationCtr.Idle ();
				OnFinishMove (message);
				break;
			}
		}

	}

	#endregion



	public SkillBase currentSkill = null;
	public List<SkillBase> totalSkill = new List<SkillBase>();
	void Update()
	{
		#if UNITY_NETWORK
		if( !isNetInit )
		{
			return;
		}
		if(!isLocalInit)
		{
			OnInit ();
			isLocalInit = true;
		}
		if (!isMine)
			return;
		#endif
		SinglePlayerUpdate ();
	}

	public void OnNetworkFireSkillWithDirection(int skillIndex, Vector3 direction)
	{
		currentSkill = totalSkill [skillIndex];
		currentSkill.CastDirection (direction);
		nextSkill = new SkillCastStructure ();
		nextSkill.skillBase = totalSkill[skillIndex];

	}

	public void OnNetworkFireSkillWithHuman(int skillIndex, Human human)
	{
		currentSkill = totalSkill [skillIndex];
		currentSkill.CastHuman (human);
		nextSkill = new SkillCastStructure ();
		nextSkill.skillBase = totalSkill[skillIndex];
		nextSkill.human = clickedHuman;
	}

	public void OnNetworkFireSkillWithPoint(int skillIndex, Vector3 point)
	{
		currentSkill = totalSkill [skillIndex];
		currentSkill.CastPoint (point);
		nextSkill = new SkillCastStructure ();
		nextSkill.skillBase = totalSkill[skillIndex];
		nextSkill.human = clickedHuman;
	}

	public virtual void SinglePlayerUpdate()
	{
		if (isMine && hangTime <= 0) {
			var point = GetMousePoint();
			for (int i = 0; i < totalSkill.Count; i++) {
				if (Input.GetKeyUp (totalSkill [i].keyCode) && totalSkill [i].CanRelease() ) {

					currentSkill = totalSkill [i];
					currentSkill.CastDirection (transform.forward);

					if (currentSkill.caseType == CastType.Immatie) {
						nextSkill = new SkillCastStructure ();
						nextSkill.skillBase = currentSkill;
					}
					else{
						nextSkill = null;
						InputManager.Instance.SetSelected ();
					}
				}
			}

			if (Input.GetButtonUp("Fire1")) {

				if (currentSkill != null) {
					if (currentSkill.caseType == CastType.Point) {
						currentSkill.CastPoint (point);
						nextSkill = new SkillCastStructure ();
						nextSkill.skillBase = currentSkill;
						nextSkill.point = point;
					} else if (currentSkill.caseType == CastType.Target && clickedHuman != null && currentSkill.CanReleaseAtHuman (clickedHuman)) {
						currentSkill.CastHuman (clickedHuman);
						nextSkill = new SkillCastStructure ();
						nextSkill.human = clickedHuman;
						nextSkill.skillBase = currentSkill;
					}
					InputManager.Instance.SetUnSelected ();
				} 
			} else if(Input.GetButtonUp ("Fire2")){
				CancelFollow ();
				CancleHoldOnSkill ();
				if (clickedHuman != null && clickedHuman != this) {
					Follow (clickedHuman);
					clickedHuman = null;
				} else {
					CmdMoveTo (point);
				}
			}

			if (Input.GetKeyUp (KeyCode.S)) {
				StopForIdle ();
			}
		}
		if (followTarget != null)
			CmdMoveTo (followTarget.transform.position);
		for (int i = 0; i < totalSkill.Count; i++) {
			totalSkill [i].UpdateCoolCD ();
		}

		if (isHangs) {
			hangTime -= Time.deltaTime;
			if(hangTime <= 0){
				isHangs = false;
				OnHangsEnded ();
			}
		}

		if (currentSkill != null && !currentSkill.isCacheSkill && currentSkill.holdOnTime <= 0) {
			CancleHoldOnSkill ();
		}

		OnUpdate ();
	}
	public void CmdMoveTo(Vector3 point)
	{
		NetworkListener.Instance.CmdMoveTo (this.netId.Value, point);
	}
	public void MoveTo(Vector3 point)
	{
		if (!isDead && !isHangs) {
			facade.MoveTo (point, false);
			goalPoint = point;
		}
	}
	public void SetCurrentSkill()
	{
		
	}
	public virtual void OnUpdate()
	{
	}
	[X]
	public void StopForIdle()
	{
		CancelFollow ();
		FinishSkill ();
		CancleHoldOnSkill ();
		Stop ();
	}

	public void CancleHoldOnSkill()
	{
		if (holdOnSkill != null) {

			holdOnSkill.OnSkillEnd ();

			currentSkill = null;
			nextSkill = null;
			holdOnSkill = null;
		}
	}

	public void RpcMoveTo(Vector3 point){
		if (!isDead && !isHangs) {
			facade.MoveTo (point, false);
			goalPoint = point;
		}
	}

	public void Stop()
	{
		facade.Stop ();
		_animationCtr.Idle ();
	}
	public float hangTime = 0f;
	public bool isHangs = false;
	public virtual void Hangs(float time){
		hangTime += time;
		if (hangTime > 0) {
			StopForIdle ();
			isHangs = true;
			Animator.Die ();
		}
	}
	public virtual void OnHangsEnded()
	{
		facade.Resume ();
		Animator.Reborn ();
	}

	public void FaceTo(Vector3 point)
	{
		if (!isHangs) {
			CancelFollow ();
			transform.forward = (point - transform.position).normalized;
		}
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
			InputManager.Instance.SetUnSelected ();

		}
		nextSkill = null;
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
			currentSkill.isCacheSkill = true;
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
		if (nextSkill != null) {
			var newSkill = nextSkill.skillBase.CreatePrefab(transform,nextSkill.skillBase.gameObject).GetComponent<SkillBase>();
			nextSkill.skillBase.Cooling ();

			newSkill.gameObject.tag = TeamTag;
			newSkill.transform.parent = null;
			newSkill.isCacheSkill = false;
			newSkill.gameObject.SetActive (true);
			newSkill.targetHuman = nextSkill.human;
			newSkill.castPoint = nextSkill.point;
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
	[X]
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
		return dataSystem.SpellImmunity || dataSystem.Immunity;
	}

	void OnDrawGizmosSelected (){
		if(goalPoint != Vector3.zero)
			Gizmos.DrawSphere (goalPoint, 1);
	}



	#if UNITY_NETWORK
	[X]
	[Command]
	public void CmdSynchronizePoint(Vector3 point)
	{

		if (isServer) 
			RpcSynchronizePoint (point);

	}
	[X]
	[ClientRpc]
	public void RpcSynchronizePoint(Vector3 point){
		transform.position = point;
	}
	#else
	public void CmdSynchronizePoint(Vector3 point)
	{
	transform.position = point;
	}
	#endif
}
