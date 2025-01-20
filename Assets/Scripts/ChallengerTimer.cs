using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Events;
//using static System.Collections.Specialized.BitVector32;

public class ChallengerTimer : MonoBehaviourPunCallbacks
{

    public bool startTimer;
    public float timeLeft = 30.0f, countDown = 3f; // Set the total countdown time (in seconds)
    public TextMeshProUGUI timerText;
    float totalTime;

    public UnityEvent timeUp, accept;

    private void OnEnable()
    {

        timeLeft = 30f;


        startTimer = true;
        totalTime = timeLeft;

    }





    public void AcceptBtn()
    {
       
       accept.Invoke();
    }


   

    void Update()
    {
        if (startTimer)
            TimeLeft();
        else
            StartCountDown();
    }

    public void Reject()
    {
        startTimer = false;
        timeUp.Invoke();
    }

    void StartCountDown()
    {
    }

    void TimeLeft()
    {
        if (startTimer)
        {
            if (timeLeft >= 0.001f)
                timeLeft -= Time.deltaTime; // Subtract elapsed time from the total time


            float fillAmount = Mathf.Clamp01(timeLeft / totalTime);


            if (timeLeft < 1)
            {
                // Game Over Logic 
                //timeUp.Invoke();
               Reject();
            }
            if (timeLeft < 10 && timeLeft > 1)
            {


            }
        }
        DisplayTime(timeLeft);
    }



    //fprmatting the string in minutes nd second
    void DisplayTime(float timeLeft)
    {
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);

        if (startTimer)
            timerText.text = string.Format("{00:00} : {01:00}", minutes, seconds);
        //else
        //    countdownTxt.text = string.Format("{00:00} : {01:00}", minutes, seconds);
    }








}
