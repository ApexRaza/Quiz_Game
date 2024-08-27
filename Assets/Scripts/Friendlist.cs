using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Friendlist : MonoBehaviour
{
    private static Friendlist instance;
    public static Friendlist Instance;

    public GameObject friendPrefab;
    public Transform friendListContainer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Instance = instance;
        }
    }

    public void DisplayFBFriends(List<Dictionary<string, object>> friends) 
    {
        foreach (var friendDict in friends)
        {
            string friendName = friendDict["name"] as string;
            string friendID = friendDict["id"] as string;

            GameObject friendItem = Instantiate(friendPrefab, friendListContainer);
            friendItem.transform.Find("FriendNameText").GetComponent<Text>().text = friendName;

            //Button inviteButton = friendItem.transform.Find("InviteButton").GetComponent<Button>();
            //inviteButton.onClick.AddListener(() => SendGameInvite(friendID));
        }
    }
}
