using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class Player : Character
{
    public static Player obj;
    [SerializeField]private float maxSpeed = 10f;
    [SerializeField]private float minY;
    [SerializeField]public bool isCombat = false;
    [SerializeField]public bool canParry;
    [SerializeField]public bool parrySuccess;
    public bool canDoubleAttack = false;
    public bool doubleAttackSuccess;
    private bool doubleAttackAttempted;
    public bool parryAttempted;
    private Camera mainCamera;
    public string lastSpawnPoint = "MainGame";
    private float lastClickTime;
    private float doubleAttackWindowStart;
    private float doubleAttackWindowEnd;

    void Start(){
        mainCamera = Camera.main;
        //basePosition = transform.position;

        //Estos son atributos de la clase Character
        if(isCombat){
            lifeText.text = hp.ToString();
        }
    }

    void Update(){        
        //Ataque y doble ataque 
        if(Input.GetMouseButtonDown(0) && isCombat){
            if(isCombat){
                TryParry();
                TryDoubleAttack();
            }
        }
        
        //Movimiento del personaje
        if (Input.GetMouseButton(0) && !isCombat){
            FollowMousePositionDelayed(maxSpeed);
        }
    }

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

    private void FollowMousePositionDelayed(float maxSpeed){
        Vector2 targetPos = GetWorldPositionFromMouse();
        //targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        targetPos.y = transform.position.y;
        transform.position = Vector2.MoveTowards(transform.position,targetPos,maxSpeed * Time.deltaTime);
    }

    private Vector2 GetWorldPositionFromMouse(){
        //Cuando haya un cambio de escena, hay que volver a buscar la camara
        if (mainCamera == null){   
            mainCamera = Camera.main;
        }

        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
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


    //------------------Escena combate----------------------

    public void Attack(){
            if(CombatController.obj.selectedEnemy != null){
                CombatController.obj.selectedEnemy.TakeDamage(attack);
            }
        }

    public IEnumerator AttackCoroutine(){

        Enemy enemy = CombatController.obj.selectedEnemy;

        if(enemy == null){
            yield break;
        }

        doubleAttackSuccess = false;
        doubleAttackAttempted = false;

        Attack();

        yield return new WaitForSeconds(0.5f);

        canDoubleAttack = true;

        doubleAttackWindowStart = Time.time;
        doubleAttackWindowEnd = Time.time + 0.4f;

        stateText.text = "Otra vez";

        yield return new WaitForSeconds(0.4f);

        canDoubleAttack = false;

        stateText.text = "";

        if(doubleAttackSuccess){
            yield return StartCoroutine(ShowText("Double", 0.5f));

            if(enemy != null){
                enemy.TakeDamage(attack);
            }
            yield break;
        }
    }
    
    public void TryParry(){
        if(parryAttempted)
            return;

        parryAttempted = true;

        if(!canParry)
            return;

        parrySuccess = true;
    }

    public void TryDoubleAttack(){
        if(doubleAttackAttempted)
            return;

        doubleAttackAttempted = true;

        if(!canDoubleAttack)
            return;

        float clickTime = Time.time;

        if(clickTime >= doubleAttackWindowStart && clickTime <= doubleAttackWindowEnd){
            doubleAttackSuccess = true;

            canDoubleAttack = false;
        }
    }

    IEnumerator DoubleAttackCooldown(float time){
        yield return new WaitForSeconds(time);
    }

}
