using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMoon : MonoBehaviour
{
    public static FakeMoon obj;
    public Animator animator;
    [SerializeField]public GameObject clickAdvice;
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
