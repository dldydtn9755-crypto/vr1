using TMPro;
using UnityEngine;
using System.Collections;

public class BatteryManager : MonoBehaviour
{
    [Header("Battery Settings")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float drainSpeed = 0.3f;

    [Header("UI")]
    public TextMeshProUGUI batteryText;
    public GameObject noiseImage;
    public GameObject camcorderOverlay;

    [Header("Low Battery Audio")]
    public AudioSource heartbeatAudio;
    public AudioSource noiseAudio;

    [Header("Black Screen")]
    public GameObject blackScreen;
    public float blackScreenDuration = 1f;

    [Header("Battery Death")]
    public GameObject deathScareCanvas;
    public GameObject gameOverCanvas;
    public AudioSource deathAudioSource;
    public AudioClip deathScareSound;
    public float deathScareDuration = 2f;

    [Header("Destroy Objects")]
    public GameObject house;

    private bool isBatteryDead = false;
    private float noiseTimer = 0f;
    private float nextNoiseTime = 0f;

    void Start()
    {
        currentBattery = maxBattery;
        UpdateBatteryUI();

        if (noiseImage != null)
            noiseImage.SetActive(false);

        if (blackScreen != null)
            blackScreen.SetActive(false);

        if (deathScareCanvas != null)
            deathScareCanvas.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        if (camcorderOverlay != null)
            camcorderOverlay.SetActive(true);

        if (heartbeatAudio != null)
            heartbeatAudio.Stop();

        if (noiseAudio != null)
            noiseAudio.Stop();

        SetNextNoiseTime();
    }

    void Update()
    {
        if (isBatteryDead) return;

        currentBattery -= drainSpeed * Time.deltaTime;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

        UpdateBatteryUI();
        CheckLowBatteryEffect();

        if (currentBattery <= 0f)
        {
            BatteryDead();
        }
    }

    public void RechargeBattery(float amount)
    {
        if (isBatteryDead) return;

        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

        UpdateBatteryUI();
        CheckLowBatteryEffect();

        Debug.Log("배터리 충전됨: " + currentBattery);
    }

    void UpdateBatteryUI()
    {
        if (batteryText != null)
        {
            batteryText.text = "BATTERY " + Mathf.CeilToInt(currentBattery) + "%";
        }
    }

    void CheckLowBatteryEffect()
    {
        if (currentBattery <= 30f)
        {
            if (heartbeatAudio != null && !heartbeatAudio.isPlaying)
                heartbeatAudio.Play();

            if (noiseAudio != null && !noiseAudio.isPlaying)
                noiseAudio.Play();

            if (noiseImage != null)
            {
                noiseTimer += Time.deltaTime;

                if (noiseTimer >= nextNoiseTime)
                {
                    noiseImage.SetActive(!noiseImage.activeSelf);
                    noiseTimer = 0f;
                    SetNextNoiseTime();
                }
            }
        }
        else
        {
            if (noiseImage != null)
                noiseImage.SetActive(false);

            if (heartbeatAudio != null && heartbeatAudio.isPlaying)
                heartbeatAudio.Stop();

            if (noiseAudio != null && noiseAudio.isPlaying)
                noiseAudio.Stop();
        }
    }

    void SetNextNoiseTime()
    {
        nextNoiseTime = Random.Range(0.2f, 0.8f);
    }

    void BatteryDead()
    {
        isBatteryDead = true;

        if (noiseImage != null)
            noiseImage.SetActive(false);

        if (heartbeatAudio != null)
            heartbeatAudio.Stop();

        if (noiseAudio != null)
            noiseAudio.Stop();

        StartCoroutine(BatteryDeathSequence());
    }

    IEnumerator BatteryDeathSequence()
    {
        if (camcorderOverlay != null)
            camcorderOverlay.SetActive(false);

        if (batteryText != null)
            batteryText.gameObject.SetActive(false);

        if (blackScreen != null)
            blackScreen.SetActive(true);

        yield return new WaitForSeconds(blackScreenDuration);

        if (house != null)
            Destroy(house);

        if (blackScreen != null)
            blackScreen.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        if (deathScareCanvas != null)
            deathScareCanvas.SetActive(true);

        if (deathAudioSource != null && deathScareSound != null)
            deathAudioSource.PlayOneShot(deathScareSound);

        yield return new WaitForSeconds(deathScareDuration);

        if (deathScareCanvas != null)
            deathScareCanvas.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);
    }
}