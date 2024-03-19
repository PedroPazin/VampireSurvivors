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
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
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
        xpSlider.value = playerStats.xp;
    }
}
