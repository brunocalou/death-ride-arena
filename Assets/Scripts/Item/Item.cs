using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Item: NetworkBehaviour
{
	public GameObject prefab;

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
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (!isServer)
			return;
		Debug.Log ("Player collision");
		if (collision.gameObject.GetComponent<InstantKill> () != null) {
			RpcDestroyItem (gameObject.GetComponent<NetworkIdentity>().netId);
		}
	}

	[ClientRpc]
	void RpcDestroyItem (NetworkInstanceId id) {
		if (!isServer)
			return;
		GameObject item = ClientScene.FindLocalObject (id);
		if (item != null) {
			Debug.Log ("Destroyed item");
			Destroy (item);
		}
	}
}
