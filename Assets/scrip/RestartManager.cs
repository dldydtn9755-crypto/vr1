using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    public string spawnPointName = "HouseSpawn";

    public void RestartGame()
    {
        PlayerPrefs.SetString("SpawnPoint", spawnPointName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}