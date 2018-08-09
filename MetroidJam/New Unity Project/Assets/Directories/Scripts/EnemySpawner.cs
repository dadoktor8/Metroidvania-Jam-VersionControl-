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
    
    int Counter = 0;

    // Use this for initialization
    void Start () {

        StartCoroutine(Spawner());

    }
	
	// Update is called once per frame
	void Update () {

        if (Counter <= EnemyLimit)
        {
            // InvokeRepeating("Spawner", 1.0f, 5.0f);
            
        }

    }

    IEnumerator Spawner()
    {
        while (Counter < EnemyLimit)
        {
            enemyBlobman.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
            Instantiate(enemyBlobman, spawnPoint.position, spawnPoint.rotation);
            Instantiate(enemyBlobman, spawnPoint1.position, spawnPoint1.rotation);
            Counter++;
            yield return new WaitForSeconds(3.0f);

        }
    }
}
