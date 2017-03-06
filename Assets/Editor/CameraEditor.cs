using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class CameraEditor : XBaseWindow {

	[MenuItem("Tools/Camera Tools")]
	static void Init()
	{
		InitWindow<CameraEditor> ();
	}
	ArrayList s_SceneViews = null;
	Shader aura = null;
	void OnEnable()
	{
		var fields = typeof(SceneView).GetFields ();
		var field = typeof(SceneView).GetField ("s_SceneViews", BindingFlags.NonPublic | BindingFlags.Static);
		s_SceneViews = field.GetValue (null) as ArrayList;

		aura = EditorGUIUtility.LoadRequired ("SceneView/SceneViewAura.shader") as Shader;
	}
	public override void OnXGUI ()
	{
		base.OnXGUI ();
		DoButton ("OnXGUI", ()=> { Selection.objects = new Object[]{Camera.main};});
		var allCamera = Camera.allCameras;

		for (int i = 0; i < allCamera.Length; i++) {
			CreateObjectField ("All Camera " ,allCamera [i]);
		}
		CreateObjectField ("current",Camera.current);
		CreateObjectField ("main", Camera.main);

		for (int i = 0; i < s_SceneViews.Count; i++) {
			
			var scenView = s_SceneViews [i] as SceneView;
			CreateObjectField ("SceneView " , scenView);
			CreateObjectField ("SceneView " ,scenView.camera);
			scenView.camera.gameObject.hideFlags = (HideFlags)CreateEnumPopup("Flag", scenView.camera.gameObject.hideFlags);

		}

		CreateObjectField (aura);
		DoButton ("aura path", () => {
			var str = AssetDatabase.GetAssetPath(aura);
			XLogger.Log(str);
		});

	}
}
