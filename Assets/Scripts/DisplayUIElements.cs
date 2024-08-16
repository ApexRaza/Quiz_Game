using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUIElements : MonoBehaviour
{

 

    public TextMeshProUGUI[] gemsTxt, keysTxt, dollarsTxt, livesTxt;

    public GameObject bottomBar, middlePanel;

    

    private void Start()
    {
        Invoke(nameof(UpdateUI), 1);
    }



    private void Update()
    {
        UpdateUI();
    }



    public void UpdateUI()
    {

        foreach (TextMeshProUGUI t in gemsTxt)
        {
            t.text = "Gems: " + DataBase.Gems.ToString();
        }

        foreach (TextMeshProUGUI t in keysTxt)
        {
            t.text = "Keys: " + DataBase.Keys.ToString();
        }

        foreach (TextMeshProUGUI t in dollarsTxt)
        {
            t.text = "Dollars: " + DataBase.Dollars.ToString();
        }

        foreach (TextMeshProUGUI t in livesTxt)
        {
            t.text = "Lives: " + DataBase.Lives.ToString();
        }



       
    }

   
  



}
