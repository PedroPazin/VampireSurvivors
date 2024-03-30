using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    float damage;

    EntityStats entityStats;

    // Start is called before the first frame update
    void Start()
    {
        entityStats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
        damage = entityStats.explosionDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EntityStats>().ReduceHp(damage);
        }
    }
}
