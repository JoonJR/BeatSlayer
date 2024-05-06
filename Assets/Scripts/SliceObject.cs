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
    private Vector3 lastStartPoint;
    private Vector3 lastEndPoint;

    void Start()
    {
        // Try finding the controller by looking at the parent or further up
        controller = GetComponentInParent<XRBaseController>();
        if (controller == null)
        {
            Debug.LogError("XRBaseController not found on parent GameObjects!");
        }
        lastStartPoint = startSlicePoint.position;
        lastEndPoint = endSlicePoint.position;
    }

    void FixedUpdate()
    {
        Vector3 currentStartPoint = startSlicePoint.position;
        Vector3 currentEndPoint = endSlicePoint.position;

        // Interpolate between the last and current positions to cover all potential collision space
        int interpolationSteps = 10; 
        for (int i = 0; i <= interpolationSteps; i++)
        {
            float t = (float)i / interpolationSteps;
            Vector3 interpolatedStart = Vector3.Lerp(lastStartPoint, currentStartPoint, t);
            Vector3 interpolatedEnd = Vector3.Lerp(lastEndPoint, currentEndPoint, t);

            RaycastHit hit;
            if (Physics.Linecast(interpolatedStart, interpolatedEnd, out hit, sliceableLayers))
            {
                GameObject target = hit.transform.gameObject;
                Cube cube = target.GetComponent<Cube>();
                if (cube != null && cube.IsCorrectSliceDirection(velocityEstimator.GetVelocityEstimate()))
                {
                    AudioManager.Instance.PlaySliceEffect();
                    Slice(target);
                    
                    break; 
                }
            }
        }

        // Update last positions for the next frame
        lastStartPoint = currentStartPoint;
        lastEndPoint = currentEndPoint;
    }

    private void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();
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