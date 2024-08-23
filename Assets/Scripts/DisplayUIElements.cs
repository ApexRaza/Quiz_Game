using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUIElements : MonoBehaviour
{
    public TextMeshProUGUI[] gemsTxt, keysTxt, dollarsTxt, livesTxt;
    public TextMeshProUGUI usernameTxt;

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
            t.text = DataBase.Gems.ToString();
        }

        foreach (TextMeshProUGUI t in keysTxt)
        {
            t.text = DataBase.Keys.ToString();
        }

        foreach (TextMeshProUGUI t in dollarsTxt)
        {
            t.text = DataBase.Dollars.ToString();
        }

        foreach (TextMeshProUGUI t in livesTxt)
        {
            t.text = DataBase.Lives.ToString();
        }

        usernameTxt.text = DataBase.UserName;


    }

   
  



}
