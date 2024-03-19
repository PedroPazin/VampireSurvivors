using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    GameObject player;
    EntityStats entityStats;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        entityStats = gameObject.GetComponent<EntityStats>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if(player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, entityStats.baseSpeed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            entityStats.ReduceHp(entityStats.maxHp + 2);
        }
    }
}
