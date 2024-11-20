using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Firebase.Auth;
//using System.Threading.Tasks;

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
    public TMP_InputField searchInputField; // Assign this in the inspector
    public Transform searchResultsContent; // Assign this in the inspector
    public GameObject userResultPrefab;

    public GameObject[] bgButtons, frontEndButtons;

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
    private void OnEnable()
    {
        FM.LoadFriendsList(friendsListContent);
        searchInputField.onEndEdit.AddListener(OnSearch);
        ButtonEffect(0);


    }
    private void OnSearch(string searchTerm)
    {
        if (!string.IsNullOrEmpty(searchTerm))
        {
            FriendManager.Instance.SearchUsersByUsername(searchTerm, DisplaySearchResults);
        }
    }
    public void TriggerSearch()
    {
        string searchTerm = searchInputField.text; // Get the text from the input field
        OnSearch(searchTerm); // Call the OnSearch method with the retrieved text
    }
    private void DisplaySearchResults(List<UserInfo> users)
    {
        ClearSearchResults();

        foreach (var user in users)
        {
            GameObject resultItem = Instantiate(userResultPrefab, searchResultsContent);
            resultItem.GetComponent<UserResultItem>().Initialize(user); // Pass the user info
        }
    }
    private void ClearSearchResults()
    {
        foreach (Transform child in searchResultsContent)
        {
            Destroy(child.gameObject);
        }
    }


    public void ButtonEffect(int num)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == num)
            {
                bgButtons[i].SetActive(false);
                frontEndButtons[i].SetActive(true);
            }
            else
            {
                bgButtons[i].SetActive(true);
                frontEndButtons[i].SetActive(false);
            }
        }

    }


}
