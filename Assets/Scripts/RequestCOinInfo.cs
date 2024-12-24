
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestCOinInfo : MonoBehaviour
{
    public TradeUIManager tradeUiManager;
    public CollectionsSO collectionSO;
    public Transform parentCoin, parentCount, askParent, askCountParent;
    public List<GameObject> coinObj, countObj, askCoinObj, askCount;
    public TextMeshProUGUI headerName, askName;

    public bool demanded;  



    private void OnEnable()
    {
        InitializeLists();
        CoinValue();
        headerName.text = tradeUiManager.userName;
        askName.text = "Demanded Coins (" + tradeUiManager.userName + "'s Own)";
    }

    void InitializeLists()
    {
        coinObj.Clear();
        foreach (Transform t in parentCoin)
        {
            coinObj.Add(t.gameObject);
        }

        countObj.Clear();
        foreach (Transform t in parentCount)
        {
            countObj.Add(t.gameObject);
        }

        askCoinObj.Clear();
        foreach (Transform t in askParent)
        {
            askCoinObj.Add(t.gameObject);
        }

        askCount.Clear();
        foreach (Transform t in askCountParent)
        {
            askCount.Add(t.gameObject);
        }

    }



    void CoinValue()
    {
        for (int i = 0; i < 5; i++)
        {
            coinObj[i].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tradeUiManager.coinCount[i];
            askCoinObj[i].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DataBase.GetCoins(int.Parse(tradeUiManager.GetIndex(DataBase.LevelUp, (i + 1)))).ToString();

           

            askCount[i].GetComponent<CountTradeValue>().countValue = tradeUiManager.demandedData[i].value;
            if (tradeUiManager.demandedData[i].value > 0)
            {
                askCount[i].transform.GetChild(3).gameObject.SetActive(true);
            }
            else 
            {
                askCount[i].transform.GetChild(3).gameObject.SetActive(false);
            }


            countObj[i].GetComponent<CountTradeValue>().countValue = tradeUiManager.proposedData[i].value;
            if (tradeUiManager.proposedData[i].value > 0)
            {
                countObj[i].transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                countObj[i].transform.GetChild(3).gameObject.SetActive(false);
            }



            coinObj[i].transform.GetChild(0).GetComponent<Image>().sprite = collectionSO.collectionData[0].item[i].Icon;
            askCoinObj[i].transform.GetChild(0).GetComponent<Image>().sprite = collectionSO.collectionData[0].item[i].Icon;

        }

        //for (int i = 0; i < 5; i++)
        //{
        //    coinObj[i].transform.GetChild(0).GetComponent<Image>().sprite = collectionSO.collectionData[0].item[i].Icon;
        //    askCoinObj[i].transform.GetChild(0).GetComponent<Image>().sprite = collectionSO.collectionData[0].item[i].Icon;
        //}


    }


}
