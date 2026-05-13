using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player obj;
    [SerializeField]private float maxSpeed = 10f;
    [SerializeField]private float minY;
    [SerializeField]private float maxY;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpSpeed = 5f;
    private Camera mainCamera;
    public Vector2 currentPosition;
    private Vector2 basePosition;
    private float jumpOffset = 0f;
    private bool isJump = false;

    void Start(){
        mainCamera = Camera.main;
        basePosition = transform.position;
    }

    void Update(){
        currentPosition = transform.position;
        FollowMousePositionDelayed(maxSpeed);
    }

    void Awake(){
        if (obj == null){
            obj = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void FollowMousePosition(){
        transform.position = GetWorldPositionFromMouse();
    }

    private void FollowMousePositionDelayed(float maxSpeed){
        Vector2 targetPos = GetWorldPositionFromMouse();
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        basePosition = Vector2.MoveTowards(basePosition, targetPos, maxSpeed * Time.deltaTime);
        transform.position = new Vector3(basePosition.x, basePosition.y + jumpOffset, transform.position.z);
    }

    private Vector2 GetWorldPositionFromMouse(){
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
