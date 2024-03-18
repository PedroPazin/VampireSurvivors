using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    EntityStats entityStats;
    public GameObject projectile;

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
        Attack();
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
        GameObject targetEnemy = FindNearestEnemy();

        if(canAttack)
        {
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody2D projectRb = projectileInstance.GetComponent<Rigidbody2D>();

            Vector2 direction = targetEnemy.transform.position - transform.position;
            direction.Normalize();

            projectRb.AddForce(direction, ForceMode2D.Impulse);
            canAttack = false;
        }

        CooldownCount();
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject targetEnemy = enemies[0];

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
