using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : Character
{
    public static Player obj;
    [SerializeField]private float maxSpeed = 10f;
    [SerializeField]private float minY;
    [SerializeField]private float maxY;
    [SerializeField]public bool isCombat = false;
    private Camera mainCamera;
    public Vector2 currentPosition;
    private Vector2 basePosition;
    private Vector2 targetPos;
    public string lastSpawnPoint = "MainGame";

    void Start(){
        mainCamera = Camera.main;
        //basePosition = transform.position;
    }

    void Update(){

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



}
