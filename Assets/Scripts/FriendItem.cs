using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
    public TextMeshProUGUI userNameText;
    public Image statusIndicator;

    public void Initialize(string userName)
    {
        userNameText.text = userName;
    }
}