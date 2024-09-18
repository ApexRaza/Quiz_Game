using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgressState : MonoBehaviour
{

    public int stateNb =0;

  //  public TextMeshProUGUI prizeTxt;
    public Image prizeImage;

    public Image[] q_State;



    public Sprite grayBtn, blueBtn, greenBtn, redBtn,chest,tick ,cross;

  public  bool[] answer = new bool[8];

    private void Start()
    {
       SetStateFirstTime();
    }


    void SetStateFirstTime()
    {
        foreach (var state in q_State)
        {
            state.gameObject.SetActive(false);
        }

        for (int i = 0; i < DataBase.QuestionsToTreasure; i++)
        {
            q_State[i].gameObject.SetActive(true);
        }
    }


    public void UpdateState(bool b)
    {
        answer[stateNb] = b;
        stateNb++;
       

        for (int i = 0; i < stateNb; i++)
        {
            if (answer[i] == false)
            {
                q_State[i].sprite = redBtn;
                q_State[i].transform.GetChild(0).gameObject.SetActive(true);
                q_State[i].transform.GetChild(1).gameObject.SetActive(false);
                q_State[i].transform.GetChild(0).GetComponent<Image>().sprite = cross;
            }
            else if (answer[i] == true)
            {
                q_State[i].sprite = greenBtn;
                q_State[i].transform.GetChild(0).gameObject.SetActive(true);
                q_State[i].transform.GetChild(1).gameObject.SetActive(false);
                q_State[i].transform.GetChild(0).GetComponent<Image>().sprite = tick;
            }
            else
            {
                q_State[i].sprite = grayBtn;
                q_State[i].transform.GetChild(0).gameObject.SetActive(false);
       
            }
            
        }
        if (q_State.Length > stateNb)
        {
            q_State[stateNb].sprite = blueBtn;
            q_State[stateNb].transform.GetChild(0).gameObject.SetActive(false);
        }

        if (stateNb >= DataBase.QuestionsToTreasure)
        {
            stateNb = 0;
          //  prizeTxt.text = "Prize Unlocked  (Open for 40 Keys)";
            prizeImage.sprite = chest;
           
        }

    }

    public void GetTreasure()
    {
        DataBase.Keys -= 40;
        TreasureSystem.Instance.CalculatePercentage(TreasureType.Low);
    }



}
