using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    //Enemy hará dañoa Player, recordar que este último existe en las escenas para los combates
    public void Attack(Player player){
        player.TakeDamage(attack);
    }

    //Todos los enemigos tienen que detectar si el jugador les hizo click
    private void OnMouseDown(){
        //Cuando se haga click, el controlador de combate seleccionará al enemigo
        CombatController.obj.SelectEnemy(this);
    }

    public IEnumerator AttackCoroutine(Player player){
        StartCoroutine(ShowText("Atacará", 0.5f));

        //La ventana de Parry se abre
        yield return new WaitForSeconds(0.2f);

        player.canParry = true;
        player.parrySuccess = false;

        yield return new WaitForSeconds(0.25f);
        //La ventana de Parry se cierra

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
