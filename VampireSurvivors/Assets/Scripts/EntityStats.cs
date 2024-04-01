using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float criticalChance;
    public int pierce;
    public float expMult;

    //Stats dos UPS do player
    public float explosionDamage;
    public float deathAreaDamage;


    //Apenas do inimigo
    public bool targeted;

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
        //Fazer a aparecer o DamagePopup com o dano recebido
        GameObject newDamagePopup = Instantiate(HUD.Instance.damagePopup, this.transform.position, Quaternion.identity);
        
        newDamagePopup.GetComponentInChildren<Text>().text = damage.ToString();
        newDamagePopup.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2f, 2f), 5), ForceMode2D.Impulse);
        
        Destroy(newDamagePopup, 1);

        //Reduz da vida o dano tomado
        hp -= damage;
        CheckDeath();
    }

    public void AddHp(float health)
    {
        hp += health;
        CheckOverHealth();
    }

    void CheckOverHealth()
    {
        if(hp > maxHp)
        {
            hp = maxHp;
        }
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
        exp += xp + (xp * (expMult/100));
        
        if(exp >= level*100)
        {
            level += 1;
            exp = 0;

            //Checa se o level for par, entao mostrara a tela de Upgrade, se for impar mostrara a tela de LevelUp
            if(level % 2 == 0)
            {
                HUD.Instance.SetupUpgradeScreen();
            }
            else
            {
                HUD.Instance.SetupLevelScreen();
            }
            
            //Parando o tempo do jogo
            Time.timeScale = 0;
        }

    }

    public void EnemyLevelUp(int levelUp)
    {
        for (int i = 0; i < levelUp; i++)
        {
            level += 1;

            maxHp += 1;
            attackDamage += 1.5f;
            attackSpeed -= 0.3f;
            exp += 20;
        }
    }
}
