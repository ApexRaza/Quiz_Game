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
    public int Coins;
    public int Dollars;
    public int Gems;
    public int Lives;
    public int Keys;
    public int LevelUp;

}
public class DataSaver : MonoBehaviour
{
    public DataToSave dts;
    public string userID;
    DatabaseReference dbRef;
    private FirebaseAuth auth;
    

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        auth = FirebaseAuth.DefaultInstance;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void assignData() 
    {
        FirebaseUser user = auth.CurrentUser;
        dts.userName = user.DisplayName;
        userID = user.UserId;
        ///... use playerPrefs to update data..//
        dts.Coins = DataBase.Coins;
        dts.Dollars = DataBase.Dollars;
        dts.Gems = DataBase.Gems;
        dts.Keys = DataBase.Keys;
        dts.LevelUp = DataBase.LevelUp;
        dts.Lives = DataBase.Lives;
    }
    public void savaData() 
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
            Debug.Log("No data available");
        }
    }

    public void updateData() 
    {
        // update player prefs using 
    }
}
