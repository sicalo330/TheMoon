using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }

        if(audioSource == null){
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void ExecuteSound(AudioClip sound){
        audioSource.PlayOneShot(sound);
    }
}