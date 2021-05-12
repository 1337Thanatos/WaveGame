using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Flags]
    public enum TestFlags 
    {
        Full , 
        Basic
    }

    public enum PlayerType
    {
        LocalPlayer,
        RemotePlayer,
        AI
    }


    public TestFlags testProperties;

    //Used to differentiate between teams, could eventually result in battle royales I guess
    [Tooltip("Used to store the different teams at runtime to determine FoF")]
    public string[] TeamNames;

    //Used to store the different player names
    [Tooltip("At runtime this will get populated with the different player names")]
    public string[] PlayerNames;

    //Used to handle the camera follow mechanics
    [Tooltip("link the cameraFollow component here from the main camera")]
    public CameraFollow cameraFollow;
    [Tooltip("How fast does the main camera move when moved by the player?")]
    public float cameraMoveSpeed = 40f;
    private Vector3 cameraFollowPosition = new Vector3(0,100);

    [System.Serializable]
    public struct ObjectOwner
    {
        public PlayerType PlayerType;
        public string TeamName;
        public string PlayerOwner;
    }

    [Tooltip("")]
    public GameObject wayPoints;



    
    // Start is called before the first frame update
    void Start()
    {
        cameraFollow.Setup(() => cameraFollowPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKey(KeyCode.W))
        {
            cameraFollowPosition.z += cameraMoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            cameraFollowPosition.x -= cameraMoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            cameraFollowPosition.z -= cameraMoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            cameraFollowPosition.x += cameraMoveSpeed * Time.deltaTime;
        }
    }
}
