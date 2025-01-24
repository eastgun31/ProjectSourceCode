using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InWater : MonoBehaviour
{
    string water = "Water";
    WaitForSeconds delay = new WaitForSeconds(10f);
    WaitForSeconds delay2 = new WaitForSeconds(1f);
    Color outColor = Color.black;
    Color inColor = new Color(28/255f, 43/255f, 39/255f);
    float inRange = 0.1f;
    float outRange = 0.01f;
    [SerializeField]
    private GameObject inWater;
    [SerializeField]
    private MeshRenderer watermesh;
    [SerializeField]
    private GameObject bubble;
    [SerializeField]
    private PlayerMovement player;
    public Image[] hit;
    GameManager gm;

    private void Start()
    {
        ResetImages();
        gm = GameManager.instance;
        player = GameObject.FindAnyObjectByType<PlayerMovement>();
    }
    public void _Reset()
    {
        StopAllCoroutines();
        ResetImages();
    }
    void InWaterSound()
    {
        SoundManager.instance.Player_paly(5, true);
    }
    IEnumerator fallWater()
    {
        yield return delay;
        player.currentHealth -= 10;
        bubble.SetActive(false);
        SoundManager.instance.Effect_paly(13);
        bubble.SetActive(true);
        yield return delay2;
        InWaterSound();
        StartCoroutine(fallWater());
        StartCoroutine(DeactivateImages());
    }

    IEnumerator DeactivateImages()
    {
        if(bubble == true)
        {
            foreach (var image in hit)
            {
                image.gameObject.SetActive(true);

            }
            for (float timer = 10f; timer > 0; timer--)
            {
                yield return delay2; // 1초 대기

                // 매 초마다 이미지 하나 비활성화
                if ((int)(10f - timer) < hit.Length)
                {
                    hit[(int)(10f - timer)].gameObject.SetActive(false);
                }
            }
            if (AreAllImagesInactive())
            {
                ResetImages(); // 모든 이미지 다시 활성화
            }
        }

    }

    private bool AreAllImagesInactive()
    {
        foreach (var image in hit)
        {
            if (image.gameObject.activeSelf)
            {
                return false; // 하나라도 활성화되어 있으면 false 반환
            }
        }
        return true; // 모든 이미지가 비활성화된 경우 true 반환
    }

    private void ResetImages()
    {
        foreach (var image in hit)
        {
            image.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(water))
        {
            RenderSettings.fogDensity = inRange;
            RenderSettings.fogColor = inColor;
            watermesh.enabled = false;
            SoundManager.instance.Effect_paly(13);
            bubble.SetActive(true);
            InWaterSound();
            StartCoroutine(fallWater());
            StartCoroutine(DeactivateImages());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(water))
        {
            gm.inwaterNow = true;
            gm.deepwater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(water))
        {
            RenderSettings.fogDensity = outRange;
            RenderSettings.fogColor = outColor;
            watermesh.enabled = true;
            bubble.SetActive(false);
            SoundManager.instance.Effect_paly(14);
            gm.inwaterNow = false;
            gm.deepwater = false;
            StopAllCoroutines();
            ResetImages();
        }
    }
}
