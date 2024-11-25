using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Events;
//using static System.Collections.Specialized.BitVector32;

public class Betting : MonoBehaviourPunCallbacks
{
    public int initialValue;
    public TextMeshProUGUI amountTxt;
    public GameObject buttonObj, startTimerObj;

    public bool startTimer;
    public float timeLeft = 30.0f, countDown = 3f; // Set the total countdown time (in seconds)
    public TextMeshProUGUI timerText,countdownTxt;
    float totalTime;

   public UnityEvent timeUp;

    private void OnEnable()
    {
        initialValue = 20;
        timeLeft = 30f;
        countDown = 3f;
        amountTxt.text = initialValue.ToString();
        startTimer = true;
        totalTime = timeLeft;
        buttonObj.SetActive(true);
        startTimerObj.SetActive(false);
    }

    public void RasieValue(int num)
    {
        GetComponent<PhotonView>().RPC(nameof(RaiseRPC), RpcTarget.All, num);

    }

    [PunRPC]
    public void RaiseRPC(int num)
    {
        initialValue += num;
        amountTxt.text = initialValue.ToString();
    }

    public void AcceptBtn()
    {
        GetComponent<PhotonView>().RPC(nameof(AcceptRPC), RpcTarget.All);
    }


    [PunRPC]
   public void AcceptRPC()
    {
        startTimer = false;
        buttonObj.SetActive(false);
        startTimerObj.SetActive(true);

    }

    void Update()
    {
        if (startTimer)
            TimeLeft();
        else
            StartCountDown();
    }



    void StartCountDown()
    {
        {
            if (countDown >= 0.001f)
                countDown -= Time.deltaTime; // Subtract elapsed time from the total time


            float fillAmount = Mathf.Clamp01(countDown / totalTime);


            if (countDown < 1)
            {
                // Game Over Logic 
                timeUp.Invoke();
                

            }
           
        }
        DisplayTime(countDown);
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
               AcceptBtn();
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
        else
            countdownTxt.text = string.Format("{00:00} : {01:00}", minutes, seconds);
    }








}
