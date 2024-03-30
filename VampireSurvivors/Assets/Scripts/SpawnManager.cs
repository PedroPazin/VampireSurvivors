using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawns;
    public GameObject enemy;
    EntityStats enemyStats;

    public float cooldown;
    float cooldown_;
    bool canSpawn = true;

    //Timer para aumentar a dificuldade
    float timer;

    //Player
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        cooldown_ = cooldown;
        enemyStats = enemy.GetComponent<EntityStats>();

        //Status base do inimigo padrao
        enemyStats.maxHp = 3;

        //Player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        spawns = GameObject.FindGameObjectsWithTag("SpawnEnemy");
        if(spawns != null && player != null)
        {
            Spawn();
        }

        timer += Mathf.CeilToInt(Time.deltaTime*1000);
    }

    void FixedUpdate()
    {
        
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
