using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {

	public const int maxHealth = 100;
	public bool destroyOnDeath;
	public AudioClip[] deathAudios;
	private AudioSource audioSource;

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;

	public RectTransform healthBar;

	private NetworkStartPosition[] spawnPoints;

	void Start ()
	{
		if (isLocalPlayer)
		{
			spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		}
		audioSource = GetComponent<AudioSource> ();
	}

	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;

		ItemEffect[] effects = GetComponentsInChildren<ItemEffect> ();
		foreach (ItemEffect effect in effects) {
			Debug.Log (effect);
			if ((effect.getEffectType() & EffectType.INVINCIBLE) != 0) {
				Debug.Log ("will NOT take damage");
				return;
			}
		}

		Debug.Log ("WILL TAKE DAMAGE");
		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			if (destroyOnDeath)
			{
				Destroy(gameObject);
			} 
			else
			{
				// called on the Server, invoked on the Clients
				RpcKill();
			}
		}
	}

	void OnChangeHealth (int currentHealth )
	{
		healthBar.sizeDelta = new Vector2(currentHealth , healthBar.sizeDelta.y);
	}

	[ClientRpc]
	public void RpcKill () {
		if (!isServer)
			return;
		RpcRespawn(Random.Range (0, deathAudios.Length));
	}

	[ClientRpc]
	void RpcRespawn(int deathAudioIdx)
	{
		audioSource.PlayOneShot (deathAudios [deathAudioIdx]);
		currentHealth = maxHealth;

		ItemEffect[] effects = GetComponentsInChildren<ItemEffect> ();
		foreach (ItemEffect effect in effects) {
			effect.remove ();
			Destroy (effect.gameObject);
		}

		if (isLocalPlayer)
		{

			// Set the spawn point to origin as a default value
			Vector3 spawnPoint = Vector3.zero;
			Quaternion spawnRotation = new Quaternion();

			// If there is a spawn point array and the array is not empty, pick one at random
			if (spawnPoints != null && spawnPoints.Length > 0)
			{
				NetworkStartPosition spawn = spawnPoints [Random.Range (0, spawnPoints.Length)];
				spawnPoint = spawn.transform.position;
				spawnRotation = spawn.transform.rotation;
				Debug.Log (spawnPoint);
			}

			// Set the player’s position to the chosen spawn point
			Rigidbody rigidBody = transform.GetComponent<Rigidbody> ();
			rigidBody.position = spawnPoint;
			rigidBody.rotation = spawnRotation;
			rigidBody.velocity = new Vector3 ();
			rigidBody.angularVelocity = new Vector3 ();

			Debug.Log (transform.position);
		}
	}

	public int getMaxHealth()
	{
		return maxHealth;
	}
}