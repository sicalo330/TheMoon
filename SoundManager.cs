using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource importantAudioSource;
    [SerializeField] private AudioSource musicSource;

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

    musicSource.loop = true;
    musicSource.volume = 0.3f;
    }

    public void ExecuteSound(AudioClip sound){
        audioSource.PlayOneShot(sound);
    }

    public void ExecuteImportantSound(AudioClip sound){
        importantAudioSource.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music){
        musicSource.clip = music;
        musicSource.Play();
    }

    public void StopMusic(){
        musicSource.Stop();
    }

    public bool IsMusicPlaying(){
        return musicSource.isPlaying;
    }
}