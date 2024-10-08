using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinInfoDisplay : MonoBehaviour
{

    public Image icon;
    public TextMeshProUGUI nameTxt;

    public void UpdateInfo(GameObject obj)
    {
        this.gameObject.SetActive(true);
        icon.sprite = obj.transform.GetChild(0).GetComponent<Image>().sprite;
        nameTxt.text = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
    }


}
