using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LockInteract : MonoBehaviour
{
    public GameObject lockMessageCanvas;
    public float messageTime = 2f;

    private XRBaseInteractable interactable;
    private bool isShowingMessage = false;

    private void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnLockSelected);

        if (lockMessageCanvas != null)
        {
            lockMessageCanvas.SetActive(false);
        }
    }

    private void OnLockSelected(SelectEnterEventArgs args)
    {
        if (!InventoryManager.instance.HasItem("Key"))
        {
            if (!isShowingMessage)
            {
                StartCoroutine(ShowLockedMessage());
            }
        }
        else
        {
            SceneChanger.instance.LoadSceneWithLoading("in home");
        }
    }

    IEnumerator ShowLockedMessage()
    {
        isShowingMessage = true;

        if (lockMessageCanvas != null)
        {
            lockMessageCanvas.SetActive(true);
        }

        yield return new WaitForSeconds(messageTime);

        if (lockMessageCanvas != null)
        {
            lockMessageCanvas.SetActive(false);
        }

        isShowingMessage = false;
    }
}