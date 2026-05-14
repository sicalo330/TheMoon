using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float iniCamPosFrame;

    void Update(){
        iniCamPosFrame = transform.position.x;
        transform.position = new Vector3(Player.obj.transform.position.x,transform.position.y,transform.position.z);
    }

}
