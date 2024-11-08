using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestReward : MonoBehaviour
{
    public coinReward dollarReward;
    public coinReward diamondReward;
    public GameObject dollar;
    public TextMeshProUGUI dollarTxt;
    public GameObject diamond;
    public TextMeshProUGUI diamondText;
    public GameObject[] coins;
    public Image[] coinsImgs;
    public TextMeshProUGUI[] coinText;
    public RewardInfo RewardInfo;


    private void OnEnable()
    {
        dollarTxt.text = RewardInfo.dollars.ToString();
        diamondText.text = RewardInfo.gems.ToString();
        for (int i = 0; i < coinsImgs.Length; i++)
        {
            coinsImgs[i].sprite = RewardInfo.itemInfo[i].Icon;
            coinText[i].text = RewardInfo.itemInfo[i].Amount.ToString();
        }
        callFunctions();
    }


    private void OnDisable()
    {
        dollar.SetActive(true);
        diamond.SetActive(true);

        for (int i = 1; i < coins.Length; i++)
        {

            coins[i].SetActive(false);

        }
        for (int i = 0; i < coinText.Length; i++) 
        {
            coinText[i].gameObject.SetActive(false);
        }
    }
    public void DollarRewardCall() 
    {
        dollarReward.RewardCoins();
        dollar.SetActive(false);
    }
    public void DiamondRewardCall() 
    {
        diamondReward.RewardCoins();    
        diamond.SetActive(false);
    }

    public void CoinsCall() 
    {
        for (int i = 0; i < coins.Length; i++)
        {
            if (RewardInfo.itemInfo[i].Amount > 0)
            {
                coins[i].SetActive(true);
                coinText[i].gameObject.SetActive(true);
            }
            else 
            {
                coins[i].SetActive(false);
                coinText[i].gameObject.SetActive(false);
            }
        }
    }

    public void callFunctions() 
    {
        StartCoroutine(mainCall());
    }

    public float num;
    IEnumerator mainCall() 
    {

        yield return new WaitForSeconds(num);

        DollarRewardCall();
        yield return new WaitForSeconds(1f);
        DiamondRewardCall();
        yield return new WaitForSeconds(1.5f);
        CoinsCall();
    }
}
