using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

public class PlayerMovement : NetworkBehaviour
{
    public float maxSpeed = 30f;
    public float maxRotateSpeed = 3f;
	public float acceleration = 20;
	public float drag = 50;

	private float speed = 0;
	private Vector3 lastPosition;

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
        Rotate(h * v);
    }

    void Move (float v)
    {
		float measuredSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime * (speed > 0 ? 1 : -1);
		speed = measuredSpeed;

		speed += acceleration * Time.deltaTime * v;

		// If the speed and the user joystick have different directions, or if v is zero, add the drag
		if (v * speed <= 0) {
			speed -= drag * Time.deltaTime * speed / maxSpeed;

			if (Mathf.Abs(speed) < 0.5)
				speed = 0;
		}

		if (speed > maxSpeed) {
			speed = maxSpeed;
		} else if (speed < -maxSpeed) {
			speed = -maxSpeed;
		}

		lastPosition = transform.position;

        transform.position += transform.forward * Time.deltaTime * speed;
    }

    void Rotate (float h)
    {
		float rotation = h * maxRotateSpeed;

        transform.Rotate(0, rotation, 0);
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