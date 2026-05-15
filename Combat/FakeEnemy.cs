using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeEnemy : MonoBehaviour
{
    public static FakeEnemy obj;
    public Animator animator;

    void Awake(){
        obj = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetParameter(string parameter){
        animator.SetBool(parameter, true);
    }

    public void OutParameter(string parameter){
        animator.SetBool(parameter, false);
    }
}
