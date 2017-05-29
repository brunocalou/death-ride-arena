using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

public class PlayerMovement : NetworkBehaviour
{
    public float maxSpeed = 25f;
    public float maxRotateSpeed = 2f;
//	public float acceleration = 30;
//	public float drag = 50;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;

//	private float m_Speed = 25f;
	private Vector3 m_LastPosition;
	private Rigidbody m_Rigidbody;
	private float m_fireRate = 0.5f; // 2 fires per second
	private float m_fireStartTime = 0;

	private void Awake ()
	{
		m_Rigidbody = GetComponent<Rigidbody> ();
//		m_Rigidbody.drag = 1f;
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
//		Rotate(h * m_Speed / maxSpeed);
		Rotate(h * (v < 0 ? -1: 1));

		if ((Time.time - m_fireStartTime > m_fireRate) && (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Z)))
		{
			m_fireStartTime = Time.time;
			CmdFire(transform.forward);
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			RaycastHit floorHit;
//
//			if (Physics.Raycast (ray, out floorHit)) {
//				// Create a vector from the player to the point on the floor the raycast from the mouse hit.
//				Vector3 playerToMouse = floorHit.point - transform.position;
//
//				// Ensure the vector is entirely along the floor plane.
//				playerToMouse.y = 0f;
//
//				if (Vector3.Angle (playerToMouse, transform.forward) < 45) {
//					CmdFire (playerToMouse.normalized);
//				}
//			}
		}

//		if (Input.GetKeyDown(KeyCode.Space))
//		{
//			CmdFire(transform.forward);
//		}

    }

    void Move (float v)
    {
//		float measuredSpeed = (transform.position - m_LastPosition).magnitude / Time.deltaTime * (m_Speed > 0 ? 1 : -1);
//		m_Speed = measuredSpeed;
//
//		m_Speed += acceleration * Time.deltaTime * v;
//
//		// If the speed and the user joystick have different directions, or if v is zero, add the drag
//		if (v * m_Speed <= 0) {
//			m_Speed -= drag * Time.deltaTime * m_Speed / maxSpeed;
//
//			if (Mathf.Abs(m_Speed) < 0.5)
//				m_Speed = 0;
//		}
//
//		if (m_Speed > maxSpeed) {
//			m_Speed = maxSpeed;
//		} else if (m_Speed < -maxSpeed) {
//			m_Speed = -maxSpeed;
//		}
//
//		m_LastPosition = transform.position;

		Vector3 movement = transform.forward * maxSpeed * v * Time.deltaTime;
//
		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

//		m_Rigidbody.AddForce (transform.forward * v * m_Speed, ForceMode.Force);
    }

    void Rotate (float h)
    {
		transform.Rotate (0, h * maxRotateSpeed, 0);
//		m_Rigidbody.AddTorque(0, h * maxRotateSpeed, 0, ForceMode.VelocityChange);
    }

	[Command]
	void CmdFire(Vector3 forward)
	{

		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

//		Physics.IgnoreCollision (bullet.GetComponent<Collider> (), GetComponent<Collider> ());
		Bullet bulletScript = bullet.GetComponent<Bullet>();
		Debug.Log (bulletScript);
		bulletScript.emitter = this.gameObject;

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = 50 * forward;
//		bullet.GetComponent<Rigidbody> ().position += forward.normalized;
		// Spawn the bullet to the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}

	public override void OnStartLocalPlayer()
	{
		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
			renderer.material.color = Color.red;
		}

		GameObject camera = GameObject.FindWithTag ("MainCamera");
		if (camera != null) {
			SmoothFollow follow = camera.GetComponent<SmoothFollow>();
			follow.target = transform;
		}
	}
}