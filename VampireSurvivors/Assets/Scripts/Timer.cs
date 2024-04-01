using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timeText;
    public float timeValue;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("IncreaseTime", 1f, 1f);
    }

    void Update()
    {

    }

    private void DecreaseTime()
    {
        if(timeValue < 0f) return;

        if(timeValue > 0f)
        {
            timeValue --;
        }

        else
        {
            timeValue = 0f;
        }

        DisplayTime(timeValue);
    }

    private void IncreaseTime()
    {
        if(timeValue < 0f) return;

        timeValue++;

        DisplayTime(timeValue);
    }

    private void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0f;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);   
    }
}
