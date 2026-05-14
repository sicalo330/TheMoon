using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("WaitToEnd",16);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            SceneManager.LoadScene("Menu");
        }
    }

    public void WaitToEnd(){
        SceneManager.LoadScene("Menu");
    }
}
