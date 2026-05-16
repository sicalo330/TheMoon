using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class Player : Character
{
    public static Player obj;
    //[SerializeField]private float maxSpeed = 10f;
    [SerializeField]private float minY;
    //[SerializeField]public bool isCombat = false;
    [SerializeField]public bool canParry;
    [SerializeField]public bool parrySuccess;
    [SerializeField]public GameObject clickAdvice;
    public bool canDoubleAttack = false;
    public bool doubleAttackSuccess;
    private bool doubleAttackAttempted;
    public bool parryAttempted;
    public bool hitSuccess = false;
    public bool enemyDied = false;
    public bool machetazo = false;
    public bool gun = false;
    private Camera mainCamera;
    public string lastSpawnPoint = "MainGame";
    private float lastClickTime;
    private float doubleAttackWindowStart;
    private float doubleAttackWindowEnd;
    public Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        lifeText.text = hp.ToString();
            select.SetActive(true); // jugador comienza primero
    }

    void Update(){        
        //Ataque
        if(Input.GetMouseButtonDown(0)){
                TryParry();
                TryDoubleAttack();
        }
        
        //Movimiento del personaje
        /*
        
        if (Input.GetMouseButton(0) && !isCombat){
            FollowMousePositionDelayed(maxSpeed);
        }
        */
    }

    void Awake(){
        if (obj == null){
            obj = this;
            //DontDestroyOnLoad(gameObject);
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else{
            Destroy(gameObject);
        }
    }

    /*
    private void FollowMousePositionDelayed(float maxSpeed){
        Vector2 targetPos = GetWorldPositionFromMouse();
        //targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        targetPos.y = transform.position.y;
        transform.position = Vector2.MoveTowards(transform.position,targetPos,maxSpeed * Time.deltaTime);
    }
    
    */

    private Vector2 GetWorldPositionFromMouse(){
        //Cuando haya un cambio de escena, hay que volver a buscar la camara
        if (mainCamera == null){   
            mainCamera = Camera.main;
        }

        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    /*
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
    
    
    */


    //------------------Escena combate----------------------

    public IEnumerator Attack(){
            if(CombatController.obj.selectedEnemy != null){
                yield return new WaitForSeconds(0.6f);
                //ESta línea le quita el hp al enemigo, es solo eta
                CombatController.obj.selectedEnemy.TakeDamage(attack);
            }
        }

    public IEnumerator AttackCoroutine(){
        //Comienza el ataque
        select.SetActive(false);
        lifeText.text = "";//Por alguna razon está cosa se ponía fea en las animaciones, entonces me tocó quitarlo

        //Player empieza su animación de ataque y enemigo de atacado
        if (gun){
            SetParameter("playerAttack");
        }

        if (machetazo){
            SetParameter("machetazo");
        }

        FakeEnemy.obj.SetParameter("playerAttack");

        Enemy enemy = CombatController.obj.selectedEnemy;
        //Aquí empieza la animación del enemigo cuando el jugador ataca al enemigo
        if(enemy == null) yield break;

        //StartCoroutine(CameraShake.obj.Shake());
        CombatController.obj.backGroundAttack.SetActive(true);

        //Attack();
        yield return StartCoroutine(Attack());


        while(CombatController.obj.selectedEnemy != null){
            // Resetea al inicio del tiempo muerto
            hitSuccess = false;
            doubleAttackAttempted = false;

            yield return new WaitForSeconds(0.2f);

            clickAdvice.SetActive(true);
            canDoubleAttack = true;
            //stateText.text = "Otra vez";

            yield return new WaitForSeconds(0.4f);

            canDoubleAttack = false;
            stateText.text = "";
            clickAdvice.SetActive(false);

            if(hitSuccess){
                //Repite la animación de ataque
                FakeEnemy.obj.animator.Play("FakeEnemyTakeDamage", 0, 0f);
                animator.Play("GunAttack", 0, 0f);
                yield return StartCoroutine(ShowText("EPA!", 0.3f));
                if(enemy != null){
                    enemy.TakeDamage(attack);
                    StartCoroutine(CameraShake.obj.Shake());
                }

                if(enemy == null || enemyDied){
                    machetazo = false;
                    gun = false;
                    enemyDied = false;

                    OutParameter("machetazo");
                    OutParameter("playerAttack");
                    FakeEnemy.obj.OutParameter("playerAttack");
                    CombatController.obj.backGroundAttack.SetActive(false);

                    yield return new WaitForSeconds(0.2f);

                    lifeText.text = hp.ToString();

                    yield break;
                }
            }else {//Se entra al else cuando falla el ataque consecutivo
                CombatController.obj.backGroundAttack.SetActive(false);
                //Hace la animación de regreso
                //animator.SetBool("again", false);
                machetazo = false;
                gun = false;
                OutParameter("machetazo");
                OutParameter("playerAttack");
                FakeEnemy.obj.OutParameter("playerAttack");

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

        //Reinicia el parryAttemped para futuros ataques
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

    public void SetParameter(string parameter){
        animator.SetBool(parameter, true);
    }

    public void OutParameter(string parameter){
        animator.SetBool(parameter, false);
    }

    public IEnumerator GunAttackCoroutine(){
        select.SetActive(false);
        lifeText.text = "";
        SetParameter("playerAttack");
        FakeEnemy.obj.SetParameter("playerAttack");
        

        CombatController.obj.backGroundAttack.SetActive(true);

        yield return new WaitForSeconds(0.6f); // espera al frame del disparo
        StartCoroutine(CameraShake.obj.Shake());

        // Daña a todos los enemigos
        foreach(Enemy enemy in FindObjectsOfType<Enemy>()){
            if(enemy) enemy.TakeDamage(attack);
        }

        yield return new WaitForSeconds(0.3f);

        CombatController.obj.backGroundAttack.SetActive(false);
        OutParameter("playerAttack");
        FakeEnemy.obj.OutParameter("playerAttack");
        machetazo = false;
        gun = false;    
        lifeText.text = hp.ToString();
    }

}
