using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioPlayer;
    public AudioSource effectPlayer;
    public AudioSource enemyPlayer;

    public AudioClip[] bgmClips;
    public AudioClip[] effectClips;
    public AudioClip[] uiClips;
    public AudioClip[] enemyClips;

    public bool stage1Clear = false;
    public bool stage2Clear = false;

    [SerializeField]
    private bool playingSource = false;
    public bool playingAction = false;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        //volume = audioPlayer.volume;   
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출, 델리게이트체인
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(scene.buildIndex);
        bgmPlay(scene.buildIndex);
    }

    public void bgmPlay(int i)
    {
        effectPlayer.Stop();
        audioPlayer.Stop();

        if (i == 5 || i == 6)
            audioPlayer.volume = 0.07f;
        else
            audioPlayer.volume = 0.1f;

        audioPlayer.clip = bgmClips[i];
        audioPlayer.loop = true;
        audioPlayer.Play();
    }

    public void EffectPlay(int i, bool type, float vol)
    {
        if(type)
        {
            if(i == 0)
                effectPlayer.loop = true;
            else
                effectPlayer.loop = false;

            effectPlayer.volume = vol;
            effectPlayer.PlayOneShot(effectClips[i]);
        }
        else if(!type && !playingSource)
        {
            effectPlayer.volume = vol;
            playingSource = true;
            effectPlayer.clip = effectClips[i];
            effectPlayer.loop = true;
            effectPlayer.Play();
        }
    }
    public void EffectOff()
    {
        effectPlayer.Stop();
        effectPlayer.clip = null;
        playingSource = false;
    }
    public void EnemyEffect(int i,float vol)
    {
        enemyPlayer.volume = vol;
        enemyPlayer.PlayOneShot(enemyClips[i]);
    }
    public void UiSoundPlay(int i)
    {
        audioPlayer.PlayOneShot(uiClips[i]);
    }
    
}
