using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SpawnCombat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")){
            Player.obj.isCombat = true;
            Player.obj.lastSpawnPoint = "Combat";
            SceneManager.LoadScene("Combat");
        }
    }
    
}
