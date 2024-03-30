using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathArea : MonoBehaviour
{

    EntityStats playerStats;

    //Lista de inimigos na area de dano
    public List<GameObject> enemiesInArea = new List<GameObject>();

    //Cooldown
    public float cooldown;
    float cooldown_;
    bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = gameObject.GetComponentInParent<EntityStats>();

        cooldown_ = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        DoDamage();
    }

    void DoDamage()
    {
        
        if(canAttack)
        {
            if(enemiesInArea.Count != 0)
            {
                foreach(GameObject enemy in enemiesInArea)
                {
                    if(enemy != null)
                    {
                        enemy.GetComponent<EntityStats>().ReduceHp(playerStats.deathAreaDamage);
                    }
                }
            }
            canAttack = false;
            cooldown_ = cooldown;
            
        }

        
        CooldownCount();
        
    }

    void CooldownCount()
    {
        if(cooldown_ <= 0)
        {
            canAttack = true;
            
            cooldown_ = cooldown;
        }
        else
        {
            cooldown_ -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemiesInArea.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemiesInArea.Remove(collision.gameObject);
        }
    }
}
