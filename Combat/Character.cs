using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject select;
    [SerializeField]public int hp;
    [SerializeField]public int maxHp;
    [SerializeField]public int attack;

    public void Select(bool select){
        this.select.SetActive(select);
    }

    public virtual void TakeDamage(int damage){
        hp -= damage;

        Debug.Log(name + " recibió " + damage);

        if(hp <= 0)
        {
            Die();
        }
    }

    public virtual void Die(){
        Destroy(gameObject);
    }
}