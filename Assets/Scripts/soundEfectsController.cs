using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class soundEfectsController : MonoBehaviour
{
    public static soundEfectsController instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void playSoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //Assign the audioClip

        audioSource.clip = audioClip;

        //Asign volume
        audioSource.volume = volume;
        //play sound

        audioSource.Play();

        //Get length of sound FX Clip

        float clipLength = audioSource.clip.length;

        //Destroy the clip after it is done playing

        Destroy(audioSource.gameObject, clipLength);



    }
}
