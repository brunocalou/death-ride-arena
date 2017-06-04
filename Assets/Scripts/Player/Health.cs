﻿using UnityEngine;
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

		ItemBehaviour[] behaviours = GetComponentsInChildren<ItemBehaviour> ();
		foreach (ItemBehaviour behaviour in behaviours) {
			Debug.Log (behaviour);
			if (behaviour.behaviourType == BehaviourType.INVINCIBLE) {
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

		ItemBehaviour[] behaviours = GetComponentsInChildren<ItemBehaviour> ();
		foreach (ItemBehaviour behaviour in behaviours) {
			Destroy (behaviour.gameObject);
		}

		if (isLocalPlayer)
		{

			// Set the spawn point to origin as a default value
			Vector3 spawnPoint = Vector3.zero;

			// If there is a spawn point array and the array is not empty, pick one at random
			if (spawnPoints != null && spawnPoints.Length > 0)
			{
				spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
				Debug.Log (spawnPoint);
			}

			// Set the player’s position to the chosen spawn point
//			transform.position = spawnPoint;
			Rigidbody rigidBody = transform.GetComponent<Rigidbody> ();
			rigidBody.position = spawnPoint;
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