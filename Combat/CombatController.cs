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
    [SerializeField] private Vector2 spawnOrigin = new Vector2(2f, -2f);
    [SerializeField] private AudioClip audioDamage;
    [SerializeField] private AudioClip audioTurn;
    [SerializeField] private PowerUpPanel powerUpPanel;
    public static CombatController obj;
    public CombatState state;
    public int enemyCount = 0;
    public int enemyAlive = 0;
    public Enemy selectedEnemy;
    private Enemy[] enemies;
    public List<string> ñeroText = new List<string>();
    private DialogueList dialogueList; 

    void Awake(){
        obj = this;
    }

    void Start(){
        LoadDialogues();
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
            Player.obj.transform.position.x + buttonOffsetX,
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
            powerUpPanel.Show();
        }
        else if(enemyAlive <= 0 && enemyCount >= 4){
            Debug.Log("¡Combate ganado!");
        }
    }

    public void StartNextWave(){
        enemyCount++;
        SpawnEnemies();
        enemyAlive = enemyCount;
        enemies = FindObjectsOfType<Enemy>();

        foreach(Enemy enemy in enemies){
            enemy.hp += enemyCount * 2;
            enemy.attack += enemyCount + 1;
            enemy.lifeText.text = enemy.hp.ToString();
        }

        state = CombatState.PlayerTurn;
        Player.obj.select.SetActive(true);
        buttonAtack.SetActive(false);
        buttonAtackGun.SetActive(true);
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
                positions.Add(center + new Vector2( 1f,  s * 0.5f));
                positions.Add(center + new Vector2(-1f, -s * 0.5f));
                break;

            case 3:
                // Triángulo "<|" punta a la izquierda
                positions.Add(center + new Vector2(-s * 0.8f,  0f));     // punta izquierda
                positions.Add(center + new Vector2( s * 0.8f,  s * 0.6f)); // arriba derecha
                positions.Add(center + new Vector2( s * 2.6f, 0f)); // abajo derecha
                break;

            case 4:
                // Anillo
                positions.Add(center + new Vector2( 1.5f,  s * 0.8f)); // arriba
                positions.Add(center + new Vector2( 1f, -s * 0.3f)); // abajo
                positions.Add(center + new Vector2( s * 2f,  0.3f)); // derecha
                positions.Add(center + new Vector2(-s * 0.9f,  0.3f)); // izquierda
                break;
        }

        return positions;
    }

    //Función para selccionar un enemigo
    public void SelectEnemy(Enemy enemy){
        if(state != CombatState.PlayerTurn) return;

        selectedEnemy = enemy;

        buttonAtack.transform.position = new Vector3(
            enemy.transform.position.x + buttonOffsetX,
            enemy.transform.position.y,
            enemy.transform.position.z
        );
        buttonAtack.SetActive(true);
    }

    //Ataque del jugador durante su turno
    public void PlayerAttack(){
        SoundManager.Instance.ExecuteSound(audioDamage);
        StartCoroutine(PlayerAttackCoroutine());
    }

    IEnumerator PlayerAttackCoroutine(){
        if(state != CombatState.PlayerTurn) yield break;

        if(selectedEnemy != null){
            state = CombatState.Busy;
            StartCoroutine(CameraShake.obj.Shake());
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

            SoundManager.Instance.ExecuteImportantSound(audioTurn);
            textAdvice.text = GetRandomDialogue("ñero");
            containerAdvice.SetActive(true);
            
            if(!enemy) continue;
            enemy.SetTurnIndicator(true);

            yield return new WaitForSeconds(1f);

            containerAdvice.SetActive(false);
            backGroundAttack.SetActive(true);
            StartCoroutine(CameraShake.obj.Shake());

            Player.obj.SetParameter("enemyAttack");
            FakeEnemy.obj.SetParameter("enemyAttack");
            SoundManager.Instance.ExecuteSound(audioDamage);

            //En sí este es el ataque del enemigo
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
        SoundManager.Instance.ExecuteImportantSound(audioTurn);
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

        yield return new WaitForSeconds(0.9f);

        if(enemyAlive <= 0){
            CheckWaveCompletion();
        } else {
            yield return StartCoroutine(EnemyTurn());
        }
    }

    /*
    
    void LoadDialogues(){
        TextAsset jsonFile = Resources.Load<TextAsset>("Dialogue");
        
        if (jsonFile == null){
            Debug.LogError("No se pudo cargar el archivo JSON. Verifica la ruta en Resources."); 
            return;
        }

        dialogueList = JsonUtility.FromJson<DialogueList>(jsonFile.text);
        
        if (dialogueList == null || dialogueList.dialogues == null){
            Debug.LogError("El JSON se cargó, pero no tiene diálogos válidos.");
        }

        
        foreach(Dialogue dialogue in dialogueList.dialogues){
            Debug.Log("ID: " + dialogue.id);
            foreach(string line in dialogue.lines)
            {
                Debug.Log(line);
            }
        }
    }
    
    a
    */

        void LoadDialogues(){
            TextAsset jsonFile = Resources.Load<TextAsset>("Dialogue"); //va a la carpeta en donde se encuentran los dialogos
            
            if (jsonFile == null){
                Debug.LogError("No se pudo cargar el archivo JSON. Verifica la ruta en Resources."); //Si el json no se encuentra en la localidad entonces  sacará esto
                return;
            }

            dialogueList = JsonUtility.FromJson<DialogueList>(jsonFile.text);//Si lo anteriorr no sucede, es porque si existe el json y lo convertirá en una lista(creo)

            if (dialogueList == null || dialogueList.dialogues == null){
                Debug.LogError("El JSON se cargó, pero no tiene diálogos válidos."); //Si el json existe pero no tiene contenido pasará esto
            }
        }



    string GetRandomDialogue(string dialogueId){
        if(dialogueList == null || dialogueList.dialogues == null){
            Debug.LogError("DialogueList no fue cargado");
            return "...";
        }

        foreach(DialogueData dialogue in dialogueList.dialogues){
            if(dialogue.id == dialogueId){
                int randomIndex = Random.Range(0, dialogue.lines.Length);
                return dialogue.lines[randomIndex];
            }
        }

        return "...";
    }

}
