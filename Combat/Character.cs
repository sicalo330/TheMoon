using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject select;
    [SerializeField]public int hp;
    //[SerializeField]public int maxHp;
    [SerializeField]public int attack;
    [SerializeField]public bool isPlayer;
    [SerializeField] protected TMP_Text stateText;
    [SerializeField] protected TMP_Text lifeText;

    void Start(){
        lifeText.text = hp.ToString();
    }

    public void Select(bool select){
        this.select.SetActive(select);
    }

    //TakeDamage no es atacar a alguien, es solo recibir daño, el ataque lo debe personalizar Player y los enemigos
    public virtual void TakeDamage(int damage){
        hp -= damage;
        lifeText.text = hp.ToString();
        StartCoroutine(TakeDamageAnimation());

        if(hp <= 0)
        {
            Die();
        }
    }

    public virtual void Die(){
        Destroy(gameObject);
    }

    IEnumerator TakeDamageAnimation(){
        stateText.text = "Impacto";
        float mov = 0.3f;
        if(isPlayer) mov *= -1;
        transform.position = new Vector3(transform.position.x + mov, transform.position.y, transform.position.z);
        yield return new WaitForSecondsRealtime(0.2f);
        transform.position = new Vector3(transform.position.x - mov, transform.position.y, transform.position.z);
        stateText.text = "";
    }

    public IEnumerator ShowText(string text, float duration){
        stateText.text = text;
        yield return new WaitForSeconds(duration);
        stateText.text = "";
    }



}