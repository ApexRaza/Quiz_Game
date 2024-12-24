using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeCOinInfo : MonoBehaviour
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



    void UpdateProposedData()
    {
        for (int i = 0; i < 5; i++)
        {
            tradeUiManager.proposedData[i].coinIndex = int.Parse(tradeUiManager.indexCount[i]);
            tradeUiManager.proposedData[i].value = parentCount.transform.GetChild(i).GetComponent<CountTradeValue>().countValue;
        }
    }


    void UpdateDemandedData()
    {
        for (int i = 0; i < 5; i++)
        {
            tradeUiManager.demandedData[i].coinIndex = int.Parse(tradeUiManager.indexCount[i]);
            tradeUiManager.demandedData[i].value = askCountParent.transform.GetChild(i).GetComponent<CountTradeValue>().countValue;
        }
    }



    private void Update()
    {
        UpdateDemandedData();
        UpdateProposedData();
    }

    void CoinValue()
    {
        for (int i = 0; i < 5; i++)
        {
            coinObj[i].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DataBase.GetCoins(int.Parse(tradeUiManager.indexCount[i])).ToString();

            askCoinObj[i].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tradeUiManager.coinCount[i];
            askCount[i].GetComponent<CountTradeValue>().totalCoin = int.Parse(tradeUiManager.coinCount[i]);
            countObj[i].GetComponent<CountTradeValue>().totalCoin = DataBase.GetCoins(int.Parse(tradeUiManager.indexCount[i]));
            new CoinData(0, 0);

            // coinObj[i].transform.GetChild(1).GetComponent<Image>().sprite = 
            Debug.Log(tradeUiManager.coinCount[i]);
            Debug.Log(tradeUiManager.indexCount[i]);

        }

        for (int i = 0; i < 5; i++)
        {
            coinObj[i].transform.GetChild(0).GetComponent<Image>().sprite = collectionSO.collectionData[0].item[i].Icon;
            askCoinObj[i].transform.GetChild(0).GetComponent<Image>().sprite = collectionSO.collectionData[0].item[i].Icon;
        }


    }





}
