using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll){
        if(coll.CompareTag("Player")){
            Debug.Log("buff");
        }
    }
}
