using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    //Enemy hará dañoa Player, recordar que este último existe en las escenas para los combates
    public void Attack(Player player){
        select.SetActive(false);
        player.TakeDamage(attack);
    }

    //Todos los enemigos tienen que detectar si el jugador les hizo click
    private void OnMouseDown(){
        //Cuando se haga click, el controlador de combate seleccionará al enemigo
        CombatController.obj.SelectEnemy(this);
    }

    //Este es la función que usa el enemigo para atacar al jugador
    public IEnumerator AttackCoroutine(Player player){
        player.parryAttempted = false;
        player.parrySuccess = false;
        player.canParry = false;

        yield return new WaitForSeconds(0.2f);
        FakeEnemy.obj.clickAdvice.SetActive(true);
        player.canParry = true;

        yield return new WaitForSeconds(0.25f);

        FakeEnemy.obj.clickAdvice.SetActive(false);
        player.canParry = false;

        if(player.parrySuccess){
            StartCoroutine(player.ShowText("Parry", 0.5f));
            TakeDamage(player.attack);
        }
        else{
            player.TakeDamage(attack);
        }
    }
}
