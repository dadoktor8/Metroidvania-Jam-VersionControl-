﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {

    public static MainGameManager instance;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    [SerializeField]
    private bool isObjectiveDone;
    [SerializeField]
    private int objectiveID = 0;

    [SerializeField]
    private InventoryScript Inventory;

    ItemData item;

    public bool IsObjectiveDone
    {
  
        get
        {
            return isObjectiveDone;
        }

        set
        {
            isObjectiveDone = value;
        }
    }

    public int ObjectiveID
    {

        get
        {
            return objectiveID;
        }

        set
        {
            objectiveID = value;
        }

    }
    // Use this for initialization
    void Start () {
  

    }
	
	// Update is called once per frame
	void Update () {
        
        if (Inventory.ConsumeItemByName("Keys") )
        {
            Debug.Log("Keys are in Inventory!");
        }

        if (IsObjectiveDone)
        {
            Debug.Log("Objective Complete!");
        }
	}


}