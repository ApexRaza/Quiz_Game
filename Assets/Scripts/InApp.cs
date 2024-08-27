using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InApp : MonoBehaviour
{
    public void GemPacks(int amount) 
    {
        DataBase.Gems += amount;
    }

    public void RemoveAds() 
    {
        //Implement Remove Ads
    }
}
