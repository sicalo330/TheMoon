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

    public void StartAnimation(){
        animator.SetBool("playerAttack", true);
    }

    public void StopAnimation(){
        animator.SetBool("playerAttack", false);
    }
}
