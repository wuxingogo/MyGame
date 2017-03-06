using UnityEngine;
using wuxingogo.Runtime;


public class UnityFollowTargetCurve : XMonoBehaviour
{
	public Transform moveTarget = null;
	public Transform target = null;
	/// <summary>
	/// 目标,是否打中
	/// </summary>
	public System.Action OnHitEvent = null;

	[Space]
	public Vector3 targetPos = Vector3.zero;
	
	[SerializeField][Disable] float elapseTimer = 0;
	[SerializeField][Disable] float Timer = 1;
	[SerializeField][Disable] float oldDistance = 0;

	public AnimationCurve xOffsetCurve = AnimationCurve.Linear (0, 0, 1, 0);
	public AnimationCurve yOffsetCurve = AnimationCurve.Linear (0, 0, 1, 0);
	public AnimationCurve zOffsetCurve = AnimationCurve.Linear (0,1,1,1);

	float oldSpeed = 0;
	public float speedZ = 1;

	void Start()
	{
		ResetTarget();
	}

	public void ClearPos()
	{
		moveTarget.localPosition = Vector3.zero;
		moveTarget.localRotation = Quaternion.identity;
		moveTarget.localScale = Vector3.one;
	}
	[X]
	void AlignFinishGo()
	{
		if( null != target ) {
			targetPos = target.position;

			//			var dir = finishPosition - this.transform.position;
			//			if(dir.sqrMagnitude < 0.5)
			//			{
			//				dir = transform.forward;
			//			}
			//			this.transform.forward = dir.normalized;

		}
	}
	bool isInit = false;
	[X]
	public void ResetTarget()
	{
		isInit = true;
		if( null != target ) {
			targetPos = target.position;
			moveTarget.localPosition = Vector3.zero;
			//			var dir = finishPosition - this.transform.position;
			//			if(dir.sqrMagnitude < 0.5)
			//			{
			//				dir = transform.forward;
			//			}
			//			this.transform.forward = dir.normalized;

		}

		int last = yOffsetCurve.keys.Length - 1;
		var frame = yOffsetCurve.keys[last];
		Timer = frame.time;
		oldDistance = ( target.position - moveTarget.position ).magnitude;
		oldSpeed = oldDistance / frame.time;
		this.transform.forward = ( target.position - this.transform.position ).normalized;
		elapseTimer = 0;
	}

	public void Update()
	{
		var delta = Time.deltaTime* speedZ;
		if (elapseTimer + delta > Timer)
			elapseTimer = Timer;
		else
			elapseTimer += delta;
		
		Vector3 result = GetNextPoint(elapseTimer);

		moveTarget.localPosition = result;
		//XLogger.Break();


		if( elapseTimer == Timer ) {
			if( OnHitEvent != null )
				OnHitEvent();
			Finish();
		}
	}

//	void OnDrawGizmos()
//	{
//		if( !isInit )
//			ResetTarget();
//		var sourceColor = Gizmos.color;
//		var sourceMatrix = Gizmos.matrix;
//		Gizmos.matrix = transform.localToWorldMatrix;
//		var sourcePoint = Vector3.zero;
//		bool firstPoint = false;
//		for( float t = elapseTimer; t < Timer; t += 0.1f * speedZ ) {
//			var p = GetNextPoint( t );
//			if( !firstPoint ) {
//				firstPoint = true;
//			} else {
//				Gizmos.DrawLine( sourcePoint, p );
//			}
//			sourcePoint = p;
//			//			Gizmos.DrawCube( p + transform.position, Vector3.one );
//		}
//
//		Gizmos.matrix = sourceMatrix;
//		Gizmos.color = sourceColor;
//	}

	protected Vector3 GetNextPoint(float timer)
	{
		if( target != null )
		{
			float distance = ( target.position - this.transform.position ).magnitude;
			float newSpeed = distance / oldDistance * oldSpeed;
			transform.forward = ( target.position - this.transform.position ).normalized;

			return new Vector3( xOffsetCurve.Evaluate( timer ), yOffsetCurve.Evaluate( timer ), timer * newSpeed );
		}
		return  new Vector3( xOffsetCurve.Evaluate( timer ), yOffsetCurve.Evaluate( timer ), oldSpeed * timer );

	}
	// Forecast result point
	public Vector3 Forecast()
	{
		return targetPos;
	}

	private void Finish()
	{

		this.enabled = false;
		//		Destroy( this.gameObject );
	}
}