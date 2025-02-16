using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Firebase.Extensions;
using System.Collections;

using System;
using TMPro;
using UnityEngine.UIElements;


[System.Serializable]
public class UserInfo
{
    public string UserId;   // The unique identifier for the user
    public string UserName;  // The display name of the user
}
public class FriendManager : MonoBehaviour
{
    private static FriendManager instance;
    public static FriendManager Instance;
    [HideInInspector]
    public DataSaver dataSaver;
    private DatabaseReference dbRef;
    private FirebaseAuth auth;
    private string userID;

    public TextMeshProUGUI displayTxt;
    public Transform searchlist;
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
    }
    public void SearchUsersByUsername(string username, System.Action<List<UserInfo>> callback)
    {
       
        dbRef.Child("users").OrderByChild("userName").StartAt(username).EndAt(username + "\uf8ff").GetValueAsync().ContinueWithOnMainThread(task =>
        {
           
            List<UserInfo> users = new List<UserInfo>();

            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error searching for users: " + task.Exception);
                callback(users);
                return;
            }

            DataSnapshot snapshot = task.Result;
            Debug.Log("in the search func: " + task.Result);
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                string userId = childSnapshot.Key;
                string userName = childSnapshot.Child("userName").Value.ToString();

                Debug.Log("in the search func: " + userId);
                // Check if the user is already a friend
                dbRef.Child("users").Child(userID).Child("Friends").Child(userId).GetValueAsync().ContinueWithOnMainThread(friendCheckTask =>
                {
                    Debug.Log("in the search func 0");
                    if (friendCheckTask.IsFaulted || friendCheckTask.IsCanceled)
                    {
                        Debug.LogError("Error checking friends list: " + friendCheckTask.Exception);
                        return;
                    }

                    DataSnapshot friendSnapshot = friendCheckTask.Result;
                    if (!friendSnapshot.Exists && userId != userID) // Only add if not already friends and not self
                    {
                        Debug.Log("in the search func 1");
                        users.Add(new UserInfo { UserId = userId, UserName = userName });
                    }

                    // Call the callback after processing all users
                    if (users.Count == snapshot.Children.Count())
                    {
                        callback(users);
                    }
                });
            }
        });
    }
    public Task SendFriendRequest(string recipientUserId)
    {
        // Check if the recipient is already a friend
        return dbRef.Child("users").Child(userID).Child("Friends").Child(recipientUserId).GetValueAsync().ContinueWithOnMainThread(friendCheckTask =>
        {
            if (friendCheckTask.IsFaulted || friendCheckTask.IsCanceled)
            {
                Debug.LogError("Error checking friends list: " + friendCheckTask.Exception);
                return;
            }

            DataSnapshot friendSnapshot = friendCheckTask.Result;
            if (friendSnapshot.Exists)
            {
                Debug.Log("You are already friends with this user.");
                return; // Exit if already friends
            }

            // Check if a friend request has already been sent
            dbRef.Child("users").Child(recipientUserId).Child("FriendRequests").Child(userID).GetValueAsync().ContinueWithOnMainThread(requestCheckTask =>
            {
                if (requestCheckTask.IsFaulted || requestCheckTask.IsCanceled)
                {
                    Debug.LogError("Error checking friend requests: " + requestCheckTask.Exception);
                    return;
                }

                if (requestCheckTask.Result.Exists)
                {
                    Debug.Log("Friend request already sent to this user.");
                    return; // Exit if request already sent
                }

                // Proceed to send the friend request
                dbRef.Child("users").Child(recipientUserId).Child("FriendRequests").Child(userID).SetValueAsync(DataBase.UserName)
                    .ContinueWithOnMainThread(setTask =>
                    {
                        if (setTask.IsFaulted || setTask.IsCanceled)
                        {
                            Debug.LogError("Error sending friend request: " + setTask.Exception);
                        }
                        else
                        {
                            Debug.Log("Friend request sent successfully.");
                            ClearContentSearch(searchlist);
                        }
                    });
            });
        });
    }
    public Task RemoveFriendRequest(string senderID)
    {
        // Get the current user's ID
        string currentUserID = auth.CurrentUser.UserId;

        // Create a task to remove the sender from the current user's friend requests
        return dbRef.Child("users").Child(currentUserID).Child("FriendRequests").Child(senderID).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error removing friend request: " + task.Exception);
                // Handle error (e.g., show a message to the user)
            }
            else
            {
                Debug.Log("Friend request from " + senderID + " removed successfully.");
                // Optionally, you can notify the UI or perform additional actions here
            }
        });
    }
    
    public Task AcceptFriendRequest(string senderID, string senderName)
    {
        // Get the current user's ID
        string currentUserID = auth.CurrentUser.UserId;

        // Create a task to add the sender to the current user's friends list
        var addFriendToCurrentUserTask = dbRef.Child("users").Child(currentUserID).Child("Friends").Child(senderID).SetValueAsync(senderName);

        // Create a task to add the current user to the sender's friends list
        var addFriendToSenderTask = dbRef.Child("users").Child(senderID).Child("Friends").Child(currentUserID).SetValueAsync(DataBase.UserName);

        // Create a task to remove the sender from the current user's friend requests
        var removeRequestTask = dbRef.Child("users").Child(currentUserID).Child("FriendRequests").Child(senderID).RemoveValueAsync();

        // Return a task that completes when all operations are done
        return Task.WhenAll(addFriendToCurrentUserTask, addFriendToSenderTask, removeRequestTask).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error accepting friend request: " + task.Exception);
                // Handle error (e.g., show a message to the user)
            }
            else
            {
                Debug.Log("Friend request accepted successfully for both users.");
                // Optionally, you can notify the UI or perform additional actions here
                ClearContentSearch(searchlist);

            }
        });
    }
    public void LoadFriendRequests(Transform friendRequestContent)
    {
        displayTxt.gameObject.SetActive(false);
        StartCoroutine(LoadFriendData("FriendRequests", friendRequestContent, true, "No friend request to display"));
    }

    public void LoadFriendsList(Transform friendsListContent)
    {
        displayTxt.gameObject.SetActive(false);
        StartCoroutine(LoadFriendData("Friends", friendsListContent, false, "No friends to display"));
    }

    private IEnumerator LoadFriendData(string childNode, Transform content, bool isRequest ,string message)
    {
        Task<DataSnapshot> DBTask = dbRef.Child("users").Child(userID).Child(childNode).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to load {childNode} with {DBTask.Exception}");
            yield break; // Exit if there's an error
        }

        DataSnapshot snapshot = DBTask.Result;

        // Clear existing UI elements
        ClearContent(content);

        if (snapshot.ChildrenCount <= 0)
        {
            // u r alone in the world looser!
            displayTxt.text = message;
            displayTxt.gameObject.SetActive(true);
        }
        else
        {

            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                string id = childSnapshot.Key.ToString();
                string username = childSnapshot.Value.ToString();

                Task<DataSnapshot> onlineStatusTask = dbRef.Child("users").Child(id).Child("IsOnline").GetValueAsync(); // Fetch online status
                yield return new WaitUntil(predicate: () => onlineStatusTask.IsCompleted);

                if (onlineStatusTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to load online status with {onlineStatusTask.Exception}");
                    yield break; // Exit if there's an error
                }

                bool isOnline = onlineStatusTask.Result.Exists && (bool)onlineStatusTask.Result.Value;

                GameObject requestItem = Instantiate(isRequest ? FriendUIManager.Instance.friendRequestItemPrefab : FriendUIManager.Instance.friendItemPrefab, content);
                if (isRequest)
                {
                    FriendRequestItem item = requestItem.GetComponent<FriendRequestItem>();
                    item.Initialize(id, username);
                }
                else
                {
                    FriendItem item = requestItem.GetComponent<FriendItem>();
                    item.Initialize(username, isOnline);
                }
            }
        }
    }

    private void ClearContent(Transform content)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    private void ClearContentSearch(Transform content)
    {
        StartCoroutine(delay(content));
    }


    IEnumerator delay(Transform content)
    {
        yield return new WaitForSeconds(1.5f);
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    public void ListenForTradeRequests(string userId)
    {

        Debug.Log(userId + "   ---------");
        dbRef.Child("users").Child("rgpKzivCDvRfXG5hKXZAi4hlcyx1").Child("Keys").ValueChanged += HandleTradeRequest;
    }

    private void HandleTradeRequest(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot.Exists)
        {
            //Debug.LogError("Keys changed");
            foreach (var user in args.Snapshot.Children)
            {
                // Process each trade request
                string senderId = user.Key;
               // int amount = int.Parse(tradeRequest.Child("amount").Value.ToString());
                // Notify the user about the trade request (UI update)
                Debug.LogError("Keys changed");
                //  ShowTradeRequest(senderId, amount);
            }
        }
    }




}



//41846186.85
