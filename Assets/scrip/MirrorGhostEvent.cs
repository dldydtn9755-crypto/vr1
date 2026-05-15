using System.Collections;
using UnityEngine;

public class MirrorGhostEvent : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Ghost Object")]
    public GameObject ghost;

    [Header("Ghost Positions")]
    public Transform farPosition;
    public Transform nearPosition;

    [Header("Light")]
    public Light eventLight;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip whisperClip;
    public AudioClip screamClip;

    [Header("Layer Settings")]
    public string mirrorOnlyLayerName = "ghost";
    public string visibleLayerName = "Default";

    [Header("Timing")]
    public float firstDelay = 1.0f;
    public float moveDelay = 2.0f;
    public float disappearDelay = 1.2f;

    private bool hasPlayed = false;
    private int mirrorOnlyLayer;
    private int visibleLayer;

    private void Start()
    {
        mirrorOnlyLayer = LayerMask.NameToLayer(mirrorOnlyLayerName);
        visibleLayer = LayerMask.NameToLayer(visibleLayerName);

        if (mirrorOnlyLayer == -1)
        {
            Debug.LogError("ghost 레이어를 찾을 수 없습니다. Layer 이름을 확인하세요.");
        }

        if (visibleLayer == -1)
        {
            Debug.LogError("Default 레이어를 찾을 수 없습니다. Layer 이름을 확인하세요.");
        }

        if (ghost != null)
        {
            SetLayerRecursively(ghost, mirrorOnlyLayer);
            ghost.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player") || other.name.Contains("XR Origin"))
        {
            hasPlayed = true;
            StartCoroutine(PlayMirrorEvent());
        }
    }

    private IEnumerator PlayMirrorEvent()
    {
        yield return new WaitForSeconds(firstDelay);

        // 1. 처음에는 ghost 레이어로 설정해서 거울 카메라에만 보이게 함
        if (ghost != null && farPosition != null)
        {
            SetLayerRecursively(ghost, mirrorOnlyLayer);

            ghost.transform.position = farPosition.position;
            ghost.transform.rotation = farPosition.rotation;
            ghost.SetActive(true);
        }

        if (audioSource != null && whisperClip != null)
        {
            audioSource.PlayOneShot(whisperClip);
        }

        yield return new WaitForSeconds(moveDelay);

        // 2. 조명 깜빡임
        if (eventLight != null)
        {
            yield return StartCoroutine(FlickerLight());
        }

        // 3. 가까운 위치로 순간이동 후 Default 레이어로 바꿔서 플레이어도 보이게 함
        if (ghost != null && nearPosition != null)
        {
            ghost.transform.position = nearPosition.position;
            ghost.transform.rotation = nearPosition.rotation;

            SetLayerRecursively(ghost, visibleLayer);
        }

        if (audioSource != null && screamClip != null)
        {
            audioSource.PlayOneShot(screamClip);
        }

        yield return new WaitForSeconds(disappearDelay);

        // 4. 사라질 때 다시 ghost 레이어로 복구
        if (ghost != null)
        {
            SetLayerRecursively(ghost, mirrorOnlyLayer);
            ghost.SetActive(false);
        }
    }

    private IEnumerator FlickerLight()
    {
        float originalIntensity = eventLight.intensity;

        eventLight.intensity = 0f;
        yield return new WaitForSeconds(0.08f);

        eventLight.intensity = originalIntensity;
        yield return new WaitForSeconds(0.08f);

        eventLight.intensity = 0f;
        yield return new WaitForSeconds(0.12f);

        eventLight.intensity = originalIntensity;
        yield return new WaitForSeconds(0.1f);

        eventLight.intensity = 0f;
        yield return new WaitForSeconds(0.08f);

        eventLight.intensity = originalIntensity;
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        if (obj == null || layer < 0) return;

        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}