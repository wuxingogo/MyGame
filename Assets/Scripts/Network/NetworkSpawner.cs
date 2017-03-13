using UnityEngine;
using UnityEngine.Networking;
using wuxingogo.Runtime;

public class NetworkSpawner : NetworkBehaviour {

	public GameObject plantPrefab;

	public void OnStartClient()
	{
		ClientScene.RegisterPrefab(plantPrefab);
	}
	[X]
	[Server]
	public void ServerSpawnPlant(Player player)
	{
		var plant = (GameObject)Instantiate(plantPrefab, transform.position, transform.rotation);
		NetworkServer.Spawn(plant);
	}

	[X]
	[Command]
	public void CmdSpawnPlant()
	{
		var plant = (GameObject)Instantiate(plantPrefab, transform.position, transform.rotation);
		NetworkServer.Spawn(plant);
	}

}