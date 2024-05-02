using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;

public class HapticUI : MonoBehaviour
{
    private XRController controller1;
    private XRController controller2;

    void Awake()
    {
        AssignEventTriggersToButtons();
    }

    void Start()
    {
        FindAndAssignControllers();
    }

    private void AssignEventTriggersToButtons()
    {
        // Find all buttons under this GameObject and add event triggers
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entry.callback.AddListener((data) => OnHoverEnter(button));
            trigger.triggers.Add(entry);
        }
    }

    private void FindAndAssignControllers()
    {
        // Find and assign controllers
        XRController[] controllers = FindObjectsOfType<XRController>();
        if (controllers.Length > 0) controller1 = controllers[0];
        if (controllers.Length > 1) controller2 = controllers[1];
    }

    private void OnHoverEnter(Button button)
    {
        SendHapticFeedback(controller1);
        SendHapticFeedback(controller2);
    }

    private void SendHapticFeedback(XRController controller)
    {
        if (controller != null && controller.inputDevice.isValid)
        {
            XRRayInteractor interactor = controller.GetComponent<XRRayInteractor>();
            if (interactor != null && interactor.enabled)
            {
                controller.inputDevice.SendHapticImpulse(0, 1, 0.5f);
                Debug.Log("Haptic feedback sent on hover over: ");
            }
        }
    }
}