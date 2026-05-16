using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;
    private AudioSource importantAudioSource;

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        AudioSource[] sources = GetComponents<AudioSource>();
        audioSource = sources[0];
        importantAudioSource = sources.Length > 1 ? sources[1] : gameObject.AddComponent<AudioSource>();
    }

    public void ExecuteSound(AudioClip sound){
        audioSource.PlayOneShot(sound);
    }

    public void ExecuteImportantSound(AudioClip sound){
        importantAudioSource.PlayOneShot(sound);
    }
}