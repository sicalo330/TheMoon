using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    [SerializeField]private string scene;
    [SerializeField] private AudioClip menuMusic;

    void Start(){
        SoundManager.Instance.PlayMusic(menuMusic);
    }

    public void ChangeScene(string scene){
        SoundManager.Instance.StopMusic();
        SceneManager.LoadScene(scene);
    }

    public void GoOut(){
        Application.Quit();
    }

    public void ButtonLoadScene(string scene){
        SceneLoadManager.obj.LoadScene(scene);
    }
}
