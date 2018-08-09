using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    GameObject enemyBlobman;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    Transform spawnPoint1;
    [SerializeField]
    Transform spawnPoint2;
    [SerializeField]
    int EnemyLimit = 0;
    
    [SerializeField]
    float RespawnTime = 13.0f;

    int Counter = 0;

    // Use this for initialization
    void Start () {

        StartCoroutine(Spawner());
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Counter == (EnemyLimit - 1) )
        {
            //StartCoroutine(Reset());
            ///InvokeRepeating("ResetInvoke", 10.0f,10.0f);
            
        }

        

    }

    IEnumerator Spawner()
    {
        while (true)
        {
            enemyBlobman.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
            Instantiate(enemyBlobman, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(3.0f);
            Instantiate(enemyBlobman, spawnPoint1.position, spawnPoint1.rotation);  
            yield return new WaitForSeconds(3.0f);
            Counter++;

            if (Counter >= EnemyLimit)
            {
                Counter = 0;
                yield return new WaitForSeconds(RespawnTime);
                
            }
            
        }
    }

    IEnumerator Reset()
    {
        Debug.Log("Respawning in...");
        yield return new WaitForSeconds(2.9f);
        Counter = 0;
        
    }

    void ResetInvoke()
    {
        Debug.Log("Respawning in...");
        Counter = 0;

    }
}
