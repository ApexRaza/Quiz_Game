using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
    public TextMeshProUGUI userNameText;

    public void Initialize(string userName)
    {
        userNameText.text = userName;
    }
}