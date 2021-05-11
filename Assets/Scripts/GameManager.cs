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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
