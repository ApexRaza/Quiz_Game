using System.Collections.Generic;
using UnityEngine;
using System;

public class DataBase 
{
    public static int Dollars
    {
        get 
        {
            return PlayerPrefs.GetInt("Dollars",80); 
        }
        set
        {
            PlayerPrefs.SetInt("Dollars", value);
            PlayerPrefs.Save();
        }
    }

    public static int Gems
    {
        get
        {
            return PlayerPrefs.GetInt("Gems",5);
        }
        set
        {
            PlayerPrefs.SetInt("Gems", value);
            PlayerPrefs.Save();
        }
    }


    public static int LevelUp
    {
        get
        {
            return PlayerPrefs.GetInt("LevelUp", 1);
        }
        set
        {
            PlayerPrefs.SetInt("LevelUp", value);
            PlayerPrefs.Save();
        }
    }


    public static int Keys
    {
        get
        {
            return PlayerPrefs.GetInt("Keys",100);
        }
        set
        {
            PlayerPrefs.SetInt("Keys", value);
            PlayerPrefs.Save();
        }
    }

    public static int Lives
    {
        get
        {
            return PlayerPrefs.GetInt("Lives");
        }
        set
        {
            PlayerPrefs.SetInt("Lives", value);
            PlayerPrefs.Save();
        }
    }

    public static string GradeName
    {
        get { return PlayerPrefs.GetString("GradeName", "Shoshi"); }
        set { PlayerPrefs.SetString("GradeName", value); PlayerPrefs.Save(); }
    }

    public static string GradeColor
    {
        get { return PlayerPrefs.GetString("GradeColor", "White"); }
        set { PlayerPrefs.SetString("GradeColor", value); PlayerPrefs.Save(); }
    }

    public static int QuestionsToTreasure
    {
        get { return PlayerPrefs.GetInt("QuestionsToTreasure", 3); }
        set { PlayerPrefs.SetInt("QuestionsToTreasure", value); PlayerPrefs.Save(); }
    }

    public static int GradeUpgrade
    {
        get { return PlayerPrefs.GetInt("GradeUpgrade", 1); }
        set { PlayerPrefs.SetInt("GradeUpgrade", value); PlayerPrefs.Save(); }
    }

    public static int SportsQuiz
    {
        get
        {
            return PlayerPrefs.GetInt("Sports");
        }
        set
        {
            PlayerPrefs.SetInt("Sports", value);
            PlayerPrefs.Save();
        }
    }

    public static int AnimalQuiz
    {
        get
        {
            return PlayerPrefs.GetInt("Animal");
        }
        set
        {
            PlayerPrefs.SetInt("Animal", value);
            PlayerPrefs.Save();
        }
    }

    public static int VehicleQuiz
    {
        get
        {
            return PlayerPrefs.GetInt("Vehicle");
        }
        set
        {
            PlayerPrefs.SetInt("Vehicle", value);
            PlayerPrefs.Save();
        }
    }

    //custom parameters
    public static int GetCollection(string s)
    {
        return PlayerPrefs.GetInt("Collection" + s);
    }


    public static void SetCollection(string s,int value)
    {
        PlayerPrefs.SetInt("Collection" + s, value);

        PlayerPrefs.Save();
    }
    public static string CoinID
    {
        get
        {
            return PlayerPrefs.GetString("coinID");
        }
        set
        {
            PlayerPrefs.SetString("coinID", value);
            PlayerPrefs.Save();
        }
    }

    public static int GetCoins(int id)
    {
        //Debug.Log("GetCoin ID " + id);
        return PlayerPrefs.GetInt("Coin" + id);
    }

    public static void SetCoins(int id, int Value)
    {
        //Debug.Log("SetCoin ID " + id);
        Value = Value + PlayerPrefs.GetInt("Coin" + id);
        PlayerPrefs.SetInt("Coin" + id, Value);
        PlayerPrefs.Save();
    }
}
