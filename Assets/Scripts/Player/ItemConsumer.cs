using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ItemConsumer : NetworkBehaviour {
	void OnCollisionEnter(Collision collision)
	{
		if (!isServer)
			return;
		Debug.Log("TRIGGERED");
		var gameObj = collision.gameObject;
		var item = gameObj.GetComponent<Item>();
		if (item != null) {
			RpcUseItem (GetComponent<NetworkIdentity> ().netId, item.GetComponent<NetworkIdentity>().netId);	
		}
	}

	[ClientRpc]
	void RpcUseItem (NetworkInstanceId playerId, NetworkInstanceId itemId) {
		Debug.Log ("Get the item");
		GameObject itemGameObject = ClientScene.FindLocalObject (itemId);

		if (itemGameObject != null) {
			Item item = itemGameObject.GetComponent<Item> ();
			if (item != null) {
				item.apply (playerId);
				Destroy(item.gameObject);
			}
		}
	}
}