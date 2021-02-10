using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private Vector3 placeToSpawnFrom;
    public Transform player;
    public bool spawner=false;
    public float timer;
    // Update is called once per frame
    private void Start()
    {
        timer = 0;
        enemyPrefab.GetComponent<HomeObjects>().targetTransform = player;
        placeToSpawnFrom = new Vector3(player.position.x, player.position.y + 5, player.position.z);
    }

    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(Time.timeSinceLevelLoad);
        if(timer>5)
        {
            spawner = true;
            timer = 0;
        }
        if (spawner == true)
        {
            Instantiate(enemyPrefab, placeToSpawnFrom, Quaternion.Inverse(Quaternion.identity));
            spawner = false;
           
        }
        
    }
}
