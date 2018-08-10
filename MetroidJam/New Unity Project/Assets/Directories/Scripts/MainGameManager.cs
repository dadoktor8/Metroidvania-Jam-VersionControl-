using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    Text Robertsays;
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
    [SerializeField]
    GameObject PreEnemy;
    [SerializeField]
    GameObject LockedDoor;
    [SerializeField]
    GameObject UnLockedDoor;
    [SerializeField]
    GameObject PreLockedDoor;
    [SerializeField]
    GameObject PreUnLockedDoor;

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
            StartCoroutine( RobertTalk( ("Gulp! I feel better now!") ) );
            StartCoroutine(fadeScreen.FadeTo());
            RemoveObjective("PillObjective");
            NextObjective("ShowerObjective");
        }

        if (Inventory.ConsumeItemByName("Shower"))
        {
            Debug.Log("Whew! Nice bath!");
            AudioManager.instance.Play("SHOWER");
            StartCoroutine(RobertTalk(("Whew! Nice bath!")));
            StartCoroutine(fadeScreen.FadeTo());
            RemoveObjective("ShowerObjective");
            NextObjective("ClosetObjective");
        }

        if (Inventory.ConsumeItemByName("Closet"))
        {
            Debug.Log("I dressed up! Ready for work!");
            StartCoroutine(RobertTalk(("Nice! Just to pick up my Keys and go to work!")));
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
            StartCoroutine(RobertTalk(("Hm? The phone is ringing?")));
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

        // Level02

        if (Inventory.ConsumeItemByName("KeysRm02"))
        {
            Debug.Log("Door Unlocked!");
            LockedDoor.SetActive(false);
            UnLockedDoor.SetActive(true);
            StartCoroutine(fadeScreen.FadeTo());
            RemoveObjective("KeysObjective");
        }

        if (Inventory.ConsumeItemByName("Knife"))
        {
            Debug.Log("Pick Up a Knife!!");
            StartCoroutine(RobertTalk(("Better take it with me! \n ( F to attack )")));
            DressedRobert.GetComponentInChildren<PlayerAttackScript>().enableMelee = true;
            RemoveObjective("KnifeObjective");
            AudioManager.instance.Play("Upgrade");
        }

        if (Inventory.ConsumeItemByName("Pistol"))
        {
            Debug.Log("Pick Up a Pistol!!");
            StartCoroutine(RobertTalk(("Thank God! A weapon! \n ( Left click to shoot )")));
            DressedRobert.GetComponentInChildren<PlayerAttackScript>().enablePistol = true;
            RemoveObjective("PistolObjective");
            AudioManager.instance.Play("Upgrade");
        }

        if (Inventory.ConsumeItemByName("ShotGun"))
        {
            Debug.Log("Pick Up a ShotGun!!");
            StartCoroutine(RobertTalk(("Now i am stronger! \n ( Right click to shoot )")));
            DressedRobert.GetComponentInChildren<PlayerAttackScript>().enableShotgun = true;
            RemoveObjective("ShotGunObjective");
            AudioManager.instance.Play("Upgrade");
        }

        if (Inventory.ConsumeItemByName("Stomp"))
        {
            Debug.Log("Stomp Ability On!!");
            StartCoroutine(RobertTalk(("I feel different ! My hands are stronger! \n ( E to Stomp )")));
            DressedRobert.GetComponentInChildren<PlayerAttackScript>().enableStomp = true;
            RemoveObjective("StompObjective");
            AudioManager.instance.Play("Upgrade");
        }

        if (PreEnemy == null)
        {
            Debug.Log("PreEnemy is Down!");
            PreUnLockedDoor.SetActive(true);
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

    IEnumerator RobertTalk(string RobSays)
    {
        yield return new WaitForSeconds(1.5f);
        Robertsays.text = RobSays;
        yield  return new WaitForSeconds(2.5f);
        Robertsays.text = "";
    }

}
