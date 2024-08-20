using UnityEngine.UI;
using UnityEngine;

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
    private int totalMoney;
    private Button button;
    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(buyProduct);
    }
    public void buyProduct() 
    {
        switch (product) 
        {
            case Products.LowTreasure:
                totalMoney = DataBase.Keys;
                if (totalMoney >= reqCost)
                {
                    TreasureSystem.Instance.callTreasure(1);
                    totalMoney -= reqCost;
                    DataBase.Keys = totalMoney;
                }
                else 
                {
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;
            case Products.MidTreasure:
                totalMoney = DataBase.Keys;
                if (totalMoney >= reqCost)
                {
                    TreasureSystem.Instance.callTreasure(2);
                    totalMoney -= reqCost;
                    DataBase.Keys = totalMoney;
                }
                else
                {
                    Debug.Log("Not Enough Keys to buy Treasure");
                }
                break;
        }
    }
}
