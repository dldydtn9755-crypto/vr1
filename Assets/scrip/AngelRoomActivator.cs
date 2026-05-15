using UnityEngine;

public class AngelRoomActivator : MonoBehaviour
{
    [Header("Player")]
    public string playerTag = "Player";

    [Header("Angel")]
    public GameObject angelObject;

    [Header("Option")]
    public bool activateOnlyOnce = true;

    private bool hasActivated = false;

    private void Start()
    {
        if (angelObject != null)
        {
            angelObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activateOnlyOnce && hasActivated) return;

        if (other.CompareTag(playerTag) || other.transform.root.CompareTag(playerTag))
        {
            hasActivated = true;

            if (angelObject != null)
            {
                angelObject.SetActive(true);
            }
        }
    }
}