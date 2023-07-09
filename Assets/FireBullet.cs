using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float speed = 10f; // The speed of the bullet
    public float homingSpeed = 2f; // The speed at which the bullet adjusts its direction
    public string playerTag = "FPS Player"; // The tag of the player gameobject
    public LayerMask groundLayer; // The layer for the ground

    private Transform target; // Reference to the player gameobject

    private void Start()
    {
        // Find the player gameobject based on the tag
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Could not find the player gameobject with tag: " + playerTag);
            Destroy(gameObject); // Destroy the bullet if the player is not found
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Calculate the direction towards the player
            Vector3 targetDirection = (target.position - transform.position).normalized;

            // Gradually adjust the bullet's direction towards the player
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, homingSpeed * Time.deltaTime, 0f);

            // Update the bullet's rotation to face the new direction
            transform.rotation = Quaternion.LookRotation(newDirection);

            // Move the bullet forward
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet has collided with the ground
        if (groundLayer == (groundLayer | (1 << other.gameObject.layer)))
        {
            Destroy(gameObject); // Destroy the bullet when it touches the ground
        }
    }
}
