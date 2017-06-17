using UnityEngine;
using UnityEngine.Networking;

public class ProteinEffect: ItemEffect
{
	public AudioClip impactAudio;
	private AudioSource audioSource;
	public float massMultiplicationFactor = 50f;
	public float speedMultiplicationFactor = 1.3f;
	public float rotationSpeedMultiplicationFactor = 1.3f;
	public float scaleMultiplicationFactor = 1.8f;

	private BoxCollider proteinCollider;

	private float originalSpeed;
	private float originalRotationSpeed;
	private float originalMass;
	private Vector3 originalScale;

	void Start()
	{
		this.effectType = EffectType.HIT_DAMAGE | EffectType.INVINCIBLE;
		this.proteinCollider = this.gameObject.GetComponent<BoxCollider> ();
		this.proteinCollider.center = this.player.GetComponent<BoxCollider> ().center;
		this.proteinCollider.size = this.player.GetComponent<BoxCollider> ().size;
		this.originalScale = this.player.transform.localScale;

		Vector3 scale = new Vector3 (originalScale.x, originalScale.y, originalScale.z);
		scale *= scaleMultiplicationFactor;
		this.player.transform.localScale = scale;

		Rigidbody playerRigidbody = this.player.GetComponent<Rigidbody> ();
		if (playerRigidbody != null) {
			this.originalMass = playerRigidbody.mass;
			playerRigidbody.mass *= massMultiplicationFactor;
		}

		PlayerMovement playerMovement = this.player.GetComponent<PlayerMovement> ();
		if (playerMovement != null) {
			this.originalSpeed = playerMovement.maxSpeed;
			this.originalRotationSpeed = playerMovement.maxRotateSpeed;
			playerMovement.maxSpeed *= speedMultiplicationFactor;
			playerMovement.maxRotateSpeed *= rotationSpeedMultiplicationFactor;
		}

		this.audioSource = this.player.GetComponent<AudioSource> ();
	}

	override protected void OnApply () {
	}
		


	override protected void OnRemove ()
	{
		this.player.transform.localScale = originalScale;
		this.player.GetComponent<Rigidbody> ().mass = originalMass;
		PlayerMovement playerMovement = this.player.GetComponent<PlayerMovement> ();
		playerMovement.maxSpeed = originalSpeed;
		playerMovement.maxRotateSpeed = originalRotationSpeed;
	}

	void OnTriggerEnter (Collider other)
	{
		if (audioSource != null) {
			if (other.GetComponent<PlayerMovement> () != null) {
				audioSource.PlayOneShot (impactAudio);

				Health health = other.GetComponent<Health> ();
				if (health != null) {
					health.TakeDamage (5);
				}
			}
		}
	}
}



