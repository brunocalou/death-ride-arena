using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public abstract class ItemEffect: NetworkBehaviour
{
	[SyncVar]
	public NetworkInstanceId playerId;
	public GameObject player;
	public GameObject instantiatedPrefab;
	protected EffectType effectType;

	protected abstract void OnRemove();

//	private RectTransform effectBar;
//	private GameObject effectBarCanvas;

	protected float maxLifetime = 10; // seconds

	[SyncVar(hook = "OnChangeLifeTime")]
	protected float lifetime;

	private float lifetimeStart;

	void FixedUpdate ()
	{
		lifetime = maxLifetime - (Time.time - lifetimeStart);
		if (lifetime <= 0) {
			this.remove ();
			Destroy (gameObject);
		}
	}

	public void apply ()
	{
//		effectBarCanvas = player.GetComponent<PlayerMovement> ().effectbarCanvas;
//		effectBarCanvas.SetActive (true);
//
//		RectTransform[] transform = effectBarCanvas.GetComponentsInChildren<RectTransform>();
//		
//		foreach (RectTransform t in transform) {
//			if (t.name == "EffectbarForeground") {
//				effectBar = t;
//				break;
//			}
//		}

		lifetime = maxLifetime;
		lifetimeStart = Time.time;
	}

	public void remove ()
	{
		this.OnRemove ();
//		effectBarCanvas.SetActive (false);

		if (this.instantiatedPrefab != null) {
			Destroy (this.instantiatedPrefab);
		}
	}

	void OnChangeLifeTime (float currentLifetime)
	{
//		effectBar.sizeDelta = new Vector2(100 * currentLifetime / maxLifetime, effectBar.sizeDelta.y);
	}

	public EffectType getEffectType () {
		return this.effectType;
	}
}
