using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject playerBuffer;
    [SerializeField] private float spawnTime;
    [SerializeField] private float lifetime;
    private float timer = 0f;
    Player player;
    void Start(){
        player = FindObjectOfType<Player>();
    }
    void Update(){
        timer += Time.deltaTime;
        if(timer >= spawnTime){
            Vector3 randomSpawnPosition = new Vector3(Random.Range(player.currentPosition.x - 8f ,player.currentPosition.x + 8f),Random.Range(-4,-2),0f);
            GameObject gb = Instantiate(playerBuffer, randomSpawnPosition, Quaternion.identity);

            Destroy(gb, lifetime);
            timer = 0f;
        }
    }
    
}
