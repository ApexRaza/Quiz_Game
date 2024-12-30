using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Auth;
using System.Threading.Tasks;

[System.Serializable]
public class StringListContainer
{
    public List<string> list;
}

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
    public static bool IsOnline
    {
        get
        {
            return PlayerPrefs.GetInt("IsOnline", 0) == 1; // 1 for true, 0 for false
        }
        set
        {
            PlayerPrefs.SetInt("IsOnline", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static async Task SaveOnlineStatus(FirebaseUser user, bool isOnline)
    {
        IsOnline = isOnline; // Save locally
        string userID = user.UserId;

        // Save to Firebase
        await DataSaver.Instance.dbRef.Child("users").Child(userID).Child("IsOnline").SetValueAsync(isOnline);
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
            return PlayerPrefs.GetInt("Lives",5);
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
        set { PlayerPrefs.SetInt("QuestionsToTreasure", value); PlayerPrefs.Save();}
    }

    public static int GradeUpgrade
    {
        get { return PlayerPrefs.GetInt("GradeUpgrade", 1); }
        set { PlayerPrefs.SetInt("GradeUpgrade", value); PlayerPrefs.Save();}
    }

   public static string UserName 
    {
        
        get { return PlayerPrefs.GetString("Username", ""); }
        set { PlayerPrefs.SetString("Username", value); PlayerPrefs.Save(); /*DataSaver.Instance.SaveData(); Debug.Log("In Database: User Name");*/ }
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


    public static void UpdateCoins(int id, int Value)
    {
        //Debug.Log("SetCoin ID " + id);
        PlayerPrefs.SetInt("Coin" + id, Value);
        PlayerPrefs.Save();
    }


    public static int GetQuiz(int id)
    {
        //Debug.Log("GetCoin ID " + id);
        return PlayerPrefs.GetInt("Quiz" + id,0);
    }

    public static void SetQuiz(int id, int Value)
    {
       
        PlayerPrefs.SetInt("Quiz" + id, Value);
        PlayerPrefs.Save();
    }

    // Utility Methods to Handle Lists in PlayerPrefs
    private static List<string> GetStringList(string key)
    {
        string json = PlayerPrefs.GetString(key, "{\"list\":[]}");
        StringListContainer container = JsonUtility.FromJson<StringListContainer>(json);
        return container.list ?? new List<string>();
    }

    private static void SetStringList(string key, List<string> list)
    {
        StringListContainer container = new StringListContainer { list = list };
        string json = JsonUtility.ToJson(container);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public static List<string> FriendRequests
    {
        get
        {
            return GetStringList("FriendRequests");
        }
        set
        {
            SetStringList("FriendRequests", value);
        }
    }

    // Friends
    public static List<string> Friends
    {
        get
        {
            return GetStringList("Friends");
        }
        set
        {
            SetStringList("Friends", value);
        }
    }

    public static int Questions
    {
        get
        {
            return PlayerPrefs.GetInt("Questions", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Questions", value);
            PlayerPrefs.Save();
        }
    }

    public static int RightAnswer
    {
        get
        {
            return PlayerPrefs.GetInt("RightAnswer", 0);
        }
        set
        {
            PlayerPrefs.SetInt("RightAnswer", value);
            PlayerPrefs.Save();
        }
    }

    public static int WrongAnswer
    {
        get
        {
            return PlayerPrefs.GetInt("WrongAnswer", 0);
        }
        set
        {
            PlayerPrefs.SetInt("WrongAnswer", value);
            PlayerPrefs.Save();
        }
    }
}
