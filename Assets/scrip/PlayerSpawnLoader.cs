using UnityEngine;

public class PlayerSpawnLoader : MonoBehaviour
{
    public Transform xrRig;

    void Start()
    {
        string spawnName = PlayerPrefs.GetString("SpawnPoint", "");

        if (spawnName != "")
        {
            GameObject spawn = GameObject.Find(spawnName);

            if (spawn != null)
            {
                xrRig.position = spawn.transform.position;
                xrRig.rotation = spawn.transform.rotation;
            }

            PlayerPrefs.DeleteKey("SpawnPoint");
        }
    }
}