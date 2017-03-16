//
// Player.cs
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

using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;
using wuxingogo.Runtime;
using System.Collections.Generic;



public class Player : Human
{
	public static List<Player> allPlayer = new List<Player>();

	public override void OnEnable()
	{
		base.OnEnable ();
		if(!allPlayer.Contains(this))
			allPlayer.Add (this);
	}

	public override void OnDisable()
	{
		base.OnDisable ();
		if(allPlayer.Contains(this))
			allPlayer.Remove (this);
	}
	public override void OnDestroy ()
	{
		base.OnDestroy ();
		if(allPlayer.Contains(this))
			allPlayer.Remove (this);
	}

	public override void OnInit ()
	{
		

		var listeners = FindObjectsOfType<NetworkListener> ();
		if (NetworkListener.Instance == null) {
			isMine = false;
			return;
		}
		for (int i = 0; i < listeners.Length; i++) {
			if (listeners [i].isLocalPlayer && listeners[i].teamID == teamID) {
				isMine = true;
			}
		}
		if (isMine) {
			var totalPoints = ResourceManager.instance.bornPoint;
			var index = Random.Range (0, totalPoints.Count - 1);
			var p = totalPoints [index];
			NetworkListener.Instance.CmdSynchronizePoint (this.netId.Value, p);
			gameObject.name = "Player_" + netId.ToString();

			var t = FindObjectOfType<FollowTarget> ();
			t.SetTarget (transform);
		}

		base.OnInit ();

	}

}


