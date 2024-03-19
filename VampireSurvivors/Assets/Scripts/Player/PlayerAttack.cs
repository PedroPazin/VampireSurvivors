using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    EntityStats entityStats;
    public GameObject projectile;
    public GameObject[] enemies;


    bool canAttack = true;

    float cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        entityStats = gameObject.GetComponent<EntityStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //Guardando todos os inimigos da cena em um array
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies.Length != 0)
        {
            Attack();
        }
    }

    void CooldownCount()
    {
        if(cooldown >= entityStats.attackSpeed)
        {
            canAttack = true;

            cooldown = 0;
        }
        else
        {
            cooldown += Time.deltaTime;
        }
    }

    void Attack()
    {
        GameObject targetEnemy = FindNearestEnemy(enemies);

        if(canAttack)
        {   
            //Instanciando o projetil e pegando seu RigidBody2D
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody2D projectRb = projectileInstance.GetComponent<Rigidbody2D>();

            //Pegando a direcao para onde o projetil vai e adicionando força nele
            Vector2 direction = targetEnemy.transform.position - transform.position;
            direction.Normalize();

            projectRb.AddForce(direction * entityStats.attackRange, ForceMode2D.Impulse);
            canAttack = false;

            //Colocando informaçoes do dano que serao enviadas para o ProjectileDamage
            projectileInstance.GetComponent<ProjectileDamage>().damage = entityStats.attackDamage;
            projectileInstance.GetComponent<ProjectileDamage>().lifeTime = entityStats.projectileLifeSpan;
        }

        CooldownCount();
    }

    GameObject FindNearestEnemy(GameObject[] enemies)
    {
        //fazendo o targetEnemy ser o primeiro index de enemies
        GameObject targetEnemy = enemies[0];

        //Vendo qual inimigo esta mais proximo do jogador e fazendo com que ele seja o targetEnemy
        foreach(GameObject enemy in enemies)
        {
            float dis = Vector2.Distance(transform.position, enemy.transform.position);

            if(dis < Vector2.Distance(transform.position, targetEnemy.transform.position))
            {
                targetEnemy = enemy;
            }
        }

        return targetEnemy;
    }
}
