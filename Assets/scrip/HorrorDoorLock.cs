using UnityEngine;

public class HorrorDoorLock : MonoBehaviour
{
    [Header("Key")]
    public string requiredKeyName = "1Ãþ ¿­¼è";

    [Header("Scene Change")]
    public bool moveNextSceneAfterOpen = false;
    public string nextSceneName = "scrin/secretroom";
    public float sceneChangeDelay = 1.5f;

    [Header("Ghost Event")]
    public GhostFlashEvent ghostFlashEvent;

    public void TryOpenDoor()
    {
        // ¿­¼è È®ÀÎ
        if (HorrorInventoryManager.Instance.HasKey(requiredKeyName))
        {
            Debug.Log("¹® ¿­¸²!");

            // ±Í½Å ÀÌº¥Æ® ½ÇÇà
            if (ghostFlashEvent != null)
            {
                ghostFlashEvent.ShowGhostOnce();
            }
            else
            {
                Debug.LogWarning("GhostFlashEvent ¿¬°á ¾ÈµÊ!");
            }

            // ¹® Á¦°Å
            gameObject.SetActive(false);

            // ¾À ÀÌµ¿
            if (moveNextSceneAfterOpen)
            {
                Invoke(nameof(GoNextScene), sceneChangeDelay);
            }
        }
        else
        {
            Debug.Log("¿­¼è ¾øÀ½!");
        }
    }

    private void GoNextScene()
    {
        SceneChanger.instance.LoadSceneWithLoading(nextSceneName);
    }
}