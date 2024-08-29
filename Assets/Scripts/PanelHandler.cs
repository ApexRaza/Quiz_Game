using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHandler : MonoBehaviour
{
    public GameObject[] onPanel, offPanel;


    public void OpenPanel()
    {
        foreach (GameObject panel in onPanel)
        {
            panel.SetActive(true);
        }


        foreach (GameObject panel in offPanel)
        {
            panel.SetActive(false);
        }

    }
}
