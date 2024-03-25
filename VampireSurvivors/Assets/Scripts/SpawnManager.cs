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


    // Start is called before the first frame update
    void Start()
    {
        cooldown_ = cooldown;
        enemyStats = enemy.GetComponent<EntityStats>();

        //Status base do inimigo padrao
        enemyStats.maxHp = 3;
    }

    // Update is called once per frame
    void Update()
    {
        spawns = GameObject.FindGameObjectsWithTag("SpawnEnemy");
        if(spawns != null)
        {
            Spawn();
        }

        timer += Mathf.CeilToInt(Time.deltaTime*1000);
        print(timer);
    }

    void FixedUpdate()
    {
        
    }


    void Spawn()
    {
        //Aumentando status do inimigo com o passar do tempo
        if(timer == 5)
        {
            enemyStats.maxHp += 5;
        }

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
