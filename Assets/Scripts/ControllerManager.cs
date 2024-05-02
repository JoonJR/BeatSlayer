using System.Linq;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    private static ControllerManager _instance;
    public static ControllerManager Instance;

    public RayController leftRayController;
    public RayController rightRayController;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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