using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochCurve : MonoBehaviour {
	public int depth = 5;

	void OnDrawGizmos (){
		var c = Gizmos.color;
		Gizmos.color = Color.red;
		var m = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;
		if (depth > -1 && depth < 6) {
			DrawKoch (depth, 20, 280, 280, 280);
			DrawKoch (depth, 280, 280, 150, 20);
			DrawKoch (depth, 150, 20, 20, 280);
		}
		//DrawCube ();
		Gizmos.color = c;
		Gizmos.matrix = m;
	}

	void DrawKoch( int lev, int x1, int y1, int x5, int y5)
	{
		int deltaX, deltaY, x2, y2, x3, y3, x4, y4;

		if (lev == 0)
		{
			//g.DrawLine(p, x1, y1, x5, y5);
			var fromPoint = new Vector3(x1,y1);
			var toPoint = new Vector3 (x5, y5);
			Gizmos.DrawLine (fromPoint, toPoint);
		
		}
		else
		{
			deltaX = x5 - x1;
			deltaY = y5 - y1;

			x2 = x1 + deltaX / 3;
			y2 = y1 + deltaY / 3;

			x3 = (int)(0.5 * (x1 + x5) + Mathf.Sqrt(3) * (y1 - y5) / 6);
			y3 = (int)(0.5 * (y1 + y5) + Mathf.Sqrt(3) * (x5 - x1) / 6);

			x4 = x1 + 2 * deltaX / 3;
			y4 = y1 + 2 * deltaY / 3;

			DrawKoch( lev - 1, x1, y1, x2, y2);
			DrawKoch( lev - 1, x2, y2, x3, y3);
			DrawKoch( lev - 1, x3, y3, x4, y4);
			DrawKoch( lev - 1, x4, y4, x5, y5);
		}
	}
}
