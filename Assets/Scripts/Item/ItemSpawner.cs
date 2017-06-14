using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawner : NetworkBehaviour {

	public GameObject[] itemPrefabs;
	private GameObject[] spawnLocations;

	private float lastSpawnTime = 0;
	private float spawnTime = 45; // Spawn an item on every 45 seconds
	private int spawnAttempts = 0;
	private int maxSpawnAttempts = 5;

	void Start()
	{
		spawnLocations = GameObject.FindGameObjectsWithTag ("ItemSpawnPoint");
	}

	void FixedUpdate()
	{
		if (Time.time - lastSpawnTime > spawnTime) {
			var spawn = spawnLocations[Random.Range(0, spawnLocations.Length)];
			var hitColliders = Physics.OverlapSphere(spawn.transform.position, 2f);
			// Check if the spawn point is beeing used
			if (hitColliders.Length == 0) {
				var item = (GameObject)Instantiate (itemPrefabs [Random.Range (0, itemPrefabs.Length)], spawn.transform.position, spawn.transform.rotation);

				lastSpawnTime = Time.time;
				NetworkServer.Spawn (item);
			} else {
				spawnAttempts += 1;

				if (spawnAttempts >= maxSpawnAttempts) {
					spawnAttempts = 0;
					lastSpawnTime = Time.time;
				}
			}
		}
	}
}