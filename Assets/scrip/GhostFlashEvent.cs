using System.Collections;
using UnityEngine;

public class GhostFlashEvent : MonoBehaviour
{
    [Header("Ghost")]
    public GameObject ghostObject;

    [Header("Light")]
    public Light ghostLight;

    [Header("Ghost Spawn Points")]
    public Transform[] ghostPoints;

    [Header("Settings")]
    public float flashIntensity = 8f;
    public float showTime = 3f;

    private bool isPlaying = false;

    // 첫 등장 여부
    private bool firstAppear = true;

    void Start()
    {
        if (ghostObject != null)
            ghostObject.SetActive(false);

        if (ghostLight != null)
        {
            ghostLight.enabled = false;
            ghostLight.intensity = 0f;
        }
    }

    public void ShowGhostOnce()
    {
        if (isPlaying) return;

        StartCoroutine(GhostRoutine());
    }

    IEnumerator GhostRoutine()
    {
        isPlaying = true;

        Debug.Log("귀신 이벤트 실행");

        // 첫 등장 이후부터 랜덤 위치 이동
        if (!firstAppear)
        {
            if (ghostObject != null &&
                ghostPoints != null &&
                ghostPoints.Length > 0)
            {
                int index = Random.Range(0, ghostPoints.Length);

                ghostObject.transform.position =
                    ghostPoints[index].position;

                ghostObject.transform.rotation =
                    ghostPoints[index].rotation;
            }
        }

        // 첫 등장 끝 처리
        firstAppear = false;

        // 귀신 등장
        if (ghostObject != null)
            ghostObject.SetActive(true);

        // 조명
        if (ghostLight != null)
        {
            ghostLight.enabled = true;
            ghostLight.intensity = flashIntensity;
        }

        yield return new WaitForSeconds(showTime);

        // 조명 끄기
        if (ghostLight != null)
        {
            ghostLight.intensity = 0f;
            ghostLight.enabled = false;
        }

        // 귀신 숨김
        if (ghostObject != null)
            ghostObject.SetActive(false);

        isPlaying = false;
    }
}