using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class RayController : MonoBehaviour
{
    public XRInteractorLineVisual lineVisual;
    public XRBaseControllerInteractor controllerInteractor;

  

    private void Start()
    {
        AssignComponents();
    }
   

    private void AssignComponents()
    {
        if (!lineVisual)
            lineVisual = GetComponentInChildren<XRInteractorLineVisual>(true);
        if (!controllerInteractor)
            controllerInteractor = GetComponentInChildren<XRBaseControllerInteractor>(true);

        // Log an error if components are still missing after an attempt to assign them.
        if (!lineVisual || !controllerInteractor)
            Debug.LogError("RayController: Failed to find necessary components on " + gameObject.name, this);
    }
    
    public void EnableRayInteractor()
    {
        if (lineVisual != null && !lineVisual.enabled)
        {
            lineVisual.enabled = true;
            Debug.Log("Enabled Ray Visual on " + gameObject.name);
        }

        if (controllerInteractor != null && !controllerInteractor.enabled)
        {
            controllerInteractor.enabled = true;
            Debug.Log("Enabled Ray Interactor on " + gameObject.name);
        }
    }

    public void DisableRayInteractor()
    {
        if (lineVisual != null && lineVisual.enabled)
        {
            lineVisual.enabled = false;
            Debug.Log("Disabled Ray Visual on " + gameObject.name);
        }

        if (controllerInteractor != null && controllerInteractor.enabled)
        {
            controllerInteractor.enabled = false;
            Debug.Log("Disabled Ray Interactor on " + gameObject.name);
        }
    }
}