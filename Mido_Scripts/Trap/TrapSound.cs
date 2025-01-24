using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSound : MonoBehaviour
{
    [SerializeField]
    private int soundID;

    public void PlayESound()
    {
        SoundManager.instance.Effect_paly(soundID);
    }

    public void PlayESoundLoop()
    {
        SoundManager.instance.Effect_paly(soundID,true);
    }

    public void StopESound()
    {
        SoundManager.instance.Effectsource.Stop();
    }
}
