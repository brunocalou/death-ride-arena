using UnityEngine;
using UnityEngine.Networking;

public class ShieldEffect: ItemEffect
{
	void Start()
	{
		this.effectType = EffectType.INVINCIBLE;
	}

	void OnTriggerEnter (Collider other)
	{
		Debug.Log ("Trigger");
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
					Vector3 velocity = projectile.GetComponent<Rigidbody> ().velocity;
					//					Destroy (projectile);
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

	override protected void OnRemove(){
	}
}

