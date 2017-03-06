using UnityEngine;
using System.Collections;
using UnityEditor;



public class MulitAddBoxCollider : XBaseEditor {

	[MenuItem("Tools/Add Box Collider for selected GameObjects")]
	static void Excute()
	{
		
		var goes = Selection.gameObjects;
		Undo.RecordObjects (goes, "XAddBoxCollider");
		foreach (var go in goes) {
			Selection.activeGameObject = go;
			AddBoxCollider (go);
		}
	}
	static void AddBoxCollider(GameObject target)
	{
		if (target.GetComponent<MeshRenderer> () != null && target.GetComponent<BoxCollider> () == null) {
			EditorApplication.ExecuteMenuItem ("Component/Physics/Box Collider");
			EditorUtility.SetDirty (target);
		}
	}

	static void DeleteBoxCollider(GameObject target)
	{
		var boxCollider = target.GetComponent<BoxCollider> ();
		if ( boxCollider != null) {
			DestroyImmediate (boxCollider, true);
			EditorUtility.SetDirty (target);
		}
	}
	[MenuItem("Tools/Add Box Collider for children gameobjects")]
	static void ExcuteChildrenAddBoxCollider()
	{

		var goes = Selection.gameObjects;
		Undo.RecordObjects (goes, "XAddBoxColliderForChildren");
		foreach (var go in goes) {
			Selection.activeGameObject = go;
			AddBoxCollider (go);

			GameObjectUtil.ChildrenAction (go.transform, (t) => {
				Selection.activeGameObject = t;
				AddBoxCollider (t);
			});

		}
	}

	[MenuItem("Tools/Delete Box Collider for children gameobjects")]
	static void ExcuteChildrenDeleteCollider()
	{

		var goes = Selection.gameObjects;
		Undo.RecordObjects (goes, "XAddBoxColliderForChildren");
		foreach (var go in goes) {
			Selection.activeGameObject = go;
			DeleteBoxCollider (go);

			GameObjectUtil.ChildrenAction (go.transform, (t) => {
				Selection.activeGameObject = t;
				DeleteBoxCollider (t);
			});

		}
	}
}
