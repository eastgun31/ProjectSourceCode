using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamController : MonoBehaviour
{
    [SerializeField]
    private Transform playerPos;
    Vector3 yPos;
    Vector3 yzPos;

    [SerializeField]
    private int value;

    private void Awake()
    {
        yPos = new Vector3 (0, 10, 0);
        yzPos = new Vector3 (0, 30, 1);

        if(value == 1)
            transform.position = playerPos.position + yPos;
        else
            transform.position = playerPos.position + yzPos;
    }

    private void OnEnable()
    {
        if (value == 1)
            transform.position = playerPos.position + yPos;
        else
            transform.position = playerPos.position + yzPos;
    }

    void LateUpdate()
    {
        if (value == 1)
            transform.position = playerPos.position + yPos;
        else
            transform.position = playerPos.position + yzPos;
    }
}
