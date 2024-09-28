using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageRange : MonoBehaviour
{
    Hostage hostage;
    Player player;

    void Start()
    {
        hostage = transform.parent.GetComponent<Hostage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            hostage.target = player.transform;
        }
    }
}
