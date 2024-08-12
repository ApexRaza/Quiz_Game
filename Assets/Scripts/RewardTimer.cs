using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class RewardTimer : MonoBehaviour
{
    private Button button;
    public float requiredTime = 30.0f;
    public TextMeshProUGUI timer;
    private bool timeOver = true;
    public UnityEvent timeFinished;

    private void Awake()
    {
        timer.gameObject.SetActive(false);
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(startTimer);
    }
    private void startTimer() 
    {
        if(requiredTime >= 0) 
        {
            Debug.Log("helooooo");
            timeOver = false;
        }
    }
    private void Update()
    {
        if (!timeOver) 
        {
            timer.gameObject.SetActive(true);
            if(requiredTime >= 1) 
            {
                requiredTime -=Time.deltaTime;
            }

            if(requiredTime < 1) 
            {
                timeOver = true;
                timeFinished.Invoke();


            }
            DisplayTime(requiredTime);
        }
    }

    void DisplayTime(float timeLeft)
    {
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);


        timer.text = string.Format("{00:00} : {01:00}", minutes, seconds);

    }
}
