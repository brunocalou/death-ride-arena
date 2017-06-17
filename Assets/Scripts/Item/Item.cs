using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Item: NetworkBehaviour
{
	public GameObject prefab;
	public AudioClip[] startAudios;
	private AudioClip startAudio;

	public void Start () {
		if (isServer && startAudios.Length > 0) {
			RpcSetAudio(Random.Range (0, this.startAudios.Length));
		}
	}

	[ClientRpc]
	private void RpcSetAudio(int audioIdx)
	{
		Debug.Log ("Set audio to " + audioIdx);
		this.startAudio = this.startAudios [audioIdx];
	}

	public void apply (NetworkInstanceId playerId)
	{
		if (prefab != null) {
			var player = ClientScene.FindLocalObject (playerId);
			var instantiatedPrefab = (GameObject)Instantiate (prefab, player.transform.position, player.transform.rotation);
			instantiatedPrefab.transform.parent = player.transform;

			ItemEffect effect = instantiatedPrefab.GetComponent<ItemEffect> ();
			if (effect != null) {
				effect.playerId = playerId;
				effect.player = player;
				effect.instantiatedPrefab = instantiatedPrefab;
				effect.apply ();
			}

			if (startAudio != null) {
				AudioSource audioSource = player.GetComponent<AudioSource> ();
				if (audioSource != null) {
					audioSource.enabled = true;
					audioSource.PlayOneShot (startAudio);
				}
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<PlayerMovement> () != null) {
			Physics.IgnoreCollision (this.gameObject.GetComponent<Collider> (), collision.collider);
			var renderer = this.gameObject.GetComponent<Renderer> ();

			if (renderer != null) {
				renderer.enabled = false;
			} else {
				foreach (var rend in this.gameObject.GetComponentsInChildren<Renderer>()) {
					rend.enabled = false;
				}
			}

			var collider = this.gameObject.GetComponent<Collider> ();
			if (collider) {
				collider.enabled = false;
			} else {
				foreach (var col in this.gameObject.GetComponentsInChildren<Collider>()) {
					col.enabled = false;
				}
			}
		}

		if (!isServer)
			return;
		if (collision.gameObject.GetComponent<InstantKill> () != null) {
			RpcDestroyItem (gameObject.GetComponent<NetworkIdentity> ().netId);
		}
	}

	[ClientRpc]
	void RpcDestroyItem (NetworkInstanceId id) {
		if (!isServer)
			return;

		GameObject item = ClientScene.FindLocalObject (id);
		if (item != null) {
			Destroy (item);
		}
	}
}
