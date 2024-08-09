using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreasureSystem : MonoBehaviour
{
    //public TextMeshProUGUI R1txt, R1Divtxt,R2txt, R3txt, R1Coinstxt, R2Coinstxt, R3Coinstxt, gemsTxt, dollarsTxt, packGemsTxt, packDollarsTxt;
    private int R1Coins, R1_1Coins, R1_2Coins, R1_3Coins, R2Coins, R3Coins, packGems, packDollars;
    private int R1Col, R2Col = 4, R3Col =5;

    public enum TreasureType
    {
        Free,
        Low,
        Medium,
        Gold
    }
    [System.Serializable]
    public class coinDistributionPercentage
    {
        public float R1Chance;
        public float R2Chance;
        public float R3Chance;

        public coinDistributionPercentage(float r1, float r2, float r3)
        {
            R1Chance = r1;
            R2Chance = r2;
            R3Chance = r3;
        }
    }

    private Dictionary<TreasureType, coinDistributionPercentage> treasureCoinDistributionPercntage;
    // Start is called before the first frame update
    void Start()
    {
        treasureCoinDistributionPercntage = new Dictionary<TreasureType, coinDistributionPercentage>
        {
            {TreasureType.Free, new coinDistributionPercentage(100f, 40f, 20f)},
            {TreasureType.Low, new coinDistributionPercentage(100f, 40f, 20f)},
            {TreasureType.Medium, new coinDistributionPercentage(100f, 60f, 30f)},
            {TreasureType.Gold, new coinDistributionPercentage(100f, 80f, 40f)}
        };
        //test
        //calculatePercentage(TreasureType.Free);
        //testend
    }
    public void testFun(int i) 
    {
        if(i == 0)
            calculatePercentage(TreasureType.Free);
        else if (i == 1)
            calculatePercentage(TreasureType.Medium);
        else if (i == 2)
            calculatePercentage(TreasureType.Gold);
    }

    public int getIndex(int R, int C)
    {
        int num1;
        num1 = ((R - 1) * 5) + (C - 1);
        return num1;
    }
    public void calculatePercentage(TreasureType treasureType)
    {
        if (treasureCoinDistributionPercntage.TryGetValue(treasureType, out coinDistributionPercentage coinDistributionPercentage))
        {
            Debug.Log("Here");
            
            //DataSaver.Instance.loadData();
            bool r1Chance = checkChance(coinDistributionPercentage.R1Chance);
            bool r2Chance = checkChance(coinDistributionPercentage.R2Chance);
            bool r3Chance = checkChance(coinDistributionPercentage.R3Chance);
            Debug.Log(r1Chance ? "R1 Awarded" : "R1 not awarded");
            //R1txt.text = r1Chance ? "R1 Awarded" : "R1 not awarded";
            Debug.Log(r2Chance ? "R2 Awarded" : "R2 not awarded");
            //R2txt.text = r2Chance ? "R2 Awarded" : "R2 not awarded";
            Debug.Log(r3Chance ? "R3 Awarded" : "R3 not awarded");
            //R3txt.text = r3Chance ? "R3 Awarded" : "R3 not awarded";


            var r1CoinsRange = getR1Coins(treasureType);
            R1Coins = r1Chance ? Random.Range(r1CoinsRange.min, r1CoinsRange.max+1) : 0;
            //R1Coinstxt.text = "Amount of R1 coins is: " + R1Coins.ToString();
            (R1_1Coins, R1_2Coins, R1_3Coins)=distributeCoins(R1Coins);

            Debug.Log("R1Col: " + R1Col);
            DataBase.SetCoins(getIndex(DataBase.LevelUp, R1Col), R1Coins);
            Debug.Log("R1 Coins Index: " + getIndex(DataBase.LevelUp, R1Col));
            Debug.Log("1st Box Coins: " + R1_1Coins + " 2nd Box Coins: " + R1_2Coins + " 3rd Box Coins: " + R1_3Coins);
            //R1Divtxt.text = "1st Box Coins: " + R1_1Coins + " 2nd Box Coins: " + R1_2Coins + " 3rd Box Coins: " + R1_3Coins;

            var r2CoinsRange = getR2Coins(treasureType);
            R2Coins = r2Chance ? Random.Range(r2CoinsRange.min, r2CoinsRange.max+1) : 0;
            DataBase.SetCoins(getIndex(DataBase.LevelUp, R2Col), R2Coins);
            Debug.Log("R2 Coins Index: " + getIndex(DataBase.LevelUp, R2Col));
            Debug.Log("Amount of R2 coins is: " + R2Coins.ToString());
            //R2Coinstxt.text = "Amount of R2 coins is: " + R2Coins.ToString();

            var r3CoinsRange = getR3Coins(treasureType);
            R3Coins = r3Chance ? Random.Range(r3CoinsRange.min, r3CoinsRange.max+1) : 0;
            DataBase.SetCoins(getIndex(DataBase.LevelUp, R3Col), R3Coins);
            Debug.Log("R3 Coins Index: " + getIndex(DataBase.LevelUp, R3Col));
            Debug.Log("Amount of R3 coins is: " + R3Coins.ToString());
            //R3Coinstxt.text = "Amount of R3 coins is: " + R3Coins.ToString();

            var gemsRange = getGems(treasureType);
            packGems = Random.Range(gemsRange.min, gemsRange.max);
            DataBase.Gems = DataBase.Gems + packGems;
            //gemsTxt.text = "Amount of Gems is: " + DataBase.Gems.ToString();
            //packGemsTxt.text = "Amount of Gems in pack is:" + packGems.ToString();

            var dollarsRange = getDollars(treasureType);
            packDollars = Random.Range(dollarsRange.min, dollarsRange.max);
            DataBase.Dollars = DataBase.Dollars + packDollars;
            //dollarsTxt.text = "Amount of Dollars is: " + DataBase.Dollars.ToString();
            //packDollarsTxt.text = "Amount of Dollars in pack is:" + packDollars.ToString();

            DataSaver.Instance.SaveData();

            CheckAndLevelUp(DataBase.LevelUp);
            CheckExtra(DataBase.LevelUp);
        }
        else
        {
            Debug.Log("invalid data type");
        }
    }
    
    private bool checkChance(float chance)
    {
        return UnityEngine.Random.Range(0f, 100f) < chance;
    }
    private (int, int, int) distributeCoins(int r1CoinsAmount) 
    {
        int r1_1 = 0;
        int r1_2 = 0; 
        int r1_3 = 0;
        switch(Random.Range(0, 3)) 
        {
            case 0:
                r1_1 = r1CoinsAmount;
                R1Col = 1;
                break;
            case 1:
                r1_2 = r1CoinsAmount;
                R1Col = 2;
                break;
            case 2:
                r1_3 = r1CoinsAmount;
                R1Col = 3;
                break;
        }
        return(r1_1, r1_2, r1_3) ;
    }
    private (int min, int max) getR1Coins(TreasureType treasureType)
    {
        return treasureType
            switch
        {
            TreasureType.Free => (2, 6),
            TreasureType.Low => (2, 6),
            TreasureType.Medium => (4, 12),
            TreasureType.Gold => (10, 20),
            _ => (0, 1)
        };
    }
    private (int min, int max) getR2Coins(TreasureType treasureType)
    {
        return treasureType
            switch
        {
            TreasureType.Free => (2, 6),
            TreasureType.Low => (2, 6),
            TreasureType.Medium => (6, 12),
            TreasureType.Gold => (6, 12),
            _ => (0, 1)
        };
    }
    private (int min, int max) getR3Coins(TreasureType treasureType)
    {
        return treasureType
            switch
        {
            TreasureType.Free => (2, 2),
            TreasureType.Low => (2, 2),
            TreasureType.Medium => (2, 6),
            TreasureType.Gold => (6, 12),
            _ => (0, 1)
        };
    }
    private (int min, int max) getGems(TreasureType treasureType)
    {
        return treasureType
            switch
        {
            TreasureType.Free => (3, 5),
            TreasureType.Low => (3, 5),
            TreasureType.Medium => (5, 8),
            TreasureType.Gold => (10, 20),
            _ => (0, 1)
        };
    }
    private (int min, int max) getDollars(TreasureType treasureType)
    {
        return treasureType
            switch
        {
            TreasureType.Free => (10, 20),
            TreasureType.Low => (10, 20),
            TreasureType.Medium => (20, 40),
            TreasureType.Gold => (200, 400),
            _ => (0, 1)
        };
    }

    private void CheckAndLevelUp(int level) 
    { 
        int num = 0;
        for(int i = 1; i <= 5; i++) 
        {
            if(DataBase.GetCoins(getIndex(level, i)) >= 10)
            {
                num++;
            }
        }
        
        if(num == 5) 
        {
            
            LevelUpScript.Instance.LevelUp();
        }
    }

    public void CheckExtra(int level) 
    {
        int num = 0;
        int extra = 0;
        for (int i = 1; i <= 5; i++) 
        {
            num = DataBase.GetCoins(getIndex(level, i));
            extra = 0;
            for(int j = 10; j < num; j++) 
            {
                extra++;
            }
            Debug.Log("Extra Coins for trade index: " + getIndex(level, i) + " Value " + extra);
        }
    }
}
