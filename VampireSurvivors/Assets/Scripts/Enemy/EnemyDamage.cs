using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    EntityStats entityStats;

    // Start is called before the first frame update
    void Start()
    {
        entityStats = gameObject.GetComponent<EntityStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<EntityStats>().ReduceHp(entityStats.attackDamage);
        }
    }
}
