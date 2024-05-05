using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SaberCollision : MonoBehaviour
{
    public ParticleSystem collisionEffect;
    private ParticleSystem effectInstance;
    private XRBaseController[] controllers;
    public string targetTag;
    void Start()
    {
        controllers = GetComponentsInParent<XRBaseController>();
        if (collisionEffect != null)
        {
            effectInstance = Instantiate(collisionEffect);
            effectInstance.Stop();
        }
    }

    void Update()
    {
        // Check for collisions and trigger effects
        CheckCollisions();
    }

    void CheckCollisions()
    {
        RaycastHit hit;
        Transform saberStartingPoint = transform.Find("StartSlicePoint");
        if (saberStartingPoint == null)
        {
            Debug.LogError("SaberStartingPoint not found!");
            return;
        }
        // Calculate the ray direction based on the current rotation of the saber
        Vector3 rayDirection = transform.forward;
        Quaternion rotation = Quaternion.LookRotation(rayDirection, transform.up); // Calculate rotation based on forward direction and up direction of the saber
        rayDirection = rotation * Quaternion.Euler(0, 90, 0) * Vector3.forward; // Apply additional rotation
        Vector3 rayStartPoint = saberStartingPoint.position; // Use the position of the child object as the starting point
        float rayLength = 1f; // Adjust the length of the ray 
        Debug.DrawRay(rayStartPoint, rayDirection * rayLength, Color.blue);
        if (Physics.Raycast(rayStartPoint, rayDirection, out hit, rayLength))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                Debug.Log("Checking for collisions2");
                // Play effect at collision point
                MoveAndPlayEffect(hit.point);
                TriggerHapticFeedback();
            }
        }
        else
        {
            if (effectInstance != null && effectInstance.isPlaying)
                effectInstance.Stop();
        }
    }

    void MoveAndPlayEffect(Vector3 collisionPoint)
    {
        if (effectInstance != null)
        {
            effectInstance.transform.position = collisionPoint;
            if (!effectInstance.isPlaying)
            {
                effectInstance.Play(true);
            }
            else
            {
                effectInstance.Stop(true);
                effectInstance.Play(true);
            }
        }
    }

    void TriggerHapticFeedback()
    {
        foreach (var controller in controllers)
        {
            if (controller != null)
            {
                controller.SendHapticImpulse(0.5f, 0.2f);
            }
        }
    }
}