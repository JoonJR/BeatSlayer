using EzySlice;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using Valve.VR.InteractionSystem;

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayers;
    public Material crossSectionMaterial;
    public float cutForce = 2000f;

    private XRBaseController controller; // Reference to the XR controller for haptic feedback

    void Start()
    {
        // Try finding the controller by looking at the parent or further up
        controller = GetComponentInParent<XRBaseController>();
        if (controller == null)
        {
            Debug.LogError("XRBaseController not found on parent GameObjects!");
        }
    }

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayers);
        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Cube cube = target.GetComponent<Cube>();
            if (cube != null && cube.IsCorrectSliceDirection(velocityEstimator.GetVelocityEstimate()))
            {
                Slice(target);
            }
        }
    }

    private void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity).normalized;
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal, crossSectionMaterial);
        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedComponent(upperHull);
            SetupSlicedComponent(lowerHull);

            // Trigger haptic feedback if controller is found
            if (controller != null)
            {
                controller.SendHapticImpulse(0.9f, 0.1f);
                Debug.Log("Vibrating;D");
            }
            else
            {
                Debug.LogWarning("Controller not found, no haptic feedback triggered.");
            }

            // Update score and combo
            if (gameObject.scene.name != "DebugScene")
            {
                ScoreManager.Instance.AddScore(25);
                ScoreManager.Instance.IncrementCombo();

            }
            

            Destroy(target);
        }
    }

    private void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
        collider.isTrigger = true;
        Destroy(slicedObject, 2.0f);
    }
}