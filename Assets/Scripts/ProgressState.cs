using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgressState : MonoBehaviour
{

    public int stateNb =0;

    public TextMeshProUGUI prizeTxt;
    public Image prizeImage;

    public Image[] q_State;

   

    public void UpdateState()
    {
        stateNb++;

        for (int i = 0; i < stateNb; i++)
        {
            q_State[i].color = Color.green;
        }

        if (stateNb >= DataBase.QuestionsToTreasure)
        {
            stateNb = 0;
            prizeTxt.text = "Prize Unlocked";
            prizeImage.color = Color.green;
        }




    }


}
