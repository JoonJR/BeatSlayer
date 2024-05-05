using System.Linq;
using UnityEngine;
using System.Collections;
public class ControllerManager : MonoBehaviour
{
    
    public RayController leftRayController;
    public RayController rightRayController;

    public GameObject leftSaber;
    public GameObject rightSaber;


    private void Start()
    {
        StartCoroutine(FindSabersWithRetry());
    }
    private IEnumerator FindSabersWithRetry()
    {
        while (leftSaber == null || rightSaber == null)
        {
            leftSaber = GameObject.FindGameObjectWithTag("Saber");
            rightSaber = GameObject.FindGameObjectWithTag("Saber1");
            if (leftSaber == null || rightSaber == null)
            {
                yield return new WaitForSeconds(1); // Wait for 1 second before retrying
            }
        }
        Debug.Log("Sabers found!");
    }
    public void EnableSabersColliders()
    {
        ToggleSabersColliders(true);
    }

    public void DisableSabersColliders()
    {
        ToggleSabersColliders(false);
    }
    private void ToggleSabersColliders(bool enabled)
    {
        if (leftSaber != null)
        {
            var collider = leftSaber.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = enabled;
            }
        }

        if (rightSaber != null)
        {
            var collider = rightSaber.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = enabled;
            }
        }
    }
    public void EnableAllInteractors()
    {
        RefreshControllers(); // Refresh references before enabling.
        leftRayController?.EnableRayInteractor();
        rightRayController?.EnableRayInteractor();
    }

    public void DisableAllInteractors()
    {
        RefreshControllers(); // Refresh references before disabling.
        leftRayController?.DisableRayInteractor();
        rightRayController?.DisableRayInteractor();
    }

    private void RefreshControllers()
    {
        // Attempt to find the controllers again if null.
        if (!leftRayController || !rightRayController)
        {
            leftRayController = FindObjectsOfType<RayController>().FirstOrDefault();
            rightRayController = FindObjectsOfType<RayController>().LastOrDefault();
        }
    }
}