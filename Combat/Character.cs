using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]public GameObject select;
    [SerializeField]public int hp;
    //[SerializeField]public int maxHp;
    [SerializeField]public int attack;
    [SerializeField]public bool isPlayer;
    [SerializeField] public TMP_Text stateText;
    [SerializeField] protected TMP_Text lifeText;

    void Start(){
        lifeText.text = hp.ToString();
    }

    public void SetTurnIndicator(bool active){
        select.SetActive(active);
    }

    //TakeDamage no es atacar a alguien, es solo recibir daño, el ataque lo debe personalizar Player y los enemigos
    public virtual void TakeDamage(int damage){
        hp -= damage;
        lifeText.text = hp.ToString();

        StartCoroutine(TakeDamageAnimation());

        if(Player.obj.hp <= 0){
            SceneManager.LoadScene("Menu");
            StopAllCoroutines();
            return;
        }

        if(hp <= 0){
            Player.obj.OutParameter("playerAttack");
            FakeEnemy.obj.OutParameter("playerAttack");
            Player.obj.enemyDied = true;
            CombatController.obj.enemyAlive--;
            CombatController.obj.selectedEnemy = null;
            CombatController.obj.backGroundAttack.SetActive(false);//Tengo que poner esto sí o sí
            Die();
        }
    }

    public virtual void Die(){
        if(CombatController.obj.selectedEnemy == this){
            CombatController.obj.selectedEnemy = null;
        }
        Destroy(gameObject);
    }

    public IEnumerator Wait(){
        yield return new WaitForSecondsRealtime(0.6f);
        Player.obj.OutParameter("playerAttack");
        FakeEnemy.obj.OutParameter("playerAttack");
        Player.obj.OutParameter("machetazo");
        Player.obj.enemyDied = true; // avisa que murió
        CombatController.obj.buttonAtack.SetActive(false);
        CombatController.obj.enemyAlive--;
        Die();
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
}