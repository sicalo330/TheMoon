using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicController : MonoBehaviour
{
    public void GoToCombat(){
        SceneManager.LoadScene("Combat");
    }
}