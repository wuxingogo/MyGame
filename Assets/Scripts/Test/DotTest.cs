using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wuxingogo.Runtime;

public class DotTest : XMonoBehaviour {

	// Use this for initialization
	public Transform v1;
	public Transform v2;
	public Transform v3;
	[X]
	public Vector3 lhs{
		get{
			return v1.position;
		}
		set{
			v1.position = value;
		}
	}
	[X]
	public Vector3 rhs{
		get{
			return v2.position;
		}set{
			v2.position = value;
		}
	}

	[X]
	public float MDot{
		get{
			var lhs = this.lhs;
			var rhs = this.rhs;
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}
	}

	[X]
	public float UDot{
		get{
			var lhs = this.lhs;
			var rhs = this.rhs;
			return Vector3.Dot(lhs, rhs);
		}
	}

	[X]
	public float Angle{
		get{
			return Vector3.Angle (lhs, rhs);
		}
	}

	[X]
	public float OAngle{
		get{
			var r = Vector3.Dot (lhs.normalized, rhs.normalized);
			var radians = Mathf.Acos (r);
			return radians * Mathf.Rad2Deg;
		}
	}

	[X]
	public Vector3 DotAngle{
		get{
			var r = Vector3.Scale( lhs, rhs);
			var b = Vector3.Scale(lhs.normalized, rhs.normalized);
			return new Vector3(r.x / b.x, r.y / b.y, r.z / b.z);
		}
	}
	[X]
	public float AngleX{
		get{
			var a = Vector3.Dot (lhs, rhs);
			var b = lhs.magnitude * rhs.magnitude;
			return Mathf.Acos( a / b) * Mathf.Rad2Deg;
		}
	}
//
	[X]
	public float Vertical{
		get{
			return (lhs.x * rhs.x) + (lhs.y * rhs.y) + (lhs.z * rhs.z);
		}
	}

	[X]
	public Vector3 Cross{
		get{
			return Vector3.Cross (lhs, rhs);
		}
	}
	[X]
	public bool Clockwise{
		get{
			var ab = v2.position - v1.position;
			var bc = v3.position - v2.position;
			var k = Vector3.Cross (ab, bc);
			var cameraDir = Camera.main.transform.forward;
			var t = Vector3.Dot (k, -cameraDir);
			if (t > 0) {
				return false;
			} 
			return true;
		}
	}
	[X]
	public bool CounterClockwise{
		get{
			var ac = v3.position - v1.position;
			var cb = v2.position - v3.position;
			var k = Vector3.Cross (ac, cb);
			var cameraDir = Camera.main.transform.forward;
			var t = Vector3.Dot (k, -cameraDir);
			if (t > 0) {
				return false;
			} 
			return true;
		}
	}
}
