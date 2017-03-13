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

	public List<GameObject> allGameObject = new List<GameObject>();

	void Start()
	{
		if(isServer && isLocalPlayer)
			CmdSpawnBoss (new Vector3 (-338.6f, 3, -623.6f));
		if (isLocalPlayer) {
			Instance = this;

			CmdSpawnPlayer(transform.position);
		}


		//ServerSpawnPlayer(transform.position);


	}

	#if UNITY_NETWORK

	[X]
	[Command]
	public void CmdSpawnPlayer(Vector3 point){
		var prefab = (GameObject)Instantiate(playerPrefab, point, transform.rotation);
		NetworkServer.Spawn(prefab);

		var player = prefab.GetComponent<Player> ();
		player.isMine = isLocalPlayer;
	}

	[X]
	[Command]
	public void CmdSpawnBoss(Vector3 point){
		var prefab = (GameObject)Instantiate(bossPrefab, point, transform.rotation);
		NetworkServer.Spawn(prefab);

		var boss = prefab.GetComponent<Boss> ();
		boss.isMine = true;
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
		var go = NetworkServer.FindLocalObject (new NetworkInstanceId(controlID));
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

	#if UNITY_NETWORK
	[Command]
	public void CmdMoveTo(uint controlID, Vector3 point)
	{

		if (isServer) 
			RpcMoveTo (controlID, point);

	}
	[ClientRpc]
	public void RpcMoveTo(uint controlID, Vector3 point){
		var go = NetworkServer.FindLocalObject (new NetworkInstanceId(controlID));
		var human = go.GetComponent<Human> ();
		human.MoveTo (point);
	}
	#else
	public void CmdMoveTo(Vector3 point)
	{
	if (!isDead && !isHangs) {
	facade.MoveTo (point, false);
	goalPoint = point;
	}
	}
	#endif
//	#if UNITY_NETWORK
//	[X]
//	[Command]
//	public void CmdSynchronizePoint(Vector3 point)
//	{
//
//		if (isServer) 
//			RpcSynchronizePoint (point);
//
//	}
//	[X]
//	[ClientRpc]
//	public void RpcSynchronizePoint(Vector3 point){
//		transform.position = point;
//	}
//	#else
//	public void CmdSynchronizePoint(Vector3 point)
//	{
//	transform.position = point;
//	}
//	#endif

	/*
	#if UNITY_NETWORK
	[Command]
	public void CmdMoveTo(Vector3 point)
	{

		if (isServer) 
			RpcMoveTo (point);

	}
	[ClientRpc]
	public void RpcMoveTo(Vector3 point){
		if (!isDead && !isHangs) {
			facade.MoveTo (point, false);
			goalPoint = point;
		}
	}
	#else
	public void CmdMoveTo(Vector3 point)
	{
	if (!isDead && !isHangs) {
	facade.MoveTo (point, false);
	goalPoint = point;
	}
	}
	#endif
	*/

	void OnDestroy ()
	{
		if (Instance == this)
			Instance = null;

		for (int i = 0; i < allGameObject.Count; i++) {
			NetworkServer.Destroy (allGameObject [i]);
		}
	}
}
