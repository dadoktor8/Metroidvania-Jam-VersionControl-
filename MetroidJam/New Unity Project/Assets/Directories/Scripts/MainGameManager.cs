using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
    private InventoryScript DRInventory;
    [SerializeField]
    FadeIn fadeScreen;
    [SerializeField]
    GameObject BoxersRobert;
    [SerializeField]
    GameObject DressedRobert;
    [SerializeField]
    GameObject cCam;
    [SerializeField]
    GameObject CutScene;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quiting...");
        }
        
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
            NextObjective("ClosetObjective");
        }

        if (Inventory.ConsumeItemByName("Closet"))
        {
            Debug.Log("I dressed up! Ready for work!");
            StartCoroutine(fadeScreen.FadeTo());
            RemoveObjective("ClosetObjective");
            BoxersRobert.SetActive(false);
            DressedRobert.SetActive(true);
            cCam.SetActive(true);
            NextObjective("KeysObjective");
        }

        if (DRInventory.ConsumeItemByName("Keys"))
        {
            Debug.Log("Keys are in Inventory!");
            StartCoroutine(fadeScreen.FadeTo());
            RemoveObjective("KeysObjective");
            NextObjective("TelephoneObjective");
        }

        if (DRInventory.ConsumeItemByName("Telephone"))
        {
            Debug.Log("I picked up the phone!");
            RemoveObjective("TelephoneObjective");
            DressedRobert.SetActive(false);
            CutScene.SetActive(true);
           // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //NextObjective("KeysObjective");
        }

        if (IsObjectiveDone)
        {
            Debug.Log("Objective Complete!");
        }
    }

    void NextObjective(string obj)
    {
         ObjectivesList[objectiveID].SetActive(true);
    }

    void RemoveObjective(string obj)
    {
        ObjectivesList.RemoveAt(objectiveID);
    }

}
