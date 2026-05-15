using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] public GameObject buttonAtack;
    [SerializeField] public GameObject backGroundAttack;
    [SerializeField] private float spacingY = 2f;
    [SerializeField] private float buttonOffsetX;
    [SerializeField] private float buttonOffsetY;
    [SerializeField] private Vector2 spawnOrigin = new Vector2(4f, 0f);
    public static CombatController obj;
    public CombatState state;
    public int enemyCount = 3;
    public Enemy selectedEnemy;
    private Enemy[] enemies;

    void Awake(){
        obj = this;
    }

    void Start(){
        enemyCount = CombatData.enemyCount;
        enemyCount = 3;
        SpawnEnemies();
        state = CombatState.PlayerTurn;
        enemies = FindObjectsOfType<Enemy>();
    }

    void SpawnEnemies(){
        List<Vector2> positions = GetSpawnPositions(enemyCount);
        foreach(Vector2 pos in positions){
            Instantiate(enemyPrefab, pos, Quaternion.identity);
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
        backGroundAttack.SetActive(true);
        StartCoroutine(PlayerAttackCoroutine());
        backGroundAttack.SetActive(false);
    }

    IEnumerator PlayerAttackCoroutine(){
        if(state != CombatState.PlayerTurn){
            yield break;
        }

        //Entra cuando hay un enemigo seleccionado
        if(selectedEnemy != null){
            state = CombatState.Busy;
            //Se invoca la función de atacar
            yield return StartCoroutine(Player.obj.AttackCoroutine());
            StartCoroutine(EnemyTurn());
        }
    }

    //Acciones del enemigo durante su turno
    IEnumerator EnemyTurn(){
        buttonAtack.SetActive(false);
        Player.obj.select.SetActive(false); // apaga flecha del jugador

        yield return new WaitForSeconds(1f);
        state = CombatState.EnemyTurn;

        foreach(Enemy enemy in FindObjectsOfType<Enemy>()){
            if(enemy != null){
                enemy.SetTurnIndicator(true); // flecha del enemigo que va a atacar
                backGroundAttack.SetActive(true);
                StartCoroutine(CameraShake.obj.Shake());

                //Empieza animación de enemigo ataca a jugador
                Player.obj.StartAnimation("enemyAttack");
                FakeEnemy.obj.StartAnimation("enemyAttack");

                yield return StartCoroutine(enemy.AttackCoroutine(Player.obj));
                yield return new WaitForSeconds(0.4f);

                FakeEnemy.obj.StopAnimation("enemyAttack");
                Player.obj.StopAnimation("enemyAttack");
                
                backGroundAttack.SetActive(false);
                enemy.SetTurnIndicator(false); // apaga al terminar
                yield return new WaitForSeconds(1.2f);
            }
        }

        Player.obj.select.SetActive(true); // devuelve flecha al jugador
        state = CombatState.PlayerTurn;
    }

}
