using UnityEngine;
using UnityEngine.Networking;

public class FireEffect: ItemEffect
{
	public GameObject originalBulletPrefab;
	public GameObject fireBulletPrefab;

	void Start()
	{
		this.effectType = EffectType.NONE;
		this.player.GetComponent<PlayerMovement> ().bulletPrefab = fireBulletPrefab;
	}

	override protected void OnRemove () {
		this.player.GetComponent<PlayerMovement> ().bulletPrefab = originalBulletPrefab;
	}

	override protected void OnApply () {
	}
}


