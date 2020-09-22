using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance = null;
    
    public static AudioManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if(_instance == null)
                {
                    GameObject go = new GameObject("Audio Manager");
                    _instance = go.AddComponent<AudioManager>();
                }
            }
            return _instance;
        }
    }

    // Configuration parameters
    AudioSource myAudioSource;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(AudioClip clipToPlay)
    {
        myAudioSource.PlayOneShot(clipToPlay);
    }

    public void PlayAudioClip(AudioClip clipToPlay, float volume)
    {
        myAudioSource.PlayOneShot(clipToPlay, volume);
    }

    public bool GetIsPlaying()
    {
        return myAudioSource.isPlaying;
    }

    public void StopAudio()
    {
        myAudioSource.Stop();
    }
}
