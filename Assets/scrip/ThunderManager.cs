using UnityEngine;
using System.Collections;

public class ThunderManager : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        StartCoroutine(ThunderLoop());
    }

    IEnumerator ThunderLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(15f, 35f));

            audioSource.Play();
        }
    }
}