using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ItemConsumer : NetworkBehaviour {
	void OnCollisionEnter(Collision collision)
	{
		if (!isServer)
			return;
		var gameObj = collision.gameObject;
		var item = gameObj.GetComponent<Item>();
		if (item != null) {
			RpcUseItem (GetComponent<NetworkIdentity> ().netId, item.GetComponent<NetworkIdentity> ().netId);
		}
	}

	[ClientRpc]
	void RpcUseItem (NetworkInstanceId playerId, NetworkInstanceId itemId) {
		GameObject itemGameObject = ClientScene.FindLocalObject (itemId);

		if (itemGameObject != null) {
			Item item = itemGameObject.GetComponent<Item> ();
			if (item != null) {
				GameObject player = ClientScene.FindLocalObject (playerId);
				ItemEffect[] effects = player.GetComponentsInChildren<ItemEffect> ();
				ItemEffect itemEffect = item.prefab.GetComponent<ItemEffect> ();

				foreach (var effect in effects) {
					if (effect.GetType ().Equals (itemEffect.GetType())) {
						effect.remove ();
					}
				}

				item.apply (playerId);
				Destroy(itemGameObject);
			}
		}
	}
}