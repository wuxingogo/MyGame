using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using wuxingogo.Runtime;


public class NetworkListener : NetworkBehaviour {
	
	//Local player
	public static NetworkListener Instance = null;
	public GameObject playerPrefab = null;
	public GameObject bossPrefab = null;
	public GameObject sceneBossGameObject = null;

	public List<GameObject> allGameObject = new List<GameObject>();
	public static int allTeamID = 0;
	public int teamID = 0;
	void Start()
	{
		allTeamID++;
		teamID = allTeamID;

		if (isLocalPlayer) {
			Instance = this;

			CmdSpawnPlayer(this.teamID, transform.position);
		}

		ServerBornListener ();
		//ServerSpawnPlayer(transform.position);
	}
	[X]
	public bool isStart = false;
	public int bossCount = 0;
	[Server]
	public void ServerBornListener()
	{
		if(!isStart){
			if (NetworkServer.connections.Count >= 2) {
				isStart = true;
				SpwanBoss ();
			}
		}
	}

	#if UNITY_NETWORK

	[X]
	public void SpwanBoss()
	{
		ServerSpawnBoss (new Vector3 (-338.6f, 3, -623.6f));
		bossCount++;

		RpcBornBoss (bossCount);
	}
	[ClientRpc]
	void RpcBornBoss(int bossCount)
	{
		var t = Resources.Load<GameObject>("UI/3DTitle");
		t = Instantiate (t);
		var mainPanel = UIManager.Instance.transform.Find ("MainPanel");
		t.transform.SetParent (mainPanel);
		string s = string.Format ("Round {0}", bossCount);
		t.GetComponent<TMPro.TextMeshPro> ().text = s;

		Destroy (t, 10);
	}


	public void OnStartClient()
	{
		ClientScene.RegisterPrefab(playerPrefab);
		ClientScene.RegisterPrefab(bossPrefab);
	}

	[X]
	[Command]
	public void CmdSpawnPlayer(int teamID, Vector3 point){
		if (isServer)
			ServerSpawnPlayer (teamID, point);
	}

	[Server]
	public void ServerSpawnPlayer(int teamID, Vector3 point)
	{
		if (isServer) {
			var prefab = (GameObject)Instantiate (playerPrefab, point, transform.rotation);
			NetworkServer.Spawn (prefab);

			var player = prefab.GetComponent<Player> ();
			RpcResetTeamID(player.netId.Value, teamID);
		}
	}
	[ClientRpc]
	public void RpcResetTeamID(uint id, int teamID){
		var prefab = ClientScene.FindLocalObject (new NetworkInstanceId (id));
		var player = prefab.GetComponent<Player> ();
		player.teamID = teamID;
		player.isNetInit = true;
	}
	[X]
	public static NetworkInstanceId GetNetworkInstanceID(GameObject go)
	{
		return go.GetComponent<NetworkBehaviour> ().netId;
	}

	[X]
	[Server]
	public void ServerSpawnBoss(Vector3 point){
		if (isServer) {
			if (sceneBossGameObject != null) {
				NetworkServer.Destroy(sceneBossGameObject);
				sceneBossGameObject = null;
			}

			sceneBossGameObject = (GameObject)Instantiate (bossPrefab, point, transform.rotation);
			NetworkServer.Spawn (sceneBossGameObject);

			var boss = sceneBossGameObject.GetComponent<Boss> ();
			boss.isMine = false;
			allGameObject.Add (bossPrefab);
		}
	}
	
	[ClientRpc]
	public void RpcDestroyObject(uint controlID)
	{
		var go = FindLocalObject (controlID);
		Destroy (go);
	}
	[ClientRpc]
	public void RpcSpawnBoss(Vector3 point){
		
		sceneBossGameObject = (GameObject)Instantiate (bossPrefab, point, transform.rotation);
		NetworkServer.Spawn (sceneBossGameObject);

		var boss = sceneBossGameObject.GetComponent<Boss> ();
		boss.isMine = false;
		allGameObject.Add (bossPrefab);
	}

	[X]
	[Command]
	public void CmdSynchronizePoint(uint controlID, Vector3 point)
	{

		if (isServer) 
			RpcSynchronizePoint (controlID, point);

	}
	[X]
	[ClientRpc]
	public void RpcSynchronizePoint(uint controlID, Vector3 point){
//		var go = NetworkServer.FindLocalObject (new NetworkInstanceId(controlID));
//		var identity = NetworkIdentity.FindObjectsOfType<NetworkIdentity>();
//		foreach (var item in identity) {
//			if (item.netId.Value == controlID) {
//				var go = item.gameObject;
//				var human = go.GetComponent<Human> ();
//				human.transform.position = point;
//				allGameObject.Add (go);
//			}
//		}

		var go = FindLocalObject (controlID);
		var human = go.GetComponent<Human> ();
		human.transform.position = point;
		allGameObject.Add (go);
	}
	#else
	public void CmdSynchronizePoint(Vector3 point)
	{
		transform.position = point;
	}
	#endif

	[Command]
	public void CmdMoveTo(uint controlID, Vector3 point)
	{
		RpcMoveTo (controlID, point);

	}
	[ClientRpc]
	public void RpcMoveTo(uint controlID, Vector3 point){
//		var go = NetworkServer.FindLocalObject (new NetworkInstanceId(controlID));
//		var identity = NetworkIdentity.FindObjectsOfType<NetworkIdentity>();
//		foreach (var item in identity) {
//			if (item.netId.Value == controlID) {
//				var go = item.gameObject;
//				var human = go.GetComponent<Human> ();
//				human.MoveTo (point);
//			}
//		}

		var go = FindLocalObject (controlID);
		var human = go.GetComponent<Human> ();
		human.MoveTo (point);

	}
	[Command]
	public void CmdDamage(uint controlID, float energy)
	{
		if (isServer)
			RpcDamage (controlID, energy);
	}
	[ClientRpc]
	public void RpcDamage(uint controlID, float energy)
	{
		var prefab = FindLocalObject(controlID);
		var player = prefab.GetComponent<Human> ();
		player.dataSystem.Damage (energy);
	}


	[Command]
	public void CmdMove(uint controlID, Vector3 energy)
	{
//		if (isServer)
			RpcMove (controlID, energy);
	}
	[ClientRpc]
	public void RpcMove(uint controlID, Vector3 energy)
	{
		var prefab = FindLocalObject(controlID);
		var player = prefab.GetComponent<NetworkMover> ();
		player.NetMove (energy);
	}

	[Command]
	public void CmdRotation(uint controlID, Quaternion energy)
	{
		if (isServer)
			RpcRotation (controlID, energy);
	}
	[ClientRpc]
	public void RpcRotation(uint controlID, Quaternion energy)
	{
		var prefab = FindLocalObject(controlID);
		var player = prefab.GetComponent<NetworkMover> ();
		player.NetRotate(energy);
	}

	[Command]
	public void CmdCastSkillWithDirection(uint controlID, int skillIndex, Vector3 forward)
	{
		if (isServer)
			RpcCastSkillWithDirection (controlID, skillIndex, forward);
	}
	[ClientRpc]
	public void RpcCastSkillWithDirection(uint controlID, int skillIndex, Vector3 forward)
	{
		var human = FindLocalObjectComponent<Human>(controlID);
		human.OnNetworkFireSkillWithDirection (skillIndex, forward);
	}
	[Command]
	public void CmdCastSkillWithTarget(uint controlID, int skillIndex, uint targetID)
	{
		if (isServer)
			RpcCastSkillWithTarget (controlID, skillIndex, targetID);
	}
	[ClientRpc]
	public void RpcCastSkillWithTarget(uint controlID, int skillIndex, uint targetID)
	{
		var human = FindLocalObjectComponent<Human>(controlID);
		var target = FindLocalObjectComponent<Human>(targetID);
		human.OnNetworkFireSkillWithHuman (skillIndex, target);
	}
	[Command]
	public void CmdCastSkillWithPoint(uint controlID, int skillIndex, Vector3 point)
	{
		if (isServer)
			RpcCastSkillWithPoint (controlID, skillIndex, point);
	}
	[ClientRpc]
	public void RpcCastSkillWithPoint(uint controlID, int skillIndex, Vector3 point)
	{
		var human = FindLocalObjectComponent<Human>(controlID);
		human.OnNetworkFireSkillWithPoint (skillIndex, point);
	}

	public GameObject FindLocalObject(uint controlID)
	{
		return ClientScene.FindLocalObject (new NetworkInstanceId (controlID));
	}
	public T FindLocalObjectComponent<T>(uint controlID) where T : Component
	{
		return FindLocalObject(controlID).GetComponent<T>();
	}

	void OnDestroy ()
	{
		if (Instance == this)
			Instance = null;

		for (int i = 0; i < allGameObject.Count; i++) {
			NetworkServer.Destroy (allGameObject [i]);
		}
	}

	void OnGUI()
	{
		if (isLocalPlayer) {
			var dict = ClientScene.objects;
			int i = 0;
			foreach (var item in dict) {
				
				GUI.Label (new Rect (Screen.width - 300, 40 + i * 40, 200, 40), i + " ID : " + item.Key.Value.ToString() + ",Name : "+ item.Value.gameObject.name);
				i++;
			}
		}
	}
}
