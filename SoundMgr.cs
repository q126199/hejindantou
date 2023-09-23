using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    private static SoundMgr instance;

    public static SoundMgr Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void PlayAudion(AudioClip ac)
    {
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
        audioSource.playOnAwake = false;
    }

    public void PlayMusicByName(string name)
    {
        string path = "Sounds/" + name;
        AudioClip clip = Resources.Load<AudioClip>(path);
        PlayAudion(clip);
    }
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
