using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public abstract class ItemEffect: NetworkBehaviour
{
	public GameObject prefab;
	public abstract void apply (GameObject gameObject);
}
