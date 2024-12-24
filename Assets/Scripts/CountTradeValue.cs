using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountTradeValue : MonoBehaviour
{

    public TextMeshProUGUI valueTxt;
    public Button plusBtn, minusBtn;
   public int countValue = 0;
    public int totalCoin;
    // Start is called before the first frame update
    void Start()
    {
        valueTxt.text = countValue.ToString();
        if (countValue <= 0)
        {
            minusBtn.interactable = false;
        }
        if (countValue >= totalCoin)
        {
            plusBtn.interactable = false;
        }
    }

    public void PlusBtn()
    {
        if (countValue < totalCoin)
        {
            countValue++;
            valueTxt.text = countValue.ToString();
        }
        if (countValue >= totalCoin)
        {
            plusBtn.interactable = false;
        }
        if (countValue > 0)
        {
            minusBtn.interactable = true;
        }
    }

    public void MinusBtn()
    {
        if (countValue > 0)
        {
            countValue--;
            valueTxt.text = countValue.ToString();
        }


        if (countValue < totalCoin)
        {
            plusBtn.interactable = true;
        }

        if (countValue <= 0)
        {
            minusBtn.interactable = false;
        }
    }
}

