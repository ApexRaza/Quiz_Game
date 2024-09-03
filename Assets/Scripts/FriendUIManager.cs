using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;

public class FriendUIManager : MonoBehaviour
{
    private static FriendUIManager instance;
    public static FriendUIManager Instance;

    private FriendManager FM;
    private DatabaseReference dbRef;
    private FirebaseAuth auth;
    private string userID;
    public GameObject friendRequestItemPrefab;
    public GameObject friendItemPrefab;
    public Transform friendRequestContent;
    public Transform friendsListContent;
    public Button reqBtn;
    public TMP_InputField userIDInputField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Instance = instance;
        }
        auth = Login.Instance.auth;
        FM = FriendManager.Instance;
        dbRef = DataSaver.Instance.dbRef;
        userID = auth.CurrentUser.UserId;
    }
    private void Start()
    {
        reqBtn.onClick.AddListener(SendRequest);
    }

    public void SendRequest() 
    {
        string recipientID = userIDInputField.text.Trim();
        if (!string.IsNullOrEmpty(recipientID))
        {
            FM.SendFriendRequest(recipientID, " " + DataBase.UserName);
        }
    }

    public void LoadFriendRequests() 
    {
        StartCoroutine(LoadFriendRequestsData());
    }
    private IEnumerator LoadFriendRequestsData()
    {
        Task<DataSnapshot> DBTask = dbRef.Child("users").Child(userID).Child("FriendRequests").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
            Debug.Log(snapshot);

            //Destroy any existing scoreboard elements
            foreach (Transform child in friendRequestContent)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                string id = childSnapshot.Key.ToString();
                string username = childSnapshot.Value.ToString();
                GameObject requestItem = Instantiate(friendRequestItemPrefab, friendRequestContent);
                FriendRequestItem item = requestItem.GetComponent<FriendRequestItem>();
                Debug.Log("LoadFriendRequests");
                item.Initialize(id, username);
            }
        }
    }


    public void LoadFriendsList() 
    {
        StartCoroutine(LoadFriendListData());
    }

    private IEnumerator LoadFriendListData() 
    {
        Task<DataSnapshot> DBTask = dbRef.Child("users").Child(userID).Child("Friends").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            Debug.Log(snapshot);

            foreach (Transform child in friendsListContent)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                string username = childSnapshot.Value.ToString();
                GameObject requestItem = Instantiate(friendItemPrefab, friendsListContent);
                FriendItem item = requestItem.GetComponent<FriendItem>();
                Debug.Log("LoadFriendRequests");
                item.Initialize(username);
            }
        }
    }
}
