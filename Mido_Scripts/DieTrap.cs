using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DieTrap : MonoBehaviour
{
    public GameObject dieUI;
    public UnityEvent dieEvent;

    string player = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(player))
        {
            dieEvent.Invoke();
            dieUI.SetActive(true);
        }
            
    }
}
