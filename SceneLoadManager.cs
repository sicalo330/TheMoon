using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager obj;
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private float fadeDuration = 0.5f;

    void Awake(){
        if(obj == null){
            obj = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    void Start(){
        StartCoroutine(FadeIn());
    }

    public void LoadScene(string scene){
        SoundManager.Instance.StopMusic();
        StartCoroutine(FadeOutAndLoad(scene));
    }

    IEnumerator FadeIn(){
        panel.alpha = 1f;
        float elapsed = 0f;
        while(elapsed < fadeDuration){
            elapsed += Time.deltaTime;
            panel.alpha = 1f - (elapsed / fadeDuration);
            yield return null;
        }
        panel.alpha = 0f;
        panel.blocksRaycasts = false;
    }

    IEnumerator FadeOutAndLoad(string scene){
        panel.blocksRaycasts = true;
        float elapsed = 0f;
        while(elapsed < fadeDuration){
            elapsed += Time.deltaTime;
            panel.alpha = elapsed / fadeDuration;
            yield return null;
        }
        panel.alpha = 1f;
        SceneManager.LoadScene(scene);
        yield return null; // espera un frame
        StartCoroutine(FadeIn()); // ← fade in en la nueva escena
    }
}