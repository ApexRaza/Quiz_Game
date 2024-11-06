using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSwipe : MonoBehaviour
{
    private Vector3 fp;
    private Vector3 lp;
    private float dragDistance;

    public NetworkQuizHandler networkQuizHandler;
    public TImer timer;

    public RectTransform rightBtn;

    public AudioSource swipeAudioSource;
    
    void Start()
    {
        dragDistance = Screen.height * 15 / 100;
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
                                // Debug.Log("Right Anwser");

                                networkQuizHandler.CheckAns("TRUE");
                                timer.startTimer = false;//.ResetTimer();

                                // Debug.Log("If Right Anwser : " + DataBase.RightAnswer);
                            }
                            else
                            {
                                // Debug.Log("Dumb Anwser");
                                timer.startTimer = false;
                                networkQuizHandler.CheckAns("FALSE");
                                //  Debug.Log("If Dumb Anwser : " + DataBase.WrongAnswer);
                            }
                        }
                        else
                        {
                            if (rightBtn.anchoredPosition.x < 0)
                            {
                                //  Debug.Log("Right Anwser");

                                networkQuizHandler.CheckAns("TRUE");
                                timer.startTimer = false;//.ResetTimer();

                                // Debug.Log("Else Right Anwser : " + DataBase.RightAnswer);
                            }
                            else
                            {
                                // Debug.Log("Dumb Anwser");
                                timer.startTimer = false;
                                networkQuizHandler.CheckAns("FALSE");
                                // Debug.Log("Else Dumb Anwser : " + DataBase.WrongAnswer);
                            }
                        }
                    }

                }
                else
                {
                    //Debug.Log("Tap");
                }
            }
        }
    }

    void Update()
    {
        SwipeControl();
    }
}
