using UnityEngine;
using UnityEngine.Networking;

public class ShieldEffect: ItemEffect
{
	void Start()
	{
		this.effectType = EffectType.INVINCIBLE;
		Vector3 scale = new Vector3 (
			                this.instantiatedPrefab.transform.localScale.x * this.player.transform.localScale.x,
			                this.instantiatedPrefab.transform.localScale.y * this.player.transform.localScale.y,
			                this.instantiatedPrefab.transform.localScale.z * this.player.transform.localScale.z
		                );
		this.instantiatedPrefab.transform.localScale = scale;
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
					Vector3 velocity = projectile.GetComponent<Rigidbody> ().velocity;
					//					Destroy (projectile);
				}
			}
		} else {
			Health health = other.GetComponent<Health> ();
			if (health != null) {
				Debug.Log ("DAMAGE SHIELD");
				health.TakeDamage (10);
			}
//			Rigidbody body = other.gameObject.GetComponent<Rigidbody> ();
//			if (body == null) {
//				// Some objects (boxes) handle collision on the children, so get teh rigid body on the parent
//				body = other.gameObject.transform.parent.GetComponent<Rigidbody> ();
//			}
//			if (body != null)
//				body.AddForce ((other.transform.position - transform.position) * 2, ForceMode.VelocityChange);
		}
	}

	override protected void OnRemove(){
	}

	override protected void OnApply () {
	}
}

