using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public static CombatController obj;
    public CombatState state;
    public Enemy selectedEnemy;
    private Enemy[] enemies;

    void Awake(){
        obj = this;
    }

    void Start(){
        state = CombatState.PlayerTurn;
        enemies = FindObjectsOfType<Enemy>();
    }

    public void SelectEnemy(Enemy enemy){
        if(state != CombatState.PlayerTurn){
            return;
        }

        if(selectedEnemy != null){
            selectedEnemy.Select(false);
        }

        selectedEnemy = enemy;
        selectedEnemy.Select(true);
    }

    public void PlayerAttack(){
        StartCoroutine(PlayerAttackCoroutine());
    }

    IEnumerator PlayerAttackCoroutine(){
        if(state != CombatState.PlayerTurn){
            yield break;
        }

        if(selectedEnemy != null){
            state = CombatState.Busy;
            yield return StartCoroutine(Player.obj.AttackCoroutine());
            StartCoroutine(EnemyTurn());
        }
    }


    IEnumerator EnemyTurn(){
        
        yield return new WaitForSeconds(1f);

        state = CombatState.EnemyTurn;
        foreach(Enemy enemy in FindObjectsOfType<Enemy>()){
            if(enemy != null){
                yield return StartCoroutine(enemy.AttackCoroutine(Player.obj));
                yield return new WaitForSeconds(1.2f);
            }
        }
        state = CombatState.PlayerTurn;
    }


}