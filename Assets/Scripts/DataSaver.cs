using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;
using Firebase.Auth;

[Serializable]
public class DataToSave 
{
    public string userName;
    public List<int> Coins;
    public List<int> Quizes;
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
        auth = FirebaseAuth.DefaultInstance;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void assignData() 
    {
        FirebaseUser user = auth.CurrentUser;
        dts.userName = user.DisplayName;
        userID = user.UserId;
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
              //  Debug.Log("Inside if : " + DataBase.GetCoins(num));
            }
            else 
            {
                dts.Coins[num] = DataBase.GetCoins(num);
               // Debug.Log("Inside else : " + DataBase.GetCoins(num));
            }
           // Debug.Log("outside if-else : " + DataBase.GetCoins(num));
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



    }
    public void SaveData() 
    {
        assignData();
        string json = JsonUtility.ToJson(dts);
        dbRef.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }

    public void loadData()
    {
        StartCoroutine(loadDataEnum());
    }
    IEnumerator loadDataEnum() 
    {
        var serverData = dbRef.Child("users").Child(userID).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        Debug.Log("Process is complete");
        DataSnapshot snapshot = serverData.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if(jsonData != null) 
        {
            Debug.Log("Server Data is found");
            dts = JsonUtility.FromJson<DataToSave>(jsonData);
            updateData();
        }
        else 
        {
            SaveData();
        }
    }

    public void updateData() 
    {
        // update player prefs using 
        FirebaseUser user = auth.CurrentUser;
        dts.userName = user.DisplayName;
        DataBase.Dollars = dts.Dollars;
        DataBase.Gems = dts.Gems;
        DataBase.Keys = dts.Keys;
        DataBase.LevelUp = dts.LevelUp;
        DataBase.Lives = dts.Lives;
        DataBase.GradeName = dts.GradeName;
        DataBase.GradeColor = dts.GradeColor;
        DataBase.QuestionsToTreasure = dts.QuestionsToTreasure;
        DataBase.GradeUpgrade = dts.GradeUpgrade;
        for (int num = 0; num < 420; num++)
        {
            DataBase.SetCoins(num, dts.Coins[num]);
        }
    }
}
