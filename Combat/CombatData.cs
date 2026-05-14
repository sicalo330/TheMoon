using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatData
{  /*En realidad no quería creer esta clase tan vacía, pero bueno, intenté poner enemyCount
    directamente a CombatController, pero como hacía parte de otra escena o porque aún no había
    sido creada o no sé por qué, me tocó hacer esta clase como puente para mandar la cantidad de
    enemigos que el juego tiene que generar en los combates por turnos
    */
    public static int enemyCount;
}
