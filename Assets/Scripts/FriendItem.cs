using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
    public TextMeshProUGUI userNameText;
    public Image onlineStatusImage;

    public void Initialize(string userName, bool isOnline)
    {
        userNameText.text = userName;
        onlineStatusImage.color = isOnline ? Color.green : Color.red;
    }



}