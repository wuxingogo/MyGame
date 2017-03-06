using UnityEngine;
using System.Collections;
using wuxingogo.Editor;
using UnityEditor;

[CustomEditor(typeof(MeshFilter))]
public class XMeshEditor : XMonoBehaviourEditor {

	MeshFilter filter = null;
	SerializedProperty uv = null;
	SerializedProperty uv2 = null;
	SerializedProperty uv3 = null;
	SerializedProperty uv4 = null;
	public float aaaa;

	void OnEnable()
	{
		filter = target as MeshFilter;

	}
	void OnDisable()
	{
		filter = null;
	}
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

	}
	bool vertexToggle = false;
	bool uvToggle = false;
	bool uv2Toggle = false;
	bool uv3Toggle = false;
	bool uv4Toggle = false;
	public override void OnXGUI ()
	{
		Mesh currentMesh = filter.sharedMesh;

		if (currentMesh == null)
			return;
		vertexToggle = CreateCheckBox ("vertexToggle", vertexToggle);
		if (vertexToggle) {
			for (int i = 0; i < currentMesh.vertexCount; i++) {
				CreateVector3Field ("Mesh_Vertex_" + i, currentMesh.vertices [i]);
			}
		}
		uvToggle = CreateCheckBox ("uv", uvToggle);
		if (uvToggle) {
			for (int i = 0; i < currentMesh.uv.Length; i++) {
				CreateVector2Field ("Mesh_uv0_" + i, currentMesh.uv [i]);
			}
		}
		uv2Toggle = CreateCheckBox ("uv2",uv2Toggle);
		if (uv2Toggle) {
			
			for (int i = 0; i < currentMesh.uv2.Length; i++) {
				CreateVector2Field ("Mesh_uv1_" + i, currentMesh.uv2 [i]);
			}
		}

		uv3Toggle = CreateCheckBox ("uv3",uv3Toggle);
		if (uv3Toggle) {
			for (int i = 0; i < currentMesh.uv3.Length; i++) {
				CreateVector2Field ("Mesh_uv2_" + i, currentMesh.uv3 [i]);
			}
		}
		uv4Toggle = CreateCheckBox ("uv4",uv4Toggle);
		if (uv4Toggle) {
			for (int i = 0; i < currentMesh.uv4.Length; i++) {
				CreateVector2Field ("Mesh_uv3_" + i, currentMesh.uv4 [i]);
			}
		}


	}
}
