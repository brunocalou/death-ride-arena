using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 10f;            // The speed that the player will move at.
    public float rotateSpeed = 3f;       // The speed to rotate the player.

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
        transform.position += transform.forward * Time.deltaTime * speed * v;
    }

    void Rotate (float h)
    {
        float rotation = h * rotateSpeed;

        transform.Rotate(0, rotation, 0);
    }

	public override void OnStartLocalPlayer()
	{
		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
			renderer.material.color = Color.red;
		}
	}
}