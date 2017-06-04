using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	ItemEffect effect;

	void Start() {
		effect = (ItemEffect) GetComponent(typeof (ItemEffect));
	}

	public ItemEffect getEffect () {
		return effect;
	}

//	void OnCollisionEnter(Collision collision) {
//		Physics.IgnoreCollision (collision.collider, GetComponent<Collider> ());
//
//		var hit = collision.gameObject;
//		var health = hit.GetComponent<Health>();
//		Debug.Log (health);
//		if (health != null) {
//			if (effect != null) {
//				Debug.Log ("Apply effect");
//				effect.apply (collision.gameObject);
//				Destroy(gameObject);
//			}
//
//		}
//
//	}
}