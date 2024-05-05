using UnityEngine;

public class XRGravityManager : MonoBehaviour
{
    private Rigidbody xrRigidbody;

    void Start()
    {
        xrRigidbody = GetComponent<Rigidbody>();
    }

    public void EnableGravity()
    {

        xrRigidbody.isKinematic = false; // Allow physics to affect the rig
        xrRigidbody.useGravity = true;   // Enable gravity
        
    }

    public void DisableGravity()
    {
        xrRigidbody.isKinematic = true;  // Physics no longer affects the rig
        xrRigidbody.useGravity = false;  // Disable gravity
        
    }
}