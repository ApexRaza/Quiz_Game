using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Firebase.Extensions;

public class FriendManager : MonoBehaviour
{
    private static FriendManager instance;
    public static FriendManager Instance;
    [HideInInspector]
    public DataSaver dataSaver;
    private DatabaseReference dbRef;
    private FirebaseAuth auth;
    private string userID;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Instance = instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        dataSaver = DataSaver.Instance;
        auth = Login.Instance.auth;
        dbRef = dataSaver.dbRef;
        userID = auth.CurrentUser.UserId;

        //ListenToFriendsStatus();
    }

    public void SendFriendRequest(string recipientID, string recipientUsername) 
    {
        dbRef.Child("users").Child(recipientID).Child("FriendRequests").GetValueAsync().ContinueWithOnMainThread(task => 
        {
            Debug.LogError("outsside if" + task.IsCompleted);
            if (task.IsCompleted)
            {
                Debug.LogError("inside if : " + task.IsCompleted);
                DataSnapshot snapshot = task.Result;
                List<string> receivedRequests = new List<string>();

                if (snapshot.Exists)
                {
                    Debug.LogError("inside if (snapshot.Exists) : " + snapshot.Exists);
                    foreach (DataSnapshot childSnapshot in snapshot.Children)
                    {
                        Debug.LogError("inside foreach : " + snapshot.ChildrenCount);
                        receivedRequests.Add(childSnapshot.ToString());
                    }
                }

                if (!receivedRequests.Contains(userID))
                {
                    Debug.LogError("inside if (!receivedRequests.Contains(userID)) : " + receivedRequests.Contains(userID));
                    receivedRequests.Add(dataSaver.dts.userName);
                    dbRef.Child("users").Child(recipientID).Child("FriendRequests").Child(userID).SetValueAsync(recipientUsername);
                    //dataSaver.dts.FriendRequests.Add(recipientID);
                }
                dataSaver.SaveData();
            }
        });
        
    }

    public async Task AcceptFriendRequest(string senderID) 
    {
        Debug.Log("AcceptFriendRequest : " + senderID);
        DatabaseReference senderRef = dbRef.Child("users").Child(senderID);
        DataSnapshot senderSnapshot = await senderRef.GetValueAsync();
        string senderName = senderSnapshot.Child("userName").Value.ToString();

        if (!dataSaver.dts.Friends.Contains(senderID))
        {
            Debug.Log("senderID : " + senderID);
            Debug.Log("userID : " + senderID);
            await dbRef.Child("users").Child(senderID).Child("Friends").Child(userID).SetValueAsync(DataBase.UserName);
            dataSaver.dts.Friends.Add(senderID);
            Debug.Log("dataSaver.dts.Friends" + dataSaver.dts.Friends.Count);
            await RemoveFriendRequest(senderID);
            //dataSaver.SaveData();
        }
    }

    public async Task RemoveFriendRequest(string recipientID) 
    {
        await dbRef.Child("users").Child(recipientID).Child("FriendRequests").Child(userID).RemoveValueAsync();

        //dataSaver.dts.FriendRequests.Remove(recipientID);

        Debug.Log("dataSaver.dts.FriendRequests" + dataSaver.dts.FriendRequests.Count);

        dataSaver.SaveData();
    }
}
