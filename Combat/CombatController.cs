using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] public GameObject buttonAtack;
    [SerializeField] public GameObject buttonAtackGun;
    [SerializeField] public GameObject backGroundAttack;
    [SerializeField] private GameObject containerAdvice;
    [SerializeField] private TMP_Text textAdvice;
    [SerializeField] private float spacingY = 2f;
    [SerializeField] private float buttonOffsetX;
    [SerializeField] private float buttonOffsetY;
    [SerializeField] private Vector2 spawnOrigin = new Vector2(4f, 0f);
    public static CombatController obj;
    public CombatState state;
    public int enemyCount = 0;
    public int enemyAlive = 0;
    public Enemy selectedEnemy;
    private Enemy[] enemies;

    void Awake(){
        obj = this;
    }

    void Start(){
        //ESta variable es importante porque dicta cuántos enemigos van a haber en pantalla
        enemyCount = 1;
        SpawnEnemies();
        state = CombatState.PlayerTurn;
        enemies = FindObjectsOfType<Enemy>();
        enemyAlive = enemies.Length;
        PutGun();
    }

    void PutGun(){
        buttonAtackGun.transform.position = new Vector3(
            Player.obj.transform.position.x - buttonOffsetX,
            Player.obj.transform.position.y,
            0f
        );
        buttonAtackGun.SetActive(true);
    }

    void SpawnEnemies(){
        List<Vector2> positions = GetSpawnPositions(enemyCount);
        foreach(Vector2 pos in positions){
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }

    public void CheckWaveCompletion(){
        if(enemyAlive <= 0 && enemyCount < 4){
            enemyCount++;
            SpawnEnemies();
            enemyAlive = enemyCount;
            enemies = FindObjectsOfType<Enemy>();
            state = CombatState.PlayerTurn;
            Player.obj.select.SetActive(true);
            buttonAtack.SetActive(false);
        }
        else if(enemyAlive <= 0 && enemyCount >= 4){
            Debug.Log("¡Combate ganado!");
        }
    }

    List<Vector2> GetSpawnPositions(int count){
        List<Vector2> positions = new List<Vector2>();
        Vector2 center = spawnOrigin;
        float s = spacingY;

        switch(count){
            case 1:
                // Centro
                positions.Add(center);
                break;

            case 2:
                // Fila diagonal "/"
                positions.Add(center + new Vector2( 0.6f,  s * 0.7f));
                positions.Add(center + new Vector2(-0.6f, -s * 0.7f));
                break;

            case 3:
                // Triángulo "<|" punta a la izquierda
                positions.Add(center + new Vector2(-s * 0.8f,  0f));     // punta izquierda
                positions.Add(center + new Vector2( s * 0.8f,  s * 0.8f)); // arriba derecha
                positions.Add(center + new Vector2( s * 0.8f, -s * 0.8f)); // abajo derecha
                break;

            case 4:
                // Anillo
                positions.Add(center + new Vector2( 0f,  s * 0.9f)); // arriba
                positions.Add(center + new Vector2( 0f, -s * 0.9f)); // abajo
                positions.Add(center + new Vector2( s * 0.9f,  0f)); // derecha
                positions.Add(center + new Vector2(-s * 0.9f,  0f)); // izquierda
                break;
        }

        return positions;
    }

    //Función para selccionar un enemigo
    public void SelectEnemy(Enemy enemy){
        if(state != CombatState.PlayerTurn) return;

        selectedEnemy = enemy;

        buttonAtack.transform.position = new Vector3(
            enemy.transform.position.x - buttonOffsetX,
            enemy.transform.position.y,
            enemy.transform.position.z
        );
        buttonAtack.SetActive(true);
    }

    //Ataque del jugador durante su turno
    public void PlayerAttack(){
        StartCoroutine(PlayerAttackCoroutine());
    }

    IEnumerator PlayerAttackCoroutine(){
        if(state != CombatState.PlayerTurn) yield break;

        if(selectedEnemy != null){
            state = CombatState.Busy;
            yield return StartCoroutine(Player.obj.AttackCoroutine());

            if(enemyAlive <= 0){
                CheckWaveCompletion();
            }
            else{
                yield return StartCoroutine(EnemyTurn());
            }
        }
    }

    //Acciones del enemigo durante su turno
    IEnumerator EnemyTurn(){
        buttonAtack.SetActive(false);
        Player.obj.select.SetActive(false);

        yield return new WaitForSeconds(1f);
        state = CombatState.EnemyTurn;

        foreach(Enemy enemy in FindObjectsOfType<Enemy>()){
            if(!enemy) continue;

            textAdvice.text = "Venga papi que no e pa' eso";
            containerAdvice.SetActive(true);
            
            if(!enemy) continue;
            enemy.SetTurnIndicator(true);

            yield return new WaitForSeconds(1f);

            containerAdvice.SetActive(false);
            backGroundAttack.SetActive(true);
            StartCoroutine(CameraShake.obj.Shake());

            Player.obj.SetParameter("enemyAttack");
            FakeEnemy.obj.SetParameter("enemyAttack");

            yield return StartCoroutine(enemy.AttackCoroutine(Player.obj));
            yield return new WaitForSeconds(0.4f);

            FakeEnemy.obj.OutParameter("enemyAttack");
            Player.obj.OutParameter("enemyAttack");
            backGroundAttack.SetActive(false);
            
            //ESte if es para ver si el enemigo sigue vivo después de un parry
            if(enemy){
                enemy.SetTurnIndicator(false);
            }

            yield return new WaitForSeconds(1.2f);
        }

        //La línea de abajo indica turno del jugador
        Player.obj.select.SetActive(true);
        buttonAtackGun.SetActive(true);
        Player.obj.stateText.text = "";
        state = CombatState.PlayerTurn;
        
        CheckWaveCompletion();
    }

    public void PlayerGunAttack(){
        StartCoroutine(PlayerGunAttackCoroutine());
    }

    IEnumerator PlayerGunAttackCoroutine(){
        if(state != CombatState.PlayerTurn) yield break;

        state = CombatState.Busy;
        buttonAtackGun.SetActive(false);
        buttonAtack.SetActive(false);

        yield return StartCoroutine(Player.obj.GunAttackCoroutine());

        if(enemyAlive <= 0){
            CheckWaveCompletion();
        } else {
            yield return StartCoroutine(EnemyTurn());
        }
    }

}
