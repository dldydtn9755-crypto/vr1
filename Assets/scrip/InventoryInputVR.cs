using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInputVR : MonoBehaviour
{
    public GameObject inventoryUI;

    public InputActionProperty openInventoryAction;

    void Update()
    {
        if (openInventoryAction.action.WasPressedThisFrame())
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
}