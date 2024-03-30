using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //Lista de projeteis
    public List<GameObject> projectileInstanceList = new List<GameObject>();

    //Lista de inimigos proximos ao player
    public List<GameObject> enemiesClosestToPlayer = new List<GameObject>();

    EntityStats entityStats;
    public GameObject projectile;
    public GameObject[] enemies;


    bool canAttack = true;

    float cooldown = 0;

    //UPS do jogador -=-=-=-=-=-=-=-=-
    public GameObject explosion;
    public GameObject deathArea;

    //MultProjectile
    bool canAdd = true;
    

    // Start is called before the first frame update
    void Start()
    {
        entityStats = gameObject.GetComponent<EntityStats>();

        //Desativa o deathArea ao iniciar
        deathArea.SetActive(false);
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

        //Ativa o DeathArea
        if(HUD.Instance.deathArea)
        {
            deathArea.SetActive(true);
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
        //Pegando qual inimigo vai ser o alvo
        GameObject targetEnemy = FindNearestEnemy(enemies);

        //Adicionando projeteis a lista de projeteis ProjecitleInstanceList
        AddProjectileInList();

        if(canAttack)
        {   
            foreach(GameObject projectileItem in projectileInstanceList)
            {
                if(targetEnemy.GetComponent<EntityStats>().targeted == false || targetEnemy.GetComponent<EntityStats>().targeted == true)
                {
                    //Instanciando o projetil
                    GameObject projectileInstance = Instantiate(projectileItem, gameObject.transform.position, Quaternion.identity);

                    //Pegando o RigidBody2D do projetil
                    Rigidbody2D projectRb = projectileInstance.GetComponent<Rigidbody2D>();


                    //Pegando a direcao para onde o projetil vai e adicionando força nele
                    Vector2 direction = targetEnemy.transform.position - transform.position;
                    direction.Normalize();

                    projectRb.AddForce(direction * entityStats.attackRange, ForceMode2D.Impulse);
                    canAttack = false;

                    //Colocando informaçoes do dano que serao enviadas para o ProjectileDamage
                    projectileInstance.GetComponent<ProjectileDamage>().damage = entityStats.attackDamage;
                    projectileInstance.GetComponent<ProjectileDamage>().lifeTime = entityStats.projectileLifeSpan;

                    //Chance de critico
                    float numRandom = Random.Range(0f, 100f);
                    if(numRandom <= entityStats.criticalChance)
                    {
                        projectileInstance.GetComponent<ProjectileDamage>().damage += entityStats.attackDamage;
                    }

                    //Fazendo o targeted do inimigo alvo ser true
                    targetEnemy.GetComponent<EntityStats>().targeted = true;
                }
            }
        }

        CooldownCount();
    }

    void AddProjectileInList()
    {
        if(HUD.Instance.multProjectile)
        {
            canAdd = true;
        }

        if(canAdd == true)
        {
            projectileInstanceList.Add(projectile);
            canAdd = false;
            HUD.Instance.multProjectile = false;
        }
    }

    GameObject FindNearestEnemy(GameObject[] enemies)
    {
        enemiesClosestToPlayer = enemies.ToList();

        //fazendo o targetEnemy ser o primeiro item de enemies
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
