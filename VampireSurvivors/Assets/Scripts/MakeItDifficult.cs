using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItDifficult : MonoBehaviour
{
    Timer timer;

    GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Invoke("UpEnemiesStats(enemies)", 5);
    }

    void UpEnemiesStats(GameObject[] enemiesToUp)
    {
        if(((int)(timer.timeValue / 5)) > 0)
        {
            foreach(GameObject enemy in enemiesToUp)
            {
                EntityStats enemyStats = enemy.GetComponent<EntityStats>();

                //Stats a serem aumentados dos inimigos em geral

                enemyStats.EnemyLevelUp(1);
            }
        }
    }

}
