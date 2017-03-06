using UnityEngine;
using System.Collections;

public class CollisionMessager : MonoBehaviour {

	public int enterGoCount = 0;
	public void SetColor(Color color)
	{
		XLogger.Log (color.ToString ());
		GameObjectUtil.ChildrenAction (transform, (t) => {
			var meshRenderer = t.GetComponent<MeshRenderer>();
			if(meshRenderer != null)
			{
				meshRenderer.material.color = color;
			}
		});
	}
	void OnTriggerEnter (Collider other)
	{
		var messager = GetMessager (other.GetComponent<CollisionMessager>(), this);
		if (messager == null)
			return;
		if (messager.enterGoCount == 0) {
			messager.SetColor (Color.red);
		}
		messager.enterGoCount++;
//		if (other.gameObject.layer == LayerManager.instance.constructionLayer) {
//			messager.enterGoCount++;
//		}
	}

	void OnTriggerExit(Collider other)
	{
		var messager = GetMessager (other.GetComponent<CollisionMessager>(), this);
		if (messager == null)
			return;
		messager.enterGoCount--;
		if (messager.enterGoCount == 0) {
			messager.SetColor (Color.green);
		}
	}

	CollisionMessager GetMessager(CollisionMessager lhs, CollisionMessager rhs)
	{
		if (lhs == null || rhs == null) {
			XLogger.LogError ("lhs or rhs was null");
			return rhs;
		}
		if (lhs.GetComponent<Rigidbody> () != null) {
			return lhs;
		}
		if (rhs.GetComponent<Rigidbody> () != null) {
			return rhs;
		}
		return null;
	}

	public void BuildingFinish()
	{
		
	}
}
