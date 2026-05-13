using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    [SerializeField]private string scene;
    public void ChangeScene(string scene){
        SceneManager.LoadScene(scene);
    }

    public void GoOut(){
        Debug.Log("Jugador sale");
    }
}
