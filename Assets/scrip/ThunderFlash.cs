using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThunderFlash : MonoBehaviour
{
    [Header("Light")]
    public Light directionalLight;

    public float normalIntensity = 0.2f;
    public float flashIntensity = 3f;

    [Header("Screen Flash")]
    public Image flashImage;

    [Range(0f, 1f)]
    public float flashAlpha = 0.4f;

    [Header("Timing")]
    public float minDelay = 8f;
    public float maxDelay = 15f;

    [Header("Thunder Sound")]
    public AudioSource thunderAudioSource;
    public AudioClip thunderClip;
    public float thunderSoundDelay = 0.4f;

    void Start()
    {
        if (directionalLight != null)
            directionalLight.intensity = normalIntensity;

        if (flashImage != null)
        {
            Color c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }

        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            // 첫 번째 번개
            FlashOn();

            yield return new WaitForSeconds(0.15f);

            FlashOff();

            yield return new WaitForSeconds(0.1f);

            // 두 번째 약한 번개
            FlashOn(flashIntensity * 0.7f, flashAlpha * 0.7f);

            yield return new WaitForSeconds(0.1f);

            FlashOff();

            yield return new WaitForSeconds(thunderSoundDelay);

            if (thunderAudioSource != null && thunderClip != null)
            {
                thunderAudioSource.PlayOneShot(thunderClip);
            }
        }
    }

    void FlashOn(float lightIntensity = -1f, float alpha = -1f)
    {
        if (directionalLight != null)
        {
            directionalLight.intensity =
                lightIntensity > 0 ? lightIntensity : flashIntensity;
        }

        if (flashImage != null)
        {
            Color c = flashImage.color;
            c.a = alpha > 0 ? alpha : flashAlpha;
            flashImage.color = c;
        }
    }

    void FlashOff()
    {
        if (directionalLight != null)
        {
            directionalLight.intensity = normalIntensity;
        }

        if (flashImage != null)
        {
            Color c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }
    }
}