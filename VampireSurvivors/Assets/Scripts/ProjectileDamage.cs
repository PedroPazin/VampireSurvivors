using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{   
    public float lifeTime;

    public float damage;
    public bool isPlayer;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "Enemy" && isPlayer == true) || (collision.gameObject.tag == "Player" && isPlayer == false))
        {
            collision.GetComponent<EntityStats>().ReduceHp(damage);
            Destroy(this.gameObject);
        }
    }
}
