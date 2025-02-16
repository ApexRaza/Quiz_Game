using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public enum Products 
{
    TreasureMultiplier,
    RewardedTreasure,
    RewardedKeys,
    RewardedDollars,
    RewardedGems,
    RewardedLife,
    LowTreasure,
    MidTreasure,
    GoldTreasure,
    KeyPacks,
    Tips,
    DollarsPack,
    Lives
}

public class Shop : MonoBehaviour
{
    private static Shop instance;
    public static Shop Instance;

    public Products product;
    public int reqCost;
    public int amount;
    private int totalMoney;
    private Button button;
    public TextMeshProUGUI shopText;
    public UnityEvent shopEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Instance = instance;
        }
    }
    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => BuyProduct(product));
    }
    public void BuyProduct(Products products) 
    {
        switch (products) 
        {
            case Products.RewardedTreasure:
                UnityAds.Instance.CurrentProduct = transform;
                UnityAds.Instance.ShowRewardedAd();
                //shopEvent.Invoke();
                break;

            case Products.TreasureMultiplier:
                TreasureSystem.Instance.TreasureMultiplier();
                //shopEvent.Invoke();
                break;

            case Products.LowTreasure:
                totalMoney = DataBase.Gems;
                if (totalMoney >= reqCost)
                {
                    TreasureSystem.Instance.CalculatePercentage(treasureType:TreasureType.Low);
                    totalMoney -= reqCost;
                    DataBase.Keys = totalMoney;
                }
                else 
                {
                    shopText.text = "Not Enough Gems to buy Treasure";
                    shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

            case Products.MidTreasure:
                totalMoney = DataBase.Gems;
                if (totalMoney >= reqCost)
                {
                    TreasureSystem.Instance.CalculatePercentage(treasureType: TreasureType.Medium); 
                    totalMoney -= reqCost;
                    DataBase.Keys = totalMoney;
                }
                else
                {
                    shopText.text = "Not Enough Gems to buy Treasure";
                    shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

            case Products.GoldTreasure:
                totalMoney = DataBase.Gems;
                if (totalMoney >= reqCost)
                {
                    TreasureSystem.Instance.CalculatePercentage(treasureType: TreasureType.Gold); 
                    totalMoney -= reqCost;
                    DataBase.Gems = totalMoney;
                }
                else
                {
                    shopText.text = "Not Enough Gems to buy Treasure";
                    shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

            case Products.KeyPacks:
                totalMoney = DataBase.Dollars;
                if (totalMoney >= reqCost)
                {
                    DataBase.Keys += amount;
                    totalMoney -= reqCost;
                    DataBase.Dollars = totalMoney;
                }
                else
                {
                    shopText.text = "Not Enough Dollars to buy Key Pack";
                    shopEvent.Invoke();
                    Debug.Log("Not Enough Coins to buy Key Pack");
                }
                break;

            case Products.RewardedKeys:
                UnityAds.Instance.CurrentProduct = transform;
                UnityAds.Instance.ShowRewardedAd();
                break;

            case Products.Tips:
                totalMoney = DataBase.Dollars;
                if (totalMoney >= reqCost)
                {
                    //Do something about tip
                    totalMoney -= reqCost;
                    DataBase.Dollars = totalMoney;
                }
                else
                {
                    shopText.text = "Not Enough Dollars to buy Tips";
                    shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

            case Products.DollarsPack:
                totalMoney = DataBase.Gems;
                if (totalMoney >= reqCost)
                {
                    DataBase.Dollars += amount;
                    totalMoney -= reqCost;
                    DataBase.Gems = totalMoney;
                }
                else
                {
                    shopText.text = "Not Enough Gems to buy Dollars Pack";
                    shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

            case Products.RewardedDollars:
                UnityAds.Instance.CurrentProduct = transform;
                UnityAds.Instance.ShowRewardedAd();
                break;

            case Products.RewardedGems:
                UnityAds.Instance.CurrentProduct = transform;
                UnityAds.Instance.ShowRewardedAd();
                break;

            case Products.RewardedLife:
                UnityAds.Instance.CurrentProduct = transform;
                UnityAds.Instance.ShowRewardedAd();
                break;
            case Products.Lives:
               
                totalMoney = DataBase.Gems;
                if (totalMoney >= reqCost)
                {
                    DataBase.Lives += amount;
                    totalMoney -= reqCost;
                    DataBase.Gems = totalMoney;
                }
                else
                {
                    shopText.text = "Not Enough Gems to buy Live";
                    shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

        }
        DataSaver.Instance.SaveData();
    }

    public void BuyProductOnReward()
    {
        switch (product)
        {
            case Products.RewardedTreasure:
                TreasureSystem.Instance.CalculatePercentage(treasureType: TreasureType.Free);
                //shopEvent.Invoke();
                break;  

            case Products.RewardedKeys:
                amount = Random.Range(300, 450);
                DataBase.Keys += amount;
                //shopEvent.Invoke();
                break;

            case Products.RewardedDollars:
                amount = Random.Range(100, 200);
                DataBase.Dollars += amount;
                //shopEvent.Invoke();
                break;

            case Products.RewardedGems:
                amount = Random.Range(3, 6);
                DataBase.Gems += amount;
                break;

            case Products.RewardedLife:
                amount = 1;
                DataBase.Lives += amount;
                break;
          
        }
        DataSaver.Instance.SaveData();
    }
}
