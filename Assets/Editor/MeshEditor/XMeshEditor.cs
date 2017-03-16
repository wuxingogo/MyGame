//using UnityEngine;
//using System.Collections;
//using wuxingogo.Editor;
//using UnityEditor;
//
//[CustomEditor(typeof(MeshFilter))]
//public class XMeshEditor : XMonoBehaviourEditor {
//
//	MeshFilter filter = null;
//
//	private Vector3[] vertices;
//	private Color[] colors;
//	private int[] triangles;
//
//	SerializedProperty uv = null;
//	SerializedProperty uv2 = null;
//	SerializedProperty uv3 = null;
//	SerializedProperty uv4 = null;
//	public float aaaa;
//
//	void OnEnable()
//	{
//		filter = target as MeshFilter;
//
//		vertices = filter.sharedMesh.vertices;
//		colors = filter.sharedMesh.colors;
//		triangles = filter.sharedMesh.triangles;
//
//	}
//	void OnDisable()
//	{
//		filter = null;
//	}
//	public override void OnInspectorGUI ()
//	{
//		base.OnInspectorGUI ();
//
//	}
//	bool vertexToggle = false;
//	bool uvToggle = false;
//	bool uv2Toggle = false;
//	bool uv3Toggle = false;
//	bool uv4Toggle = false;
//	public override void OnXGUI ()
//	{
//		Mesh currentMesh = filter.sharedMesh;
//
//		if (currentMesh == null)
//			return;
//		vertexToggle = CreateCheckBox ("vertexToggle", vertexToggle);
//		if (vertexToggle) {
//			for (int i = 0; i < currentMesh.vertexCount; i++) {
//				CreateVector3Field ("Mesh_Vertex_" + i, currentMesh.vertices [i]);
//			}
//		}
//		uvToggle = CreateCheckBox ("uv", uvToggle);
//		if (uvToggle) {
//			for (int i = 0; i < currentMesh.uv.Length; i++) {
//				CreateVector2Field ("Mesh_uv0_" + i, currentMesh.uv [i]);
//			}
//		}
//		uv2Toggle = CreateCheckBox ("uv2",uv2Toggle);
//		if (uv2Toggle) {
//			
//			for (int i = 0; i < currentMesh.uv2.Length; i++) {
//				CreateVector2Field ("Mesh_uv1_" + i, currentMesh.uv2 [i]);
//			}
//		}
//
//		uv3Toggle = CreateCheckBox ("uv3",uv3Toggle);
//		if (uv3Toggle) {
//			for (int i = 0; i < currentMesh.uv3.Length; i++) {
//				CreateVector2Field ("Mesh_uv2_" + i, currentMesh.uv3 [i]);
//			}
//		}
//		uv4Toggle = CreateCheckBox ("uv4",uv4Toggle);
//		if (uv4Toggle) {
//			for (int i = 0; i < currentMesh.uv4.Length; i++) {
//				CreateVector2Field ("Mesh_uv3_" + i, currentMesh.uv4 [i]);
//			}
//		}
//
//
//	}
//	private static Vector3 pointSnap = Vector3.one * 0.1f;
//	void OnSceneGUI()
//	{
//		DrawVertex ();
//	}
//	void DrawVertex()
//	{
//		Mesh currentMesh = filter.sharedMesh;
//		var m = Handles.matrix;
//		Handles.color = Color.red;
//		Handles.matrix = filter.transform.localToWorldMatrix;
//		for(int i = 0; i < currentMesh.vertexCount; i++){
//			Vector3 oldPoint = vertices[i];
//			var newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, 0.04f, pointSnap, DotCap);
//
//			if(oldPoint != newPoint){
//				vertices [i] = newPoint;
//				UpdateVertex();
//			}
//		}
//
//
//
//
//		Handles.DrawPolyLine(currentMesh.vertices);
//		Handles.matrix = m;
//	}
//
//	public static void DotCap (int controlID, Vector3 position, Quaternion rotation, float size)
//	{
//		Handles.DotCap (controlID, position, rotation, size);
//	}
//
//
//	void UpdateVertex()
//	{
//		
//		var currentMesh = new Mesh();
//		currentMesh.name = filter.name;
//		currentMesh.vertices = vertices;
//		currentMesh.colors = colors;
//		currentMesh.triangles = triangles;
//		Undo.RecordObject (currentMesh, "UpdateVertex");
//		filter.sharedMesh = currentMesh;
//		if(AssetsUtilites.GetPrefabObject(filter.gameObject) != null)
//			AssetsUtilites.AddObjectToAsset (currentMesh, filter.gameObject);
//	}
//}
