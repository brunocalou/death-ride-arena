using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawner : NetworkBehaviour {

	public GameObject[] itemPrefabs;
	private GameObject[] spawnLocations;

	private float lastSpawnTime = 0;
	private float spawnTime = 15; // Spawn an item on every 15 seconds

	void Start()
	{
		spawnLocations = GameObject.FindGameObjectsWithTag ("ItemSpawnPoint");
	}

	void FixedUpdate()
	{
		if (Time.time - lastSpawnTime > spawnTime) {
			var spawn = spawnLocations[Random.Range(0, spawnLocations.Length)];
			var item = (GameObject) Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)], spawn.transform.position, spawn.transform.rotation);

			lastSpawnTime = Time.time;
			NetworkServer.Spawn(item);
		}
	}
}