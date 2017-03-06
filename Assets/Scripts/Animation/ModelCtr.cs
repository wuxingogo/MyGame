using System;
using wuxingogo.Runtime;

public class ModelCtr : XMonoBehaviour
{
	public WeaponBehaivour mainWeapon = null;

	public AnimationCtr animCtr = null;
	
	[X]
	public void BindWeapon(WeaponBehaivour newWeapon)
	{
		if (mainWeapon != null) {
			mainWeapon.RemoveFormParent (this);
			mainWeapon = null;
		}
		
		if (newWeapon != null) {
			if (!newWeapon.isInScene) {
				newWeapon = Instantiate (newWeapon);
			}
			mainWeapon = newWeapon;
			var parent = animCtr.GetSkeleton (mainWeapon.hands);
			newWeapon.BindModel (parent);
			animCtr.weaponType = newWeapon.weaponType;
			animCtr.Idle ();
		}
	}

	
}


