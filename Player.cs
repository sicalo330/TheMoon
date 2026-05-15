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
    public bool hitSuccess = false;
    private Camera mainCamera;
    public string lastSpawnPoint = "MainGame";
    private float lastClickTime;
    private float doubleAttackWindowStart;
    private float doubleAttackWindowEnd;
    public Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        if(isCombat){
            lifeText.text = hp.ToString();
            select.SetActive(true); // jugador comienza primero
        }
    }

    void Update(){        
        //Ataque y doble ataque 
        if(Input.GetMouseButtonDown(0) && isCombat){
                TryParry();
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
        //Comienza el ataque
        select.SetActive(false);
        lifeText.text = "";//Por alguna razon está cosa se ponía fea en las animaciones, entonces me tocó quitarlo
        StartAnimation("playerAttack");
        Enemy enemy = CombatController.obj.selectedEnemy;
        //Aquí empieza la animación del enemigo cuando el jugador ataca al enemigo
        FakeEnemy.obj.StartAnimation("playerAttack");
        if(enemy == null) yield break;

        StartCoroutine(CameraShake.obj.Shake());
        CombatController.obj.backGroundAttack.SetActive(true);
        Attack();

        yield return new WaitForSeconds(0.3f);

        CombatController.obj.backGroundAttack.SetActive(false);

        while(enemy != null){
            CombatController.obj.backGroundAttack.SetActive(true);
            // Resetea al inicio del tiempo muerto
            hitSuccess = false;
            doubleAttackAttempted = false;

            yield return new WaitForSeconds(0.5f); // tiempo muerto, clicks aquí marcan doubleAttackAttempted = true con canDoubleAttack = false

            canDoubleAttack = true;
            stateText.text = "Otra vez";

            yield return new WaitForSeconds(0.4f);

            canDoubleAttack = false;
            stateText.text = "";
            CombatController.obj.backGroundAttack.SetActive(false);

            if(hitSuccess){
                //animator.SetBool("again", true);
                animator.Play("Attack", 0, 0f);
                FakeEnemy.obj.animator.Play("FakeEnemyTakeDamage", 0, 0f);

                yield return StartCoroutine(ShowText("Hit!", 0.3f));
                CombatController.obj.backGroundAttack.SetActive(true);
                if(enemy != null) enemy.TakeDamage(attack);
                StartCoroutine(CameraShake.obj.Shake());
                CombatController.obj.backGroundAttack.SetActive(false);
                if(enemy == null) yield break;
            } else {//Se entra al else cuando falla el ataque consecutivo
                CombatController.obj.backGroundAttack.SetActive(false);
                //Hace la animación de regreso
                //animator.SetBool("again", false);
                StopAnimation("playerAttack");
                FakeEnemy.obj.StopAnimation("playerAttack");

                yield return new WaitForSeconds(0.2f);

                //Vuelve a idle
                //select.SetActive(true);//Devuelve la marca
                lifeText.text = hp.ToString();//Devuelve el texto de la vida
                yield break;
            }
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

        hitSuccess = true;
        canDoubleAttack = false;
    }

    IEnumerator DoubleAttackCooldown(float time){
        yield return new WaitForSeconds(time);
    }

    public void StartAnimation(string parameter){
        animator.SetBool(parameter, true);
    }

    public void StopAnimation(string parameter){
        animator.SetBool(parameter, false);
    }

}
