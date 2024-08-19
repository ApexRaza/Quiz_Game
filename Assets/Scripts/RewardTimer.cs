using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public class RewardTimer : MonoBehaviour
{
    private string timerKey;
    private Button button;
    public float requiredTime = 30.0f; // Time in seconds
    public TextMeshProUGUI timer;
    private bool timeOver = true;
    public UnityEvent timeFinished;

    private DateTime timerStart;
    private DateTime timerEnd;
    private bool isTimerRunning = false;

    private void Awake()
    {
        timerKey = gameObject.name + "_TimerEnd";
        timer.gameObject.SetActive(false);
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(StartTimer);

        // Check if a timer was already running
        if (PlayerPrefs.HasKey(timerKey))
        {
            timerEnd = DateTime.Parse(PlayerPrefs.GetString(timerKey));
            double remainingTime = (timerEnd - DateTime.Now).TotalSeconds;

            if (remainingTime > 0)
            {
                requiredTime = (float)remainingTime;
                timeOver = false;
                isTimerRunning = true;
            }
            else
            {
                timeFinished.Invoke();
                requiredTime = 0;
            }
        }
    }

    private void StartTimer()
    {
        if (!isTimerRunning)
        {
            timeOver = false;
            isTimerRunning = true;
            timerStart = DateTime.Now;
            timerEnd = timerStart.AddSeconds(requiredTime);

            // Save timer end time in PlayerPrefs
            PlayerPrefs.SetString(timerKey, timerEnd.ToString());
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        if (!timeOver)
        {
            timer.gameObject.SetActive(true);
            double remainingTime = (timerEnd - DateTime.Now).TotalSeconds;

            if (remainingTime > 0)
            {
                requiredTime = (float)remainingTime;
                DisplayTime(requiredTime);
            }
            else
            {
                timeOver = true;
                isTimerRunning = false;
                requiredTime = 0;
                DisplayTime(0);
                timeFinished.Invoke();
                PlayerPrefs.DeleteKey(timerKey);
            }
        }
    }

    void DisplayTime(float timeLeft)
    {
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnApplicationQuit()
    {
        if (isTimerRunning)
        {
            PlayerPrefs.SetString(timerKey, timerEnd.ToString());
            PlayerPrefs.Save();
        }
    }
}