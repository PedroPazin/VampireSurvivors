using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawns;
    public GameObject enemy;

    public float cooldown;
    float cooldown_;
    bool canSpawn = true;


    // Start is called before the first frame update
    void Start()
    {
        cooldown_ = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        spawns = GameObject.FindGameObjectsWithTag("SpawnEnemy");
        if(spawns != null)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        if(canSpawn)
        {
            int spawnIndex = Random.Range(0, spawns.Length);

            Instantiate(enemy, spawns[spawnIndex].transform.position, Quaternion.identity); 
            canSpawn = false;
        }

        CooldownSpawn();
    }

    void CooldownSpawn()
    {
        if(cooldown_ <= 0)
        {
            canSpawn = true;
            cooldown_ = cooldown;
        }
        else
        {
            cooldown_ -= Time.deltaTime;
        }
    }
}
