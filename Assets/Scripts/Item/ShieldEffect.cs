using UnityEngine;
using UnityEngine.Networking;

public class ShieldEffect: ItemEffect
{
	public override void apply (GameObject gameObject)
	{
		var shield = (GameObject)Instantiate (prefab, gameObject.transform.position, gameObject.transform.rotation);
		shield.transform.parent = gameObject.transform;
	}
}

