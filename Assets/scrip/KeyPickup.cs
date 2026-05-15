using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeyPickup : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private bool isGrabbed = false;
    private bool isPicked = false;

    private Transform attachTransform;

    public float pickupDistance = 0.2f;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (isPicked) return;

        isGrabbed = true;

        if (args.interactorObject != null)
        {
            attachTransform = args.interactorObject.transform;
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (isPicked) return;

        isGrabbed = false;
        attachTransform = null;
    }

    private void Update()
    {
        if (isPicked) return;
        if (!isGrabbed) return;
        if (attachTransform == null) return;

        float distance = Vector3.Distance(transform.position, attachTransform.position);

        if (distance <= pickupDistance)
        {
            PickUpKey();
        }
    }

    private void PickUpKey()
    {
        isPicked = true;

        InventoryManager.instance.AddItem("Key");
        InventoryManager.instance.ShowInventory();
        Debug.Log("¿­¼è¸¦ È¹µæÇß½À´Ï´Ù.");

        gameObject.SetActive(false);
    }
}