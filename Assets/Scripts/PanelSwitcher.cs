using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour
{


    public bool isUp;
    public ButtonPanelPair[] buttonPanelPairs;

  
    void Start()
    {
  
        AddBtnListener();
    }


    // Method to add listeners to each button
    private void AddBtnListener()
    {
        foreach (ButtonPanelPair pair in buttonPanelPairs)
        {
            Button button = pair.button;
            GameObject panel = pair.panel;

            if (button != null && panel != null)
            {
                button.onClick.AddListener(() =>
                {
                    ManagePanels(panel);
                    ButtonEffect(button);
                    });
            }
        }
    }

    // Method to manage the visibility of panels
    private void ManagePanels(GameObject panelObj)
    {
        foreach (ButtonPanelPair pair in buttonPanelPairs)
        {
            GameObject panel = pair.panel;
            if (panel != null)
            {
                panel.SetActive(panel == panelObj);
            }
        }

        Debug.Log("Panels managed");
    }


    public void ButtonEffect(Button clickedButton)
    {

        foreach (ButtonPanelPair pair in buttonPanelPairs)
        {
            RectTransform rectTransform = pair.button.transform.GetChild(0).GetComponent<RectTransform>();
            
            if (pair.button == clickedButton)
            {
                rectTransform.anchoredPosition = new Vector3(0, isUp ? 50 : -50, 0);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector3(0, 0, 0);
            }
        }
    }




}



[System.Serializable]
public class ButtonPanelPair
{
    public string Name;
    public Button button;
    public GameObject panel;
}