using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AnimationClip finalAnimation;

    // Start is called before the first frame update
    void Start(){
        animator = GetComponent<Animator>();
    }

    public void LoadScene(string escene){
        StartCoroutine(ChangeScene(escene));
    }

    IEnumerator ChangeScene(string escene){
        animator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(finalAnimation.length);

        SceneManager.LoadScene(escene);

    }
}