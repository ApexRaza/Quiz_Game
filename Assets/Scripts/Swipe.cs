using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    private Vector3 fp;   
    private Vector3 lp;   
    private float dragDistance;
    public UiManager uiManager;
    public QuizHandler quizHandler;
    public TImer timer;

   public RectTransform rightBtn;

    void Start()
    {
        uiManager = FindAnyObjectByType<UiManager>();

        quizHandler = FindAnyObjectByType<QuizHandler>();
        timer = FindAnyObjectByType<TImer>();
        dragDistance = Screen.height * 15 / 100;
        //Debug.Log( rightBtn.anchoredPosition.x);
    }


    public void SwipeControl()
    {
        if (Input.touchCount == 1) 
        {
            Touch touch = Input.GetTouch(0); 
            if (touch.phase == TouchPhase.Began) 
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) 
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) 
            {
                lp = touch.position;  

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   
                        if ((lp.x > fp.x))  
                        {
                            if (rightBtn.anchoredPosition.x > 0)
                            {
                                Debug.Log("Right Anwser");
                                uiManager.StartCoroutine(nameof(UiManager.Next));
                                quizHandler.StartCoroutine(nameof(quizHandler.Next));
                                timer.startTimer = false;//.ResetTimer();
                                RightAns();
                                Debug.Log("If Right Anwser : " + DataBase.RightAnswer);
                            }
                            else
                            {
                                Debug.Log("Dumb Anwser");
                                uiManager.StartCoroutine(nameof(UiManager.WrongAns));
                                quizHandler.StartCoroutine(nameof(quizHandler.WrongAns));
                                WrongAns();
                                Debug.Log("If Dumb Anwser : " + DataBase.WrongAnswer);
                            }
                        }
                        else
                        {
                            if (rightBtn.anchoredPosition.x < 0)
                            {
                                Debug.Log("Right Anwser");
                                uiManager.StartCoroutine(nameof(UiManager.Next));
                                quizHandler.StartCoroutine(nameof(quizHandler.Next));
                                timer.startTimer = false;//.ResetTimer();
                                RightAns();
                                Debug.Log("Else Right Anwser : " + DataBase.RightAnswer);
                            }
                            else
                            {
                                Debug.Log("Dumb Anwser");
                                uiManager.StartCoroutine(nameof(UiManager.WrongAns));
                                quizHandler.StartCoroutine(nameof(quizHandler.WrongAns));
                                WrongAns();
                                Debug.Log("Else Dumb Anwser : " + DataBase.WrongAnswer);
                            }
                        }
                    }
                   
                }
                else
                {   
                    Debug.Log("Tap");
                }
            }
        }
    }

    void RightAns() 
    {
        DataBase.Questions += 1;
        DataBase.RightAnswer += 1;
        DataSaver.Instance.SaveData();
    }

    void WrongAns()
    {
        DataBase.Keys += 10;
        DataBase.Lives -= 1;
        DataBase.Questions += 1;
        DataBase.WrongAnswer += 1;
        DataSaver.Instance.SaveData();
    }



    void Update()
    {
        SwipeControl();
    }
}