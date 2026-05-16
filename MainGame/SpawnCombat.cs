using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SpawnCombat : MonoBehaviour
{
    [SerializeField] private int enemyCount = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")){
            Player player = other.GetComponent<Player>();
            if(player == null) return;

            //Solo hace que el texto de la vida aparezca cuando cambia de juego a combate
            CombatData.enemyCount = enemyCount;
            //player.isCombat = true;
            player.lastSpawnPoint = "Combat";
            SceneManager.LoadScene("Combat");
        }
    }
}