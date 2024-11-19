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
    private string senderUsername;
    private string userID;
    private string userUsername;

    public void Initialize(string senderID, string userName)
    {
        this.senderID = senderID;
        userNameText.text = userName;
        db = DataSaver.Instance.dbRef;
        auth = Login.Instance.auth;
        senderUsername = userName;
        userID = auth.CurrentUser.UserId;
        Debug.Log("senderID : " + senderID);
        acceptButton.onClick.AddListener(AcceptRequest);
        rejectButton.onClick.AddListener(RejectRequest);
    }

    public void AcceptRequest()
    {
        Debug.Log("Accepting friend request from: " + senderID);
        // Logic to accept the friend request
        // Add sender to friends list and remove from requests
        FriendManager.Instance.AcceptFriendRequest(senderID, senderUsername).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error accepting request: " + task.Exception);
                ShowErrorMessage("Error accepting friend request.");
            }
            else
            {
                Debug.Log("Friend request accepted.");
                RemoveFromList(); // Remove this item from the list
            }
        }); 
    }

    public void RejectRequest()
    {
        Debug.Log("Rejecting friend request from: " + senderID);
        // Logic to reject the friend request
        // Remove sender from requests
        FriendManager.Instance.RemoveFriendRequest(senderID).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error rejecting request: " + task.Exception);
                ShowErrorMessage("Error rejecting friend request.");
            }
            else
            {
                Debug.Log("Friend request rejected.");
                RemoveFromList(); // Remove this item from the list
            }
        });
    }

    private void RemoveFromList()
    {
        Destroy(gameObject); // Remove this item from the UI
    }

    // Helper methods to show messages to the user
    private void ShowErrorMessage(string message)
    {
        // Implement UI logic to show error messages to the user
        Debug.LogError(message);
    }
}
