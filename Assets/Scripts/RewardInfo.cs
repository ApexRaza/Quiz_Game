using System;
using UnityEngine;

[CreateAssetMenu(menuName = "QuizGame/RewardInfo", fileName = "RewardInfo")]
public class RewardInfo : ScriptableObject
{
    public ItemsInfo[] itemInfo;
    public CollectionsSO collectionsSO;

    public int coins, gems;

    public void GetCoin1Info(int col, int amount)
    {
        itemInfo[0].Name = collectionsSO.collectionData[0].item[col].Name;
        itemInfo[0].Amount = amount;
        itemInfo[0].Icon = collectionsSO.collectionData[0].item[col].Icon;
    }
    public void GetCoin2Info(int col, int amount)
    {
        itemInfo[1].Name = collectionsSO.collectionData[0].item[col].Name;
        itemInfo[1].Amount = amount;
        itemInfo[1].Icon = collectionsSO.collectionData[0].item[col].Icon;
    }
    public void GetCoin3Info(int col, int amount)
    {
        itemInfo[2].Name = collectionsSO.collectionData[0].item[col].Name;
        itemInfo[2].Amount = amount;
        itemInfo[2].Icon = collectionsSO.collectionData[0].item[col].Icon;
    }
    public void GetDollarInfo(int amount)
    {
        coins = amount;
    }
    public void GetGemsInfo(int amount)
    {
        gems = amount;
    }

}
[Serializable]
public class ItemsInfo
{
    public string Name;
    
    public int Amount;

    public Sprite Icon;
}