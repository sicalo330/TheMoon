using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerCombat : Character
{
    public static PlayerCombat obj;
    public string lastSpawnPoint = "Combat";

    void Awake(){
        if (obj == null){
            obj = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else{
            Destroy(gameObject);
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode){

        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No se encontraron SpawnPoints en esta escena.");
            return;
        }

        foreach (SpawnPoint sp in spawnPoints)
        {
            if (sp.spawnID == lastSpawnPoint)
            {
                transform.position = sp.transform.position; // Mueve al jugador
                return;
            }
        }

        Debug.LogWarning("No se encontró un SpawnPoint con el ID: " + lastSpawnPoint);
    }

    void OnDestroy(){
        SceneManager.sceneLoaded -= OnSceneLoaded; // Evitar duplicados
    }

    //Cuando el jugador hace click a un enemigo, se selecciona a ese "único" enemigo clickeado
    public void Attack(){
        if(CombatController.obj.selectedEnemy != null){
            CombatController.obj.selectedEnemy.TakeDamage(attack);
        }
    }
}
