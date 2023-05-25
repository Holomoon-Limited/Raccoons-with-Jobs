using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private static AudioPlayer instance;

    public static AudioPlayer Instance {get { return instance;}}
    
    [Header("Battle SFX")]
    [SerializeField] AudioClip battleClip;
    [SerializeField] [Range(0.0f, 1.0f)] float battleVolume = 1.0f;

    [Header("Deal SFX")]
    [SerializeField] AudioClip dealClip;
    [SerializeField] [Range(0.0f, 1.0f)] float dealVolume = 1.0f;

    [Header("Interact SFX")]
    [SerializeField] AudioClip interactClip;
    [SerializeField] [Range(0.0f,1.0f)] float InteractVolume = 1.0f;
    
    void Awake() 
    {
        ManageSingleton();    
    }

    void ManageSingleton()
    {
        if(instance != null && instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

   
   
    public void PlayBattleClip()
    {
        PlayClip(battleClip, battleVolume);
    }

    public void PlayDealClip()
    {
        PlayClip(dealClip, dealVolume);
    }

    public void PlayInteractClip()
    {
        PlayClip(interactClip, InteractVolume);
    }

    void PlayClip(AudioClip clip, float volume)
    {
       if(clip != null)
       {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
       }
       
    }

    

}

