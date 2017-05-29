using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public GameObject emitter;
	public AudioClip sound;
	private AudioSource audioSource;
	private bool mustDestroyItself = false;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.PlayOneShot (sound);
	}

	void Update () {
		if (!audioSource.isPlaying && mustDestroyItself) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log ("Bullet collision");
		Debug.Log (collision.gameObject.tag);
		Debug.Log (collision.gameObject);
		Debug.Log (emitter);
		if (collision.gameObject != this.emitter && collision.gameObject.tag != "Barrier") {
			Debug.Log ("Deleting it");
			var hit = collision.gameObject;
			var health = hit.GetComponent<Health>();
			if (health != null)
			{
				health.TakeDamage(10);
			}
			mustDestroyItself = true;
			GetComponent<MeshRenderer> ().enabled = false;
			GetComponent<Collider> ().enabled = false;

		} else {
			Physics.IgnoreCollision (collision.collider, GetComponent<Collider> ());
		}
	}
}