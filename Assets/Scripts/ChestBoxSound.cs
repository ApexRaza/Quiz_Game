using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBoxSound : MonoBehaviour
{

    public AudioSource source;




    public void PlayAudio(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

}
