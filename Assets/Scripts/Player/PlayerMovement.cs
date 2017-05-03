using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

public class PlayerMovement : NetworkBehaviour
{
    public float maxSpeed = 30f;
    public float maxRotateSpeed = 3f;
	public float acceleration = 30;
	public float drag = 50;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	private float m_Speed = 0;
	private Vector3 m_LastPosition;
	private Rigidbody m_Rigidbody;  

	private void Awake ()
	{
		m_Rigidbody = GetComponent<Rigidbody> ();
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
		Rotate(h * m_Speed / maxSpeed);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}

    }

    void Move (float v)
    {
		float measuredSpeed = (transform.position - m_LastPosition).magnitude / Time.deltaTime * (m_Speed > 0 ? 1 : -1);
		m_Speed = measuredSpeed;

		m_Speed += acceleration * Time.deltaTime * v;

		// If the speed and the user joystick have different directions, or if v is zero, add the drag
		if (v * m_Speed <= 0) {
			m_Speed -= drag * Time.deltaTime * m_Speed / maxSpeed;

			if (Mathf.Abs(m_Speed) < 0.5)
				m_Speed = 0;
		}

		if (m_Speed > maxSpeed) {
			m_Speed = maxSpeed;
		} else if (m_Speed < -maxSpeed) {
			m_Speed = -maxSpeed;
		}

		m_LastPosition = transform.position;

		Vector3 movement = transform.forward * m_Speed * Time.deltaTime;

		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    void Rotate (float h)
    {
		float rotation = h * maxRotateSpeed;

        transform.Rotate(0, rotation, 0);
    }

	[Command]
	void CmdFire()
	{

		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = 50 * bullet.transform.forward;
		bullet.GetComponent<Rigidbody> ().position += transform.forward.normalized;
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