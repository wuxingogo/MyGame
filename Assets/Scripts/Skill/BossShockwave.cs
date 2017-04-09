//
// BossShockwave.cs
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
using UnityEngine;


public class BossShockwave : SkillBase
{
	public GameObject throwBall = null;
	public GameObject collision = null;
	public float energy = 10;
	void Start()
	{
		human.gameObject.SetActive (false);

		var skelegon = human.GetSkeleton (HandsType.RIGHT);
		throwBall = CreatePrefab(throwBall);
		throwBall.SetActive (true);
		throwBall.transform.position = skelegon.transform.position;
		throwBall.transform.forward = human.transform.forward;
		var curve = throwBall.GetComponent<UnityExtendCurve> ();
		curve.onFinish = () =>{ 
			DestroyGameObject(throwBall, 0.0f);
			DestroyGameObject (gameObject, 0.1f);
			var pos = throwBall.transform.position;
			human.transform.position = new Vector3(pos.x, 0, pos.z);
			human.gameObject.SetActive (true);
			human.Animator.HoldonSkill(false);
		};

		var messager = throwBall.AddComponent<SkillColliderMessager> ();
		messager.onCollisionEnter = OnBallCollision;
	}

	void OnBallCollision(SkillColliderMessager messager, Collider other)
	{
		var targetHuman = other.GetComponent<Human> ();
		if (targetHuman != null && CanTarget (targetHuman)) {

			//targetHuman.dataSystem.Damage (energy);
			NetworkListener.Instance.CmdDamage(targetHuman.netId.Value, energy);
			if (collision != null) {
				var go = CreatePrefab (collision);
				go.SetActive (true);
				go.transform.position = other.transform.position;

				DestroyGameObject (go, 5);
			}
		}
	}
}

