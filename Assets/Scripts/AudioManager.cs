using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SoundType
{
    BG,
    OnClick,
}



public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource BgSource, SoundSource;
    public Sound[] sounds;
    private Dictionary<SoundType, Sound> soundDictionary = new Dictionary<SoundType, Sound>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeSounds();   
    }





    private void InitializeSounds()
    {
        foreach (var sound in sounds)
        {
            soundDictionary[sound.type] = sound;
        }
    }

    public void PlaySound(SoundType soundType)
    {
        if (soundDictionary.ContainsKey(soundType))
        {
            Sound sound = soundDictionary[soundType];
            SoundSource.clip = sound.clip;
            SoundSource.pitch = sound.pitch;
            SoundSource.volume = sound.volume;
            SoundSource.Play();
        }
    }







}



[System.Serializable]
public class Sound
{
    public string name;
    public SoundType type;
    public AudioClip clip;
    public float volume = 1f;
    public float pitch = 1f;
}

