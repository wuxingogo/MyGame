using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using wuxingogo.Runtime;

public class HPComponent : XMonoBehaviour {

	public Slider slider = null;
	public Transform followTarget = null;
	public Vector3 offset = Vector3.zero;
	[X]
	public void ChangeHP(float hp, float maxHP)
	{
		slider.value = hp / maxHP * 100.0f;
	}

	void Update()
	{
		RectTransform hpRect = transform as RectTransform;
		var p = Camera.main.WorldToScreenPoint (followTarget.position);
		p = UIManager.Instance.UICamera.WorldToScreenPoint (followTarget.position);
		hpRect.position = p + offset;
	}
}
