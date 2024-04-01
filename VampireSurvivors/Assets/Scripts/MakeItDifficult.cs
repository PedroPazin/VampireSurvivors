using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItDifficult : MonoBehaviour
{
    Timer timer;

    public GameObject enemy;

    EntityStats enemyStats;

    public SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Timer>();

        InvokeRepeating("FuncMakeItDifficult", 5f , 5f);

        enemyStats = enemy.GetComponent<EntityStats>();

        enemyStats.EnemyDefaultLevelZero();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Funcao que vai chamar todas as outras funcões que precisam de tempo
    void FuncMakeItDifficult()
    {
        UpEnemiesStats();
        SpawnFaster();
    }

    //LevelUP dos inimigos
    void UpEnemiesStats()
    {
        print("levelUP");
        
        enemyStats.EnemyDefaultLevelUp(1);
        
    }

    //Aumentar velocidade de spawn dos inimigos
    void SpawnFaster()
    {
        print("spawn faster");
        
        spawnManager.cooldown -= 0.1f;
        if(spawnManager.cooldown <= 0.5f)
        {
            spawnManager.cooldown = 0.5f;
        }

    }

}
