using UnityEngine;

public class DataBase 
{

    public static int Coins
    {
        get
        {
            return PlayerPrefs.GetInt("Coins");
        }
        set
        {
            PlayerPrefs.SetInt("Coins", value);
            PlayerPrefs.Save();
        }


    }
 
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
            return PlayerPrefs.GetInt("LevelUp");
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

    public static int GeneralQuiz
    {
        get
        {
            return PlayerPrefs.GetInt("General");
        }
        set
        {
            PlayerPrefs.SetInt("General", value);
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

   
}
