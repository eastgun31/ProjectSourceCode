using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldSkinItem : MonoBehaviour
{
    public MeshRenderer mesh;
    string worldskin = "WorldSkin";
    WaitForSeconds during = new WaitForSeconds(10f);

    IEnumerator ObjectOff()
    {
        yield return during;

        mesh.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(worldskin))
        {
            mesh.enabled = true;
            StartCoroutine(ObjectOff());
        }
    }
}
