//
// AngleTest.cs
//
// Author:
//       wuxingogo <>
//
// Copyright (c) 2017 wuxingogo
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using wuxingogo.Runtime;
using UnityEngine;

public class AngleTest : XMonoBehaviour
{
	public Transform t1 = null;

	public float Radius = 5;
	public Color color = Color.red;
	public Vector3 dir = Vector3.zero;
	private static int x;
	[X]
	public static void P()
	{
		XLogger.Log ("P");
	}
	[X]
	public static int v{
		get{
			return x;
		}set{
			x = value;
		}
	}
	[X]
	public float angle{
		get{
			var p1 = transform.position;
			var p2 = t1.position;
			var v = p2 - p1;
			dir = (v.normalized) * Radius;
			return 0;
		}
	}
	void OnDrawGizmos ()
	{
		var m = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;

		var c = Gizmos.color;
		Gizmos.color = color;


		Gizmos.DrawSphere (Vector3.zero, Radius);


		Gizmos.color = c;
		Gizmos.matrix = m;

		if (dir != Vector3.zero) {
			Gizmos.DrawCube (dir + transform.position, Vector3.one);
		}
	}
}


