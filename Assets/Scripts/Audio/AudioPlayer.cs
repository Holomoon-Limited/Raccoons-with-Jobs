using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Battle SFX")]
    [SerializeField] AudioClip battleClip;
    [SerializeField] [Range(0.0f, 1.0f)] float battleVolume = 1.0f;

    static AudioPlayer instance;
    
    void Awake() 
    {
        ManageSingleton();    
    }

    void ManageSingleton()
    {
        if(instance != null)
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

   
   
    public void playBattleClip()
    {
        PlayClip(battleClip, battleVolume);
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

