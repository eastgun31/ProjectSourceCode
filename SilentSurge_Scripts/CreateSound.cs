using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;

public class CreateSound : MonoBehaviour
{
    public GameObject soundprefab;
    public GameObject effect;
    public int value;
    CoolTime delaytime = new CoolTime();
    new SphereCollider collider;
    public AudioSource audioSource;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        //if(value == 0)
        //{
        //    StartCoroutine(SoundCreateDelete());
        //}
    }

    public IEnumerator SoundCreateDeleteGun()
    {
        //yield return delaytime.colsize;
        //audioSource.Play();
        GameObject sound = Instantiate(soundprefab);
        sound.transform.position = gameObject.transform.position;
        collider = sound.GetComponent<SphereCollider>();
        collider.radius = 0.1f;
        yield return delaytime.cool1sec;
        //collider.radius = 0.5f;
        //yield return delaytime.coolhalf1sec;
        //collider.radius = 1f;
        //yield return delaytime.coolhalf1sec;
        //collider.radius = 1.5f;
        //yield return delaytime.coolhalf1sec;
        //collider.radius = 2f;
        //yield return delaytime.coolhalf1sec;
        collider.radius = 2.5f;
        yield return delaytime.coolhalf1sec;
        collider.radius = 5f;

        Destroy(sound, 1f);
    }


    IEnumerator SoundCreateDelete()
    {
        //yield return delaytime.colsize;
        GameObject sound = Instantiate(soundprefab);
        sound.transform.position = gameObject.transform.position;
        collider = sound.GetComponent<SphereCollider>();
        collider.radius = 0.1f;
        yield return delaytime.cool1sec;
        //collider.radius = 0.5f;
        //yield return delaytime.coolhalf1sec;
        collider.radius = 1f;
        //yield return delaytime.coolhalf1sec;
        //collider.radius = 1.5f;
        //yield return delaytime.coolhalf1sec;
        //collider.radius = 2f;
        //yield return delaytime.coolhalf1sec;
        //collider.radius = 2.5f;
        yield return delaytime.coolhalf1sec;
        collider.radius = 3f;

        Destroy(sound,1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(value == 1)
        {
            audioSource.Play();
            StartCoroutine (SoundCreateDelete());
        }
        if(value == 2 && GameManager.instance.onecollison)
        {
            GameManager.instance.onecollison = false;
            GameObject effectprefab = Instantiate(effect);
            effectprefab.transform.position = gameObject.transform.position;
            GameObject flash = Instantiate(soundprefab);
            flash.transform.position = gameObject.transform.position;
            Destroy(flash, 1f);
            Destroy(effectprefab, 1f);
        }
    }
}
