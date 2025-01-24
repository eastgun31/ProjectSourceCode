using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxReset : MonoBehaviour
{
    public Material mat;
    void Start()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        GameManager.instance.itemreset += MatReset;
    }

    public void MatReset()
    {
        gameObject.GetComponent<Renderer>().material = mat;
    }
}
