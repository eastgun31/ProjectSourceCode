using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterSpawn : MonoBehaviour
{
    public GameObject[] water;

    private void OnEnable()
    {
        StartCoroutine(waterOnOff());
    }


    IEnumerator waterOnOff()
    {
        int random = Random.Range(0, water.Length);

        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

        water[random].SetActive(true);

        yield return waitForSeconds;

        water[random].SetActive(false);

        yield return waitForSeconds;

        water[random].SetActive(false);

        StartCoroutine(waterOnOff());
    }
}
