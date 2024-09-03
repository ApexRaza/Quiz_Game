using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;

public class FriendRequestItem : MonoBehaviour
{
    public TextMeshProUGUI userNameText;
    private DatabaseReference db;
    private FirebaseAuth auth;

    public Button acceptButton;
    public Button rejectButton;

    private string senderID;
    private string username;
    private string userID;

    public void Initialize(string senderID, string userName)
    {
        this.senderID = senderID;
        userNameText.text = userName;
        db = DataSaver.Instance.dbRef;
        auth = Login.Instance.auth;
        username = userName;
        userID = auth.CurrentUser.UserId;
        Debug.Log("senderID : " + senderID);
        acceptButton.onClick.AddListener(AcceptRequest);
        rejectButton.onClick.AddListener(RejectRequest);
    }

    public void AcceptRequest()
    {
        Debug.Log("senderID in AcceptRequest() : " + senderID);
        Debug.Log("FriendManager.Instance Exist : " + FriendManager.Instance);

        Task friendTask = FriendManager.Instance.AcceptFriendRequest(senderID);
        friendTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error accepting request : " + task.Exception);
            }
            else
            {
                Debug.Log("Friend added");
                db.Child("users").Child(userID).Child("Friends").Child(senderID).SetValueAsync(username);
                DestroyGameObject();
            }
        });
    }

    public void RejectRequest()
    {
        Task friendTask = FriendManager.Instance.RemoveFriendRequest(senderID);
        friendTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error accepting request : " + task.Exception);
            }
            else
            {
                Debug.Log("Friend added");
                DestroyGameObject();
            }
        });
    }

    private void DestroyGameObject() 
    {
        Destroy(gameObject);
    }
}
