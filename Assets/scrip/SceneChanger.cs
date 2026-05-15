using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;

    public GameObject mainSceneObjects;
    public GameObject loadingCanvas;
    public float loadingTime = 120;

    private void Awake()
    {
     
            instance = this;

        

        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(false);
        }
    }

    public void LoadSceneWithLoading(string sceneName)
    {
        StartCoroutine(LoadRoutine(sceneName));
    }
    public void StartGame()
    {
        LoadSceneWithLoading("scrin/main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadRoutine(string sceneName)
    {
        if (mainSceneObjects != null)
        {
            mainSceneObjects.SetActive(false);
        }

        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(true);
        }

        yield return new WaitForSeconds(loadingTime);

        SceneManager.LoadScene(sceneName);
    }
}