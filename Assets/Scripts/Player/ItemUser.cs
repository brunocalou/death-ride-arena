using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ItemUser : NetworkBehaviour {
//	ArrayList itemBehaviours = new ArrayList();

//	void addItemBehaviour (ItemBehaviour item) {
//		itemBehaviours.Add(item);
//	}

	void OnTriggerEnter(Collider collider)
	{
		if (!isServer)
			return;
		Debug.Log("TRIGGERED");
		var gameObj = collider.gameObject;
		var item = gameObj.GetComponent<Item>();
		if (item != null) {
			RpcUseItem (GetComponent<NetworkIdentity> ().netId, item.GetComponent<NetworkIdentity>().netId);	
		}
	}

	[ClientRpc]
	void RpcUseItem (NetworkInstanceId playerId, NetworkInstanceId itemId) {
		Debug.Log ("Get the item");
		GameObject itemGameObject = ClientScene.FindLocalObject (itemId);
//		GameObject playerGameObject = ClientScene.FindLocalObject (playerId);

		if (itemGameObject != null) {
			Item item = itemGameObject.GetComponent<Item> ();
			ItemEffect effect = item.getEffect ();
			if (effect != null) {
				effect.apply (playerId);
				Destroy(item.gameObject);
			}
//			ItemBehaviour behaviour = GetComponent<ItemBehaviour> ();
//			if (behaviour != null)
//				addItemBehaviour (behaviour);
		}
	}
}