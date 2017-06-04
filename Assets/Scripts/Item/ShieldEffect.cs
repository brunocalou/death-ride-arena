using UnityEngine;
using UnityEngine.Networking;

public class ShieldEffect: ItemEffect
{
	public override void apply (NetworkInstanceId playerId)
	{
//		if (!isServer)
//			return;

		GameObject playerGameObject = ClientScene.FindLocalObject (playerId);
		var shield = (GameObject)Instantiate (prefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
		shield.transform.parent = playerGameObject.transform;
		shield.GetComponent<ShieldBehaviour> ().playerId = playerId;
//		NetworkServer.Spawn (shield);
	}
}

