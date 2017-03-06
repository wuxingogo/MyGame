using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConstructPanel : MonoBehaviour {

	public List<GameObject> constructGo = new List<GameObject>();
	public ConstructionStruct currentBuilding = null;
	public GameObject mouseGo = null;
	public Vector3 currentMousePosition = Vector3.zero;
	public ScrollRect scrollview = null;

	public Transform content = null;
	void Start()
	{
		PrototypeManager.instance.Sort ();
		for (int i = 0; i < content.childCount; i++) {
			// 把scrollView的子节点 设置类型等级为0的名字
			var type = (ConstructType)((i + 1) * 10);
			var name = PrototypeManager.instance.Construction[type] [0].gameObject.name;
			var textComponent = content.GetChild (i).GetChild (0).GetComponent<Text> ();
			textComponent.text = name;
		}
	}
	public void SelectConstruct(int type)
	{
		var t = (ConstructType)((type+1) * 10);
		currentBuilding = PrototypeManager.instance.Construction[t][0];
		ClearMouseGo ();
		mouseGo = Instantiate<GameObject>(currentBuilding.gameObject);
		mouseGo.AddComponent<Rigidbody> ();
//		ConstructBuilding ();

	}
	void ClearMouseGo()
	{
		if (mouseGo != null) {
			Destroy (mouseGo);
			mouseGo = null;
		}
	}

	void Update()
	{
		var fingerPos = Input.mousePosition;
		//fingerPos = Camera.main.ScreenToWorldPoint (fingerPos);
		var ray = Camera.main.ScreenPointToRay(fingerPos);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 10000, LayerManager.instance.goundLayer)) {
//			print (hit.point);
			Debug.DrawRay (ray.origin, ray.direction, Color.red);
			SetMouseGameObjectPos (hit.point, true);
		}

		if (Input.GetMouseButtonDown (1)) {
			ClearMouseGo ();
		}
		else if (Input.GetMouseButtonDown (0) && mouseGo != null) {
			ConstructBuilding ();
			ClearMouseGo ();
		}

	}
	void ConstructBuilding()
	{
		var messager = mouseGo.GetComponent<CollisionMessager> ();
		if (messager.enterGoCount == 0) {
			var constructing = Resources.Load<GameObject> ("Prefabs/Constructing");
			constructing = Instantiate<GameObject> (constructing);
			constructing.transform.position = currentMousePosition;
			var newBuilding = Instantiate<GameObject> (currentBuilding.gameObject);
			newBuilding.transform.SetParent (constructing.transform.Find ("Container"));
			newBuilding.transform.localPosition = Vector3.zero;
		} else {
			XLogger.Log ("Cannot build target at position");
		}
	}

	void SetMouseGameObjectPos(Vector3 pos, bool visible)
	{
		if (mouseGo == null)
			return;
		if ( mouseGo.activeSelf != visible) {
			mouseGo.gameObject.SetActive (true);

		}
		mouseGo.transform.position = pos;
		currentMousePosition = pos;
	}


}
