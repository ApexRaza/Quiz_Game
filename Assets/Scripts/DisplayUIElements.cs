using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUIElements : MonoBehaviour
{

    public TextMeshProUGUI gemsTxt, keysTxt, dollarsTxt, livesTxt;

    
    public GameObject bottomBar, middlePanel;



    private void Start()
    {
        Invoke(nameof(UpdateUI), 1);
    }


    public void UpdateUI()
    {
        gemsTxt.text = "Gems: " + DataBase.Gems.ToString();
        keysTxt.text = "Keys: " + DataBase.Keys.ToString();
        dollarsTxt.text = "Dollars: " + DataBase.Dollars.ToString();
        livesTxt.text = "Lives: " + DataBase.Lives.ToString();
    }

    public void ManageMiddlePanels(GameObject panelObj)
    {
        foreach (Transform t in middlePanel.transform)
        {
            if (t.gameObject == panelObj)
            {
                t.gameObject.SetActive(true);
            }
            else 
            {
                t.gameObject.SetActive(false);
            }
        }
    }
   
    public void ButtonEffect(Transform btn)
    {
        
        foreach (Transform t in bottomBar.transform)
        {
            if (t == btn)
            {
                btn.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 50, 0);
            }
            else
            {
                t.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            }
        }
    }



}
