using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserResultItem : MonoBehaviour
{
    public TextMeshProUGUI userNameText;
    private UserInfo userInfo;

    public void Initialize(UserInfo user)
    {
        userInfo = user;
        userNameText.text = user.UserName;

        Button sendRequestButton = GetComponent<Button>();
        sendRequestButton.onClick.AddListener(SendFriendRequest); // Attach the send request method
    }

    public void SendFriendRequest()
    {
        Debug.Log("Sending friend request to: " + userInfo.UserName);
        FriendManager.Instance.SendFriendRequest(userInfo.UserId); // Call the method to send a friend request
    }
}
