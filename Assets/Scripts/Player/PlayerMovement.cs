using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

public class PlayerMovement : NetworkBehaviour
{
    public float maxSpeed = 25f;
    public float maxRotateSpeed = 2f;
	private Vector3 m_currentVelocity = new Vector3();

	public GameObject bulletPrefab;
	public Rigidbody bulletSpawn;
//	protected GameObject effectbarCanvas;

//	private float m_Speed = 25f;
	private Vector3 m_lastPosition;
	private Rigidbody m_rigidbody;
	private float m_fireRate = 0.5f; // 2 fires per second
	private float m_fireStartTime = 0;

	private void Awake ()
	{
		m_rigidbody = GetComponent<Rigidbody> ();
//		effectbarCanvas = this.transform.Find ("Effectbar Canvas").gameObject;
//		effectbarCanvas.SetActive (false);
	}

    void FixedUpdate ()
    {
		if (!isLocalPlayer) {
			return;
		}

        // Store the input axes.
        float h = Input.GetAxis ("Horizontal");
        float v = Input.GetAxis ("Vertical");

        // Move the player around the scene.
		Move (v);

        // Rotate the player
		Rotate(h);

		if ((Time.time - m_fireStartTime > m_fireRate) && (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Z)))
		{
			m_fireStartTime = Time.time;
			fire (bulletSpawn.position, bulletSpawn.transform.forward, m_currentVelocity, GetComponent<NetworkIdentity>().netId);
    	}
	}

    void Move (float v)
    {

		m_currentVelocity = m_rigidbody.transform.forward * maxSpeed * v;

		Vector3 movement = m_currentVelocity * Time.deltaTime;
		m_rigidbody.MovePosition(m_rigidbody.position + movement);

    }

    void Rotate (float h)
    {
		transform.Rotate (0, h * maxRotateSpeed, 0);
    }

	public void fire (Vector3 position, Vector3 direction, Vector3 velocity, NetworkInstanceId emitterId) {
		if (isServer) {
			RpcFire(position, direction, velocity, emitterId);
		} else {
			CmdFire(position, direction, velocity, emitterId);
		}
	}

	[Command]
	void CmdFire (Vector3 position, Vector3 direction, Vector3 velocity, NetworkInstanceId emitterId) {
		fire (position, direction, velocity, emitterId);
	}

	[ClientRpc]
	void RpcFire (Vector3 position, Vector3 direction, Vector3 velocity, NetworkInstanceId emitterId) {
		if (!isServer)
			return;
		
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			position,
			bulletSpawn.rotation);

		Bullet bulletScript = bullet.GetComponent<Bullet>();
		bulletScript.emitter = this.gameObject;
		bulletScript.emitterId = emitterId;

		Vector3 scale = bullet.transform.localScale;
		bullet.transform.localScale = scale * transform.localScale.x;

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = (50 + velocity.magnitude) * direction;
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}

	public override void OnStartLocalPlayer()
	{
//		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
//			renderer.material.color = Color.red;
//		}

		GameObject camera = GameObject.FindWithTag ("MainCamera");
		if (camera != null) {
			SmoothFollow follow = camera.GetComponent<SmoothFollow>();
			follow.target = transform;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (!isServer)
			return;
//		Debug.Log ("Player collision");
		if (collision.gameObject.GetComponent<InstantKill> () != null) {
			RpcKill (gameObject.GetComponent<NetworkIdentity>().netId);
		}
	}

	[ClientRpc]
	void RpcKill (NetworkInstanceId playerId) {
		if (!isServer)
			return;
		GameObject player = ClientScene.FindLocalObject (playerId);
		Health health = player.GetComponent<Health> ();
		health.RpcKill ();
	}
}