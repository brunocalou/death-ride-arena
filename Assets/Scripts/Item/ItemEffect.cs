using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public abstract class ItemEffect: NetworkBehaviour
{
	[SyncVar]
	public NetworkInstanceId playerId;
	public GameObject player;
	public GameObject instantiatedPrefab;
	public EffectType effectType;

	public void remove ()
	{
		if (this.instantiatedPrefab != null) {
			Destroy (this.instantiatedPrefab);
		}
	}
}
