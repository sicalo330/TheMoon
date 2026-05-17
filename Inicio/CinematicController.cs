using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicController : MonoBehaviour
{
    public void GoToCombat(){
        SceneManager.LoadScene("Combat");
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            SceneManager.LoadScene("Combat");
        }
    }
}