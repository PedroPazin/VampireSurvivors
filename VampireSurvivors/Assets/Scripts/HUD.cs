using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set;}

    public Slider hpSlider;
    public Slider xpSlider;

    EntityStats playerStats;

    public string stats;

    public GameObject levelUpScreen;

    public GameObject upgradeScreen;

    public GameObject damagePopup;

    //Verifica se o player possui os upgrades: true or false
    public bool explosion = false;
    public bool multProjectile = false;
    public bool deathArea = false;

    //PlayerAttack
    PlayerAttack playerAttack;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Pegando os stats do player
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();

        //Ativando as screens
        levelUpScreen.SetActive(false);
        upgradeScreen.SetActive(false);

        //Pegando o script PlayerAttack
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        HpSliderChange();
        XpSliderChange();
    }

    void HpSliderChange()
    {
        hpSlider.maxValue = playerStats.maxHp;
        hpSlider.value = playerStats.hp;
    }

    void XpSliderChange()
    {
        xpSlider.maxValue = playerStats.level*100;
        xpSlider.value = playerStats.exp;
    }


    //Funçoes de LevelUp
    public void SelectedStat(string stat)
    {
        if(stat == "atkDamage")
        {
            playerStats.attackDamage += 1;
        }
        if(stat == "atkSpeed")
        {
            playerStats.attackSpeed -= 0.2f;
        }
        if(stat == "atkRange")
        {
            playerStats.attackRange += 0.1f;
        }
        if(stat == "maxHp")
        {
            playerStats.maxHp += 5;
            playerStats.AddHp(5);
        }
        if(stat == "criticalChance")
        {
            playerStats.criticalChance += 1.5f;
        }
        if(stat == "pierce")
        {
            playerStats.pierce += 1;
        }
        if(stat == "expMult")
        {
            playerStats.expMult += 8;
        }

        Time.timeScale = 1;
        levelUpScreen.SetActive(false);
    }

    public void SetupLevelScreen()
    {
        levelUpScreen.SetActive(true);
    }

    //Funçoes de Upgrade
    public void SelectedUpgrade(string upgrade)
    {
        if(upgrade == "explosion")
        {
            if(explosion)
            {
                playerStats.explosionDamage += 0.2f;
            }

            explosion = true;
        }

        if(upgrade == "multProjectile")
        {
            multProjectile = true;
        }

        if(upgrade == "deathArea")
        {
            if(deathArea)
            {
                //Aumentando o dano do DeathArea
                playerStats.deathAreaDamage += 0.2f;

                //Pegando a escala X do DeathArea
                float scaleX = playerAttack.deathArea.transform.localScale.x;

                //Criando novas variaveis para serem as novas escalas
                float newScaleX = scaleX + 0.2f;
                float newScaleY = newScaleX/2;

                //Adicionando as novas variaveis nos valores da escala do DeathArea
                playerAttack.deathArea.transform.localScale = new Vector2(newScaleX, newScaleY); 
            }

            deathArea = true;
        }

        //Desativa a tela e volta o tempo do jogo
        upgradeScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void SetupUpgradeScreen()
    {
        upgradeScreen.SetActive(true);
    }
}
