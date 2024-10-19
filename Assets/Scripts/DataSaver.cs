using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using Firebase.Database;
using Firebase.Auth;

[System.Serializable]
public class DataToSave 
{
    public string userName;
    public List<int> Coins;
    public List<int> Quizes;
    public List<string> FriendRequests;
    public List<string> Friends;
    public int Question;
    public int RightAnswer;
    public int WrongAnswer;
    public int Dollars;
    public int Gems;
    public int Lives;
    public int Keys;
    public int LevelUp;
    public string GradeName;
    public string GradeColor;
    public int QuestionsToTreasure;
    public int GradeUpgrade;
}

public class DataSaver : MonoBehaviour
{
    private static DataSaver instance;
    public static DataSaver Instance;
    public DataToSave dts;
    public string userID;
    public DatabaseReference dbRef;
    private FirebaseAuth auth;
    

    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
            Instance = instance;
            DontDestroyOnLoad(gameObject);
        }
        auth = Login.Instance.auth;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public string UserName(FirebaseUser user)
    {
        if (user.IsAnonymous)
        {
            return ("Player" + Random.Range(10, 99) + Random.Range(100, 999));
        }
        else
        {
            return user.DisplayName;
        }
    }

    public void SaveUsername() 
    {
        FirebaseUser user = auth.CurrentUser;
        dts.userName = UserName(user);
        DataBase.UserName = dts.userName;
    }
    
    public void SaveData() 
    {
        //Debug.Log("Inside SaveData() Function");
        AssignData();
        
    }

    public void AssignData()
    {
        //Debug.Log("Inside AssignData() Function");
        FirebaseUser user = auth.CurrentUser;
        userID = user.UserId;
        if (user.IsAnonymous)
        {
            dts.userName = DataBase.UserName;
        }
        else
        {
            dts.userName = user.DisplayName;
            DataBase.UserName = dts.userName;
        }

        dts.Dollars = DataBase.Dollars;
        dts.Gems = DataBase.Gems;
        dts.Keys = DataBase.Keys;
        dts.LevelUp = DataBase.LevelUp;
        dts.Lives = DataBase.Lives;
        dts.GradeName = DataBase.GradeName;
        dts.GradeColor = DataBase.GradeColor;
        dts.QuestionsToTreasure = DataBase.QuestionsToTreasure;
        dts.GradeUpgrade = DataBase.GradeUpgrade;
        for (int num = 0; num < 420; num++)
        {
            if (dts.Coins.Count <= num)
            {
                dts.Coins.Add(DataBase.GetCoins(num));
            }
            else
            {
                dts.Coins[num] = DataBase.GetCoins(num);
            }
        }

        for (int num = 0; num < 16; num++)
        {
            if (dts.Quizes.Count <= num)
            {
                dts.Quizes.Add(DataBase.GetQuiz(num));
            }
            else
            {
                dts.Quizes[num] = DataBase.GetQuiz(num);
            }
        }

        //Questions
        dts.Question = DataBase.Questions;
        dts.RightAnswer = DataBase.RightAnswer;
        dts.WrongAnswer = DataBase.WrongAnswer;

        //

      //  Debug.Log("FriendRequests : " + DataBase.FriendRequests.Count);
        dts.FriendRequests = DataBase.FriendRequests;
        dts.Friends = DataBase.Friends;

        string json = JsonUtility.ToJson(dts);
        dbRef.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }

    public void LoadData()
    {
        Debug.Log("Inside LoadData() Function");
        StartCoroutine(LoadDataEnum());
    }
    public IEnumerator LoadDataEnum() 
    {
        Debug.Log("LoadDataEnum() userID : " + auth.CurrentUser.UserId);
        userID = auth.CurrentUser.UserId;
        var serverData = dbRef.Child("users").Child(userID).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);
        Debug.Log("serverData : " + serverData);
        Debug.Log("Process is complete");
        DataSnapshot snapshot = serverData.Result;
        string jsonData = snapshot.GetRawJsonValue();
      //  Debug.Log("json data : " + jsonData);
        if(jsonData != null) 
        {
            Debug.Log("Server Data is found");
            dts = JsonUtility.FromJson<DataToSave>(jsonData);
            Debug.Log("load Username : " + dts.userName);

            UpdateData();
        }
        else 
        {
            Debug.Log("Else load Username: " + dts.userName);
            SaveData();
        }
    }

    public void UpdateData() 
    {
        // update player prefs using 
        Debug.Log("UpdateData() Fun");
        if (auth.CurrentUser.IsAnonymous) 
        {
            DataBase.UserName = dts.userName;
        }
        else 
        {
            dts.userName = auth.CurrentUser.DisplayName;
            DataBase.UserName = dts.userName;
        }
        DataBase.Dollars = dts.Dollars;
        DataBase.Gems = dts.Gems;
        DataBase.Keys = dts.Keys;
        DataBase.LevelUp = dts.LevelUp;
        DataBase.Lives = dts.Lives;
        DataBase.GradeName = dts.GradeName;
        DataBase.GradeColor = dts.GradeColor;
        DataBase.QuestionsToTreasure = dts.QuestionsToTreasure;
        DataBase.GradeUpgrade = dts.GradeUpgrade;
        //for (int num = 0; num < 420; num++)
        //{
        //    DataBase.SetCoins(num, dts.Coins[num]);
        //    Debug.Log("Is it Here");
        //}
        for(int num = 0; num < 16; num++) 
        {
            DataBase.SetQuiz(num, dts.Quizes[num]);
        }
        DataBase.Questions = dts.Question;
        DataBase.RightAnswer = dts.RightAnswer;
        DataBase.WrongAnswer = dts.WrongAnswer;

        Debug.Log("savedataforfriends : " + dts.Friends.ToString());
        Debug.Log("savedataforfriends : " + dts.FriendRequests.ToString());
        DataBase.FriendRequests = dts.FriendRequests;
        DataBase.Friends = dts.Friends;

        //SaveData();
    }
}
