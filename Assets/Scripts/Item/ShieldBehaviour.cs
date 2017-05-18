using UnityEngine;
using UnityEngine.Networking;

public class ShieldBehaviour: NetworkBehaviour
{
	Collider shieldCollider;

	void Start()
	{
		shieldCollider = GetComponent<Collider> ();
	}

	void OnCollisionEnter(Collision collision)
	{
		var hit = collision.gameObject;
		Debug.Log ("Collision with " + hit);
		Debug.Log (transform.parent.gameObject);

//		Physics.IgnoreCollision (collision.collider, GetComponent<Collider> ());

		if (hit == transform.parent.gameObject) {
			Debug.Log ("Hit the player");
			Physics.IgnoreCollision (collision.collider, GetComponent<Collider> ());
			return;
		}

		// Hit another player
		var health = hit.GetComponent<Health> ();

		if (health != null) {
			Debug.Log ("Get awary from me!");
			health.TakeDamage (10);
			Rigidbody body = collision.gameObject.GetComponent<Rigidbody> ();
			body.AddForce (-collision.contacts [0].normal * 50, ForceMode.VelocityChange);

		} else {
			// Hit something else
			Debug.Log ("Hit something else");
			// If it comes from the player the shield is attatched to, ignore the collision
			if (hit.transform.IsChildOf (this.transform.parent)) {
				Debug.Log ("Hit my own bullet");
				Physics.IgnoreCollision (collision.collider, shieldCollider);
			}
				
		}

		//		Destroy(gameObject);
	}
}
