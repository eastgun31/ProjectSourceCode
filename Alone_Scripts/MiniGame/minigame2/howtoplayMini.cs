using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class howtoplayMini : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Delay", 3f);
    }

    void Delay()
    {
        gameObject.SetActive(false);
    }
}
