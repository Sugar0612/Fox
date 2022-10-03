using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip jumpAudio, HurtAudio, cherryAudio,
        EnemdeathAudio;

    public AudioSource audioSource;

    private static SoundManager instance = null;

    public static SoundManager Get()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        if(instance == null)
        {
            Destroy(instance);
        }
    }

    public void Jump()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }
    
    public void Hurt()
    {
        audioSource.clip = HurtAudio;
        audioSource.Play();
    }

    public void Pick()
    {
        audioSource.clip = cherryAudio;
        audioSource.Play();
    }

    public void EnemDeath()
    {
        audioSource.clip = EnemdeathAudio;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
