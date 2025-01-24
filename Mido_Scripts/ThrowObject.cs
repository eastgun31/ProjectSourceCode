using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    string player = "Player";
    string wall = "Wall";

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(player) || other.CompareTag(wall))
            Destroy(gameObject);
    }
}
