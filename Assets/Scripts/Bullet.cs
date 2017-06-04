using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
	public GameObject emitter;

	[SyncVar]
	public NetworkInstanceId emitterId;
	public AudioClip sound;
	private AudioSource audioSource;
	private bool mustDestroyItself = false;

	void Start () {
		//Null Check before getting components...
		if(audioSource == null)
			audioSource = GetComponent<AudioSource> ();
		audioSource.PlayOneShot (sound);
//
//		Debug.Log ("Start bullet");
		GameObject obj = ClientScene.FindLocalObject (emitterId);
//		Debug.Log ("emitterId");
//		Debug.Log (emitterId);
//		Debug.Log ("obj");
//		Debug.Log (obj);
		if (obj != null)
			Physics.IgnoreCollision (obj.GetComponent<Collider> (), GetComponent<Collider> ());
	}

	void Update () {
		if (!audioSource.isPlaying && mustDestroyItself) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
//		Debug.Log ("Bullet collision");
//		Debug.Log (collision.gameObject.tag);
//		Debug.Log (collision.gameObject);
//		Debug.Log (emitter);
		if (collision.gameObject != this.emitter && collision.gameObject.tag != "Barrier") {
//			Debug.Log ("Deleting it");
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
