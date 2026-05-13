using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    //Enemy hará dañoa PlayerCombat, recordar que este último existe en las escenas para los combates
    public void Attack(PlayerCombat player){
        player.TakeDamage(attack);
    }

    //Todos los enemigos tienen que detectar si el jugador les hizo click
    private void OnMouseDown(){
        //Cuando se haga click, el controlador de combate seleccionará al enemigo
        CombatController.obj.SelectEnemy(this);
    }
}
