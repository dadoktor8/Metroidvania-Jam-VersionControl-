using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {

    public static MainGameManager instance;


    [SerializeField]
    List<GameObject> ObjectivesList = new List<GameObject>();

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
    [SerializeField]
    FadeIn fadeScreen;


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
    void Start()
    {


        for (int i = 1; i < ObjectivesList.Count; i++)
        {
           // ObjectivesList[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {

        Objectives();
        
	}

    void Objectives() // Where the Objectives trigger
    { 

        if (Inventory.ConsumeItemByName("Pill"))
        {
            Debug.Log("Gulp! I feel better now!");
            StartCoroutine(fadeScreen.FadeTo());
            RemoveObjective("PillObjective");
            NextObjective("ShowerObjective");
        }

        if (Inventory.ConsumeItemByName("Shower"))
        {
            Debug.Log("Whew! Nice bath!");
            StartCoroutine(fadeScreen.FadeTo());
            RemoveObjective("ShowerObjective");
            NextObjective("KeysObjective");
        }

        if (Inventory.ConsumeItemByName("Keys"))
        {
            Debug.Log("Keys are in Inventory!");
            StartCoroutine(fadeScreen.FadeTo());
            RemoveObjective("KeysObjective");
            NextObjective("TelephoneObjective");
        }

        if (Inventory.ConsumeItemByName("Telephone"))
        {
            Debug.Log("I picked up the phone!");
            RemoveObjective("TelephoneObjective");
            ///NextObjective("KeysObjective");
        }

        if (IsObjectiveDone)
        {
            Debug.Log("Objective Complete!");
        }
    }

    void NextObjective(string obj)
    {
        for (int i = 0; i < ObjectivesList.Count; i++)
        {
            if (ObjectivesList[i].name == obj)
            {
                ObjectivesList[i].SetActive(true);
                
            }

            
        }
        
    }

    void RemoveObjective(string obj)// Get a Runtime Error!
    {
        for (int i = 0; i < ObjectivesList.Count; i++)
        {
            if (ObjectivesList[i].name == obj)
            {
                Debug.Log(ObjectivesList[i].name);
                ObjectivesList.RemoveAt(i);
            }
        }

    }

}
