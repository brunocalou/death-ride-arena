using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawner : NetworkBehaviour {

	public GameObject itemPrefab;
//	public int numberOfEnemies;

	public override void OnStartServer()
	{
//		for (int i=0; i < numberOfEnemies; i++)
//		{
			var spawnPosition = new Vector3(
				Random.Range(-8.0f, 8.0f),
				0.0f,
				Random.Range(-8.0f, 8.0f));

			var spawnRotation = Quaternion.Euler( 
				0.0f, 
				Random.Range(0,180), 
				0.0f);

		var item = (GameObject)Instantiate(itemPrefab, spawnPosition, spawnRotation);
		NetworkServer.Spawn(item);
//		}
	}
}