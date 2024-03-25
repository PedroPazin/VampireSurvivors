using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{   
    public float lifeTime;

    public float damage;
    public bool isPlayer;

    int piercedEnemies = 0;

    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
        player = GameObject.FindGameObjectWithTag("Player");
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

            //Verifica quantos inimigos o projetil do jogador atingiu, para assim, destrui-lo
            if(collision.gameObject.tag == "Enemy" && isPlayer == true)
            {
                piercedEnemies += 1;
                if(piercedEnemies >= player.GetComponent<EntityStats>().pierce)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
