using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
public enum Products 
{
    LowTreasure,
    MidTreasure,
    GoldTreasure,
    KeyPacks,
    Tips,
    DollarsPack1,
    DollarsPack2,
    DollarsPack3,
    DollarsPack4
}

public class Shop : MonoBehaviour
{
    public Products product;
    public int reqCost;
    public int amount;
    private int totalMoney;
    private Button button;
    public UnityEvent shopEvent;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(BuyProduct);
    }
    public void BuyProduct() 
    {
        switch (product) 
        {
            case Products.LowTreasure:
                totalMoney = DataBase.Keys;
                if (totalMoney >= reqCost)
                {
                    TreasureSystem.Instance.CalculatePercentage(treasureType:TreasureType.Low);
                    totalMoney -= reqCost;
                    DataBase.Keys = totalMoney;
                }
                else 
                {
                    //shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

            case Products.MidTreasure:
                totalMoney = DataBase.Keys;
                if (totalMoney >= reqCost)
                {
                    TreasureSystem.Instance.CalculatePercentage(treasureType: TreasureType.Medium); 
                    totalMoney -= reqCost;
                    DataBase.Keys = totalMoney;
                }
                else
                {
                    //shopEvent.Invoke();
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
                    //shopEvent.Invoke();
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
                    //shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
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
                    //shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

            case Products.DollarsPack1:
                totalMoney = DataBase.Gems;
                if (totalMoney >= reqCost)
                {
                    DataBase.Dollars += amount;
                    totalMoney -= reqCost;
                    DataBase.Gems = totalMoney;
                }
                else
                {
                    //shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

            case Products.DollarsPack2:
                totalMoney = DataBase.Gems;
                if (totalMoney >= reqCost)
                {
                    DataBase.Dollars += amount;
                    totalMoney -= reqCost;
                    DataBase.Gems = totalMoney;
                }
                else
                {
                    //shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

            case Products.DollarsPack3:
                totalMoney = DataBase.Gems;
                if (totalMoney >= reqCost)
                {
                    DataBase.Dollars += amount;
                    totalMoney -= reqCost;
                    DataBase.Gems = totalMoney;
                }
                else
                {
                    //shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;

            case Products.DollarsPack4:
                totalMoney = DataBase.Gems;
                if (totalMoney >= reqCost)
                {
                    DataBase.Dollars += amount;
                    totalMoney -= reqCost;
                    DataBase.Gems = totalMoney;
                }
                else
                {
                    //shopEvent.Invoke();
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;
        }
    }
}
