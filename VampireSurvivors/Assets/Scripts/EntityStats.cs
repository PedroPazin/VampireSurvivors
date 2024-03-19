using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public float baseSpeed;
    public float maxHp;
    public float hp;
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;
    public float projectileLifeSpan;
    public float exp;

    //Apenas do player(por enquanto)
    public float level = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReduceHp(float damage)
    {
        hp -= damage;
        CheckDeath();
    }

    public void CheckDeath()
    {
        if(hp <= 0)
        {
            Destroy(this.gameObject);

            //Checa se o objeto morte é diferente do jogador, se for, adiciona xp ao jogador
            if(this.gameObject.tag != "Player")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>().AddExp(exp);
            }
        }
    }

    void AddExp(float xp)
    {
        exp += xp;
        
        if(exp >= level*100)
        {
            level += 1;
            exp = 0;
            HUD.Instance.SetupLevelScreen();

            Time.timeScale = 0;
        }

    }
}
