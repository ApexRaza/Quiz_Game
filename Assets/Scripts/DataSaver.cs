using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using Firebase.Database;
using Firebase.Auth;
using System.Linq;
using System.Threading.Tasks;

[System.Serializable]
public class DataToSave 
{
    public string userName;
    public List<int> Coins;
    public List<int> Quizes;
    public Dictionary<string, string> FriendRequests;
    public Dictionary<string, string> Friends;
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
    public bool IsOnline;
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
        if (instance == null)
        {
            instance = this;
            Instance = instance;
            DontDestroyOnLoad(gameObject);
        }
        auth = Login.Instance.auth;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async void Online()
    {
        DataBase.IsOnline = true;
        FirebaseUser user = auth.CurrentUser;
        await DataBase.SaveOnlineStatus(user, true);
    }
    public string UserName(FirebaseUser user)
    {
        if (user.IsAnonymous)
        {
            return ("player" + Random.Range(10, 99) + Random.Range(100, 999));
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
        Debug.Log("Inside SaveData() Function");
        Debug.Log("Local Status" + DataBase.IsOnline);
        AssignData();

    }
    public async void TestFun()
    {
        //Debug.Log("Inside AssignData() Function");
        //FirebaseUser user = auth.CurrentUser;
        //userID = user.UserId;

        //// Fetch Friends and FriendRequests directly from Firebase
        //var snapshot = await dbRef.Child("users").Child(userID).GetValueAsync();

        //if (snapshot.Exists)
        //{
        //    Debug.Log("Fetching Friends and FriendRequests from Firebase");
        //    //comment if didn't work
        //    var friendsDictionary = new Dictionary<string, string>();

        //    // Retrieve Friends if it exists
        //    if (snapshot.Child("Friends").Exists)
        //    {
        //        //comment if didn't work
        //        foreach (var child in snapshot.Child("Friends").Children)
        //        {
        //            // Add the key and value to the dictionary
        //            friendsDictionary.Add(child.Key, child.Value.ToString());
        //        }

        //        //dts.Friends = snapshot.Child("Friends").Children
        //        //    .Select(child => child.Value.ToString())
        //        //    .ToList();

        //        //comment if didn't work
        //        dts.Friends = friendsDictionary.Select(f => f.Value).ToList();
        //    }
        //    else
        //    {
        //        dts.Friends = new List<string>(); // Initialize as empty list if not found
        //    }

        //    // Retrieve FriendRequests if it exists
        //    if (snapshot.Child("FriendRequests").Exists)
        //    {
        //        dts.FriendRequests = snapshot.Child("FriendRequests").Children
        //            .Select(child => child.Value.ToString())
        //            .ToList();
        //    }
        //    else
        //    {
        //        dts.FriendRequests = new List<string>(); // Initialize as empty list if not found
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning("No existing data found in Firebase for Friends and FriendRequests");
        //    // Initialize as empty lists if no data exists
        //    dts.Friends = new List<string>();
        //    dts.FriendRequests = new List<string>();
        //}

        //Debug.Log("Friends List: " + string.Join(", ", dts.Friends));
        //Debug.Log("FriendRequests List: " + string.Join(", ", dts.FriendRequests));
    }
    public async void AssignData()
    {
        Debug.Log("Inside AssignData() Function");
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
        dts.IsOnline = DataBase.IsOnline;
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

        var friendsTask = dbRef.Child("users").Child(userID).Child("Friends").GetValueAsync();
        var friendRequestsTask = dbRef.Child("users").Child(userID).Child("FriendRequests").GetValueAsync();

        // Wait for both tasks to complete
        await Task.WhenAll(friendsTask, friendRequestsTask);

        // Check if the data exists and assign it to dts
        dts.Friends = new Dictionary<string, string>(); // Initialize the dictionary
        if (friendsTask.Result.Exists)
        {
            foreach (var child in friendsTask.Result.Children)
            {
                dts.Friends[child.Key] = child.Value.ToString(); // Key is userID, value is username
            }
        }

        dts.FriendRequests = new Dictionary<string, string>(); // Initialize the dictionary
        if (friendRequestsTask.Result.Exists)
        {
            foreach (var child in friendRequestsTask.Result.Children)
            {
                dts.FriendRequests[child.Key] = child.Value.ToString(); // Key is userID, value is username
            }
        }
        await DataBase.SaveOnlineStatus(user, true);
        var updates = new Dictionary<string, object>
        {
            { "Dollars", dts.Dollars },
            { "Gems", dts.Gems },
            { "Keys", dts.Keys },
            { "LevelUp", dts.LevelUp },
            { "Lives", dts.Lives },
            { "GradeName", dts.GradeName },
            { "GradeColor", dts.GradeColor },
            { "QuestionsToTreasure", dts.QuestionsToTreasure },
            { "GradeUpgrade", dts.GradeUpgrade },
            { "IsOnline", dts.IsOnline },
            { "Friends", dts.Friends },
            { "FriendRequests", dts.FriendRequests },
            { "Coins", dts.Coins },
            { "Quizes", dts.Quizes },
            { "Question", dts.Question },
            { "RightAnswer", dts.RightAnswer },
            { "WrongAnswer", dts.WrongAnswer }
            
        };
        if (PlayerPrefs.GetInt("FirstTimeAnom") == 0)
        {
            string json = JsonUtility.ToJson(dts);
            await dbRef.Child("users").Child(userID).SetRawJsonValueAsync(json);
            PlayerPrefs.SetInt("FirstTimeAnom", 1);
        }
        else
        {
            // Update the database without deleting existing data
            await dbRef.Child("users").Child(userID).UpdateChildrenAsync(updates);
        }
    }


    public void UpdateCoin()
    {
         dbRef.Child("users").Child(userID).Child("Coins").ValueChanged += (object sender, ValueChangedEventArgs e) =>
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError($"Database Error: {e.DatabaseError.Message}");
                return;
            }

            if (e.Snapshot.Exists && e.Snapshot.Value != null)
            {

               
                    Debug.Log($"Coins updated: ");
                StartCoroutine(LoadDataEnum());
              
            }
            else
            {
                Debug.Log("Coins data does not exist.");
            }
        };
       
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
        if (jsonData != null)
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

    public async void UpdateData()
    {
        FirebaseUser user = auth.CurrentUser;
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
        for (int num = 0; num < 420; num++)
        {
            DataBase.UpdateCoins(num, dts.Coins[num]);
            //Debug.Log("Is it Here");
        }
        for (int num = 0; num < 16; num++)
        {
            DataBase.SetQuiz(num, dts.Quizes[num]);
        }
        DataBase.Questions = dts.Question;
        DataBase.RightAnswer = dts.RightAnswer;
        DataBase.WrongAnswer = dts.WrongAnswer;
        await DataBase.SaveOnlineStatus(user, true);
        Debug.Log("UpdateData() Complete. " + dts.userName);

    }

    private void OnApplicationQuit()
    {
        DataBase.IsOnline = false;
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            // Save the online status to the database
            DataBase.SaveOnlineStatus(user, false).ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError($"Failed to save online status: {task.Exception}");
                }
            });
        }
    }
}
