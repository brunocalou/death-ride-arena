using UnityEngine;
using UnityEngine.Networking;

public class ShieldBehaviour: NetworkBehaviour
{
	Collider shieldCollider;
	GameObject player;

	void Start()
	{
		shieldCollider = GetComponent<Collider> ();
		player = transform.parent.gameObject;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Projectile") {
			Debug.Log ("Projectile");
			var projectile = other.gameObject.GetComponent<Bullet> ();
			if (projectile != null) {
				Debug.Log ("is not null");
				if (projectile.emitter == player) {
					Debug.Log ("My own bullet");
					Physics.IgnoreCollision (other, GetComponent<Collider> ());
				} else {
					Debug.Log ("The other guy's bullet");
					Destroy (projectile);
				}
			}
		} else {
//			var otherPlayer = other.gameObject;
//			var health = otherPlayer.GetComponent<Health> ();
//			if (health != null) {
//				// Hit another player
//				Rigidbody body = otherPlayer.gameObject.GetComponent<Rigidbody> ();
//				body.AddForce ((otherPlayer.transform.position - transform.position) * 30, ForceMode.VelocityChange);
//			}
		}
	}

//	void OnCollisionEnter(Collision collision)
//	{
//		handleCollision (collision);
//	}

//	void handleCollision(Collision collision)
//	{
//		var hit = collision.gameObject;
//		Debug.Log ("Collision with " + hit);
//		Debug.Log (transform.parent.gameObject);
//		Physics.IgnoreCollision (collision.collider, GetComponent<Collider> ());
////		//		Physics.IgnoreCollision (collision.collider, GetComponent<Collider> ());
////
//		if (hit == transform.parent.gameObject) {
//			Debug.Log ("Hit the player");
////			Physics.IgnoreCollision (collision.collider, GetComponent<Collider> ());
//			return;
//		}
////
////		// Hit another player
//		var health = hit.GetComponent<Health> ();
//
//		if (health != null) {
//			Physics.IgnoreCollision (collision.collider, GetComponent<Collider> (), false);
//			Debug.Log ("Get awary from me!");
//			health.TakeDamage (10);
//			Rigidbody body = collision.gameObject.GetComponent<Rigidbody> ();
//			body.AddForce (-collision.contacts [0].normal * 50, ForceMode.VelocityChange);
////
//		} // else {
////			// Hit something else
////			Debug.Log ("Hit something else");
////			// If it comes from the player the shield is attatched to, ignore the collision
////			if (hit.transform.IsChildOf (this.transform.parent)) {
////				Debug.Log ("Hit my own bullet");
////				Physics.IgnoreCollision (collision.collider, shieldCollider);
////			}
////
////		}
//
//		//		Destroy(gameObject);
//	}
}
