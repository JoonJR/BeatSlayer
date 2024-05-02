using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed = 35f;
    public float lifetime = 7.0f;  // Time in seconds after which the object will self-destruct

    private float timeAlive;

    private void FixedUpdate()
    {
        // Move the object forward
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // Increment the time the object has been alive
        timeAlive += Time.deltaTime;

        if (transform.position.z <= 25)
        {
            speed = 9f;
            if (transform.position.z <= 0)
            {
                speed = 70f;
                // Check if the object has lived past its lifetime and destroy it if so
                if (timeAlive > lifetime)
                {
                    Destroy(gameObject);
                }
            }
        }   
    }
}