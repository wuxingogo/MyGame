using System;
using UnityEngine;
using wuxingogo.Runtime;

public class WeaponBehaivour : XMonoBehaviour
{
	public bool isInScene = false;
	void Start()
	{
		isInScene = true;
	}
	[X]
	public void BindModel(Transform parent)
	{
		transform.SetParent (parent);
		transform.localPosition = Vector3.zero;
		if(hands == HandsType.LEFT)
			transform.localRotation = Quaternion.Euler(new Vector3(-90,0,0));
		else 
			transform.localRotation = Quaternion.Euler(new Vector3(-90,0,0));
	}
	[X]
	public void RemoveFormParent(ModelCtr parent)
	{
		Destroy (gameObject);
	}

	public HandsType hands = HandsType.LEFT;
	public WeaponType weaponType = WeaponType.MELEE;
}


