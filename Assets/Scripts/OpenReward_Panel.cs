using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenReward_Panel : MonoBehaviour
{
    public GameObject[] RewardPanels;

    public void OpenReward(int i)
    {
        for (int j = 0; j < RewardPanels.Length; j++)
        {
            if (i == j)
                RewardPanels[j].SetActive(true);
            else
                RewardPanels[j].SetActive(false);

           
        }

       
        TreasureSystem.Instance.CallTreasure(i);
        
    }

    public void KeyConsume(int num)
    {
        DataBase.Keys -= num;
    }

   



}
