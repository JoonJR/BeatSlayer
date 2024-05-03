using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SaberCollision : MonoBehaviour
{
    public ParticleSystem collisionEffect; // Assign a particle system prefab
    private XRBaseController controller; // Controller for haptic feedback

    void Start()
    {
        // Attempt to get the XR controller associated with the saber
        controller = GetComponentInParent<XRBaseController>();
        if (controller == null)
        {
            Debug.LogError("XRBaseController not found on parent GameObjects!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with another saber
        if (collision.gameObject.CompareTag("Saber"))
        {
            TriggerEffects();
        }
    }

    private void TriggerEffects()
    {
        // Trigger Haptic Feedback
        if (controller != null)
        {
            controller.SendHapticImpulse(0.5f, 0.2f); 
        }

        // Trigger Particle Effect
        if (collisionEffect != null)
        {
            Instantiate(collisionEffect, transform.position, Quaternion.identity).Play();
        }
        else
        {
            Debug.LogError("No collision effect assigned!");
        }
    }
}
