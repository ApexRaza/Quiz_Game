using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TImer : MonoBehaviour
{

    public Image timeFiller;
    public bool startTimer;
    public float timeLeft = 10.0f; // Set the total countdown time (in seconds)
    public TextMeshProUGUI startText;
    float totalTime;
    // Start is called before the first frame update
    void Start()
    {
        totalTime = timeLeft;
    }

    public void ResetTimer()
    {
        timeFiller.fillAmount = 1;
        timeLeft = 10;
        totalTime = timeLeft;
        startTimer=true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            if (timeLeft >= 0.001f)
                timeLeft -= Time.deltaTime; // Subtract elapsed time from the total time





            float fillAmount = Mathf.Clamp01(timeLeft / totalTime);

            timeFiller.fillAmount = fillAmount;
            if (timeLeft < 1)
            {
                // Game Over Logic 

            }
            if (timeLeft < 10 && timeLeft > 1)
            {


            }
        }
    }


    //fprmatting the string in minutes nd second
    void DisplayTime(float timeLeft)
    {
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);


        startText.text = string.Format("{00:00} : {01:00}", minutes, seconds);

    }





}
