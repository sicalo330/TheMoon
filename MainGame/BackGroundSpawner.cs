    using System.Collections;
    using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

    public class BackGroundSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject backGroundPrefab;

        [SerializeField] private float backgroundWidth = 17.74f;

        private Transform player;

        private GameObject currentBG;
        private GameObject leftBG;
        private GameObject rightBG;

        private float currentCenterX = 0f;

        void Start()
        {
            player = FindObjectOfType<Player>().transform;
            //Inicialmente va a instancia un backgrounen el punto cero
            currentBG = Instantiate(backGroundPrefab,Vector3.zero,Quaternion.identity);
        }

        void Update()
        {
            SpawnSideBackgrounds();
            UpdateCurrentBackground();
        }

        void SpawnSideBackgrounds()
        {
            //Cuando el jugador llegue al borde derecho
            
            if(player.position.x > currentCenterX + backgroundWidth * 0.00001f){
                if(rightBG == null){
                    rightBG = Instantiate(backGroundPrefab,new Vector3(currentCenterX + backgroundWidth,0,0),Quaternion.identity);
                }
            }

            //LO mismo pero en el borde izquierdo
            if(player.position.x < currentCenterX - backgroundWidth * 0.000001f){
                if(leftBG == null){
                    leftBG = Instantiate(backGroundPrefab,new Vector3(currentCenterX - backgroundWidth,0,0),Quaternion.identity);
                }
            }
        }

        //Esta función se llama cuando el jugador entra completamente al otro background generado
        //SE desactiva el background viejo
        void UpdateCurrentBackground()
        {
            if(player.position.x > currentCenterX + backgroundWidth/2){
                Destroy(leftBG);
                leftBG = currentBG;
                currentBG = rightBG;
                rightBG = null;

                currentCenterX += backgroundWidth;
            }

            if(player.position.x < currentCenterX - backgroundWidth/2){
                Destroy(rightBG);
                rightBG = currentBG;
                currentBG = leftBG;

                leftBG = null;
                currentCenterX -= backgroundWidth;
            }
        }
    }