//
// DataSystem.cs
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
using UnityEngine.Networking;


public class DataSystem : XMonoBehaviour
{
	public float HP = 100;
	public float MaxHP = 100;

	public float MoveSpeed = 5;

	public bool Stunning = false;
	public bool SpellImmunity = false;
	public bool Immunity = false;

	public HPComponent hpSlider = null;
	public Human human = null;

	void Start()
	{
		hpSlider = GameObjectUtil.CreatePrefab<HPComponent>(HPPanel.Instance.transform, "Prefabs/UI/HPSlider");
		hpSlider.followTarget = transform;

	}

	[X]
	public void Damage(float damage)
	{
		if (!human.isDead) {
			HP = Mathf.Max (0, HP - damage);
			hpSlider.ChangeHP (HP, MaxHP);

			if (HP == 0) {
				human.isDead = true;
				human.Animator.Die ();
			}
		}
	}

	[X]
	public void Reborn()
	{
		if (human.isDead) {
			human.isDead = false;
			human.Animator.Reborn ();
		}
		HP = 1;
	}
	[X]
	public void RecoveryHP(float energy)
	{
		if (human.isDead) {
			return;
		}
		HP = Mathf.Min (MaxHP, HP + energy);
		hpSlider.ChangeHP (HP, MaxHP);
	}

	void OnDestroy()
	{
		Destroy (hpSlider.gameObject);
	}


}


