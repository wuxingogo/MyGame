using UnityEngine;
//using UnityEditor;
using System.Collections;
#if UNITY_IPHONE
using UnityEngine.iOS;
#endif
using System.Collections.Generic;
public class GameObjectUtil {
	public static Transform FindByName(Transform root,string name)
	{
		for( int i = 0; i < root.childCount; ++i )
		{
			Transform t = root.GetChild(i);
			if( t.gameObject.name == name)
				return t;
		}
		for( int i = 0; i < root.childCount; ++i )
		{
			Transform t = root.GetChild(i);
			Transform result = FindByName(t,name);
			if( result != null )
				return result;
		}
		return null;
	}

	public static void FindByNameAll(Transform root,string name,ref List<Transform> list)
	{
		for( int i = 0; i < root.childCount; ++i )
		{
			Transform t = root.GetChild(i);
			if( t.gameObject.name == name)
				list.Add(t);
		}
		for( int i = 0; i < root.childCount; ++i )
		{
			Transform t = root.GetChild(i);
			FindByNameAll(t,name,ref list);
		}
	}

	//	[MenuItem ("GameObjectUtil/Add Empty GameObject")]
	//	public static void AddGameObject()
	//	{
	//		if(Selection.activeGameObject == null )
	//		{
	//			GameObject obj = new GameObject("GameObject");
	//			obj.transform.position = Vector3.zero;
	//			obj.transform.rotation = Quaternion.identity;
	//			obj.transform.localScale = Vector3.one;
	//		}
	//		else
	//		{
	//			GameObject obj = new GameObject("GameObject");
	//			obj.transform.parent = Selection.activeGameObject.transform;
	//			obj.transform.localPosition = Vector3.zero;
	//			obj.transform.localRotation = Quaternion.identity;
	//			obj.transform.localScale = Vector3.one;
	//		}
	//	}
	public static GameObject CreatePrefabByScreen(Transform parent,string prefabName)
	{
		if( !GameObjectUtil.IsWideScreen )
		{
			return CreatePrefab(parent,prefabName+"_43");
		}
		else
		{
			return CreatePrefab(parent,prefabName);
		}
	}
	public static GameObject CreatePrefab(Transform parent,GameObject prefab)
	{
		if( prefab == null )
			return null;
		GameObject go = (GameObject)GameObject.Instantiate(prefab);
		go.transform.parent = parent;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
		if (parent != null) {
			go.layer = parent.gameObject.layer;
			go.tag = parent.tag;
		}
		return go;
	}
	public static GameObject CreatePrefab(Vector3 pos,Vector3 eulrAngles,string prefabName)
	{
		GameObject result = CreatePrefab(null,prefabName);
		if( result == null )
			return result;
		result.transform.position = pos;
		result.transform.eulerAngles = eulrAngles;
		result.transform.localScale = Vector3.one;
		return result;
	}
	public static bool IsWideScreen
	{
		get
		{
			float screenRito = (float)Screen.width / (float)Screen.height;
			if( screenRito <= 1.4 )
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}

	public static bool isPoorDevice()
	{
		#if UNITY_IPHONE
		switch(UnityEngine.iOS.Device.generation)
		{
		case UnityEngine.iOS.DeviceGeneration.iPad1Gen:
		case UnityEngine.iOS.DeviceGeneration.iPad2Gen:
		case UnityEngine.iOS.DeviceGeneration.iPad3Gen:
		case UnityEngine.iOS.DeviceGeneration.iPadMini1Gen:
		case UnityEngine.iOS.DeviceGeneration.iPadMini2Gen:
		case UnityEngine.iOS.DeviceGeneration.iPhone4:
		case UnityEngine.iOS.DeviceGeneration.iPhone4S:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch1Gen: 
		case UnityEngine.iOS.DeviceGeneration.iPodTouch2Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch3Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch4Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch5Gen:
		return true;
		}
		#endif
		return false;

	}

	public static T CreatePrefab<T>(Transform parent,string prefabName) where T : Component
	{
		var go = CreatePrefab (parent, prefabName);

		return go.GetComponent<T>();
	}

	public static GameObject CreatePrefab(Transform parent,string prefabName)
	{
		Object o = Resources.Load<GameObject>(prefabName);
		if( o == null )
			return null;
		GameObject go = (GameObject)GameObject.Instantiate(o);
		go.transform.parent = parent;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
		if( parent !=null)
			go.layer = parent.gameObject.layer;
		return go;
	}

	public static void AlignTransform(Transform lhs, Transform rhs)
	{
		lhs.position = rhs.position;
		lhs.rotation = rhs.rotation;
	}
	public static void DestoryAllChildren(Transform root)
	{
		for( int i = 0; i < root.childCount; ++i )
		{
			Transform t = root.GetChild(i);
			GameObject.Destroy(t.gameObject);
		}
	}
	public static void DestoryChild(Transform root,string childName)
	{
		Transform childGo = GameObjectUtil.FindByName(root,childName);
		if( childGo != null)
			GameObject.Destroy(childGo.gameObject);
	}
	public static void ChildrenAction(Transform root,System.Action<GameObject> action,bool isRecursion = true)
	{
		for( int i = 0; i < root.childCount; ++i )
		{
			Transform t = root.GetChild(i);
			if( isRecursion ) ChildrenAction(t,action);
			action(t.gameObject);
		}
	}

	public static string getFullPathName(GameObject obj,string oldStr)
	{
		GameObject o = obj;

		while(o.transform.parent != null )
		{
			oldStr = "/" + o.name + oldStr;
			o = o.transform.parent.gameObject;
		}
		oldStr = "/" + o.name + oldStr;
		return oldStr;
	}

	public static string getRelativePath( GameObject obj, GameObject child)
	{
		Transform o = child.transform;
		string relitivePath = child.name;
		while( o.parent != null )
		{
			if( o.parent == obj.transform )
			{
				return relitivePath;
			}
			else
			{
				relitivePath =  o.parent.name + "/" + relitivePath;
				o = o.transform.parent;

			}
		}
		return "";
	}
	public static string TransTimeSecondIntToString (long second)
	{
		string str = "";
		try {
			long hour = second / 3600;
			long min = second % 3600 / 60;
			long sec = second % 60;
			if (hour > 0) {
				if (hour < 10) {
					str += "0" + hour.ToString ();
				} else {
					str += hour.ToString ();
				}
				str += ":";
			}
			if (min < 10) {
				str += "0" + min.ToString ();
			} else {
				str += min.ToString ();
			}
			str += ":";
			if (sec < 10) {
				str += "0" + sec.ToString ();
			} else {
				str += sec.ToString ();
			}
		} catch (System.Exception ex) {
			//XLogger.Log("Catch:" + ex.Message);
		}
		return str;
	}

	public static System.DateTime ConvertTime (long time)
	{

		System.DateTime dateTimeStart = System.TimeZone.CurrentTimeZone.ToLocalTime (new System.DateTime (1970, 1, 1));
		long lTime = long.Parse (time + "0000");
		System.TimeSpan toNow = new System.TimeSpan (lTime); 
		return dateTimeStart.Add (toNow);

		//		System.DateTime timeStamp = new System.DateTime (1970, 1, 1); //得到1970年的时间戳
		//		long t = (time + 8 * 60 * 60 * 1000) * 10000 + timeStamp.Ticks;
		//		System.DateTime dt = new System.DateTime (t);
		//		return dt;
	}
	public static System.TimeSpan SpaceOfTime(long time){
		return System.DateTime.Now - ConvertTime (time);
	}
}
