using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Firebase.Extensions;
using System.Collections;
//using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Net;
using System;

public class ChallengeFrnd : MonoBehaviour
{
    
    private DatabaseReference dbRef;
    public string ID, index;
    private FirebaseAuth auth;
    private string userID, frndID;
    public GameObject tradeFrnd, viewTradeReqst, proposedPanel, demandedPanel;



    // Start is called before the first frame update
    void Awake()
    {

        auth = Login.Instance.auth;
        dbRef = DataSaver.Instance.dbRef;
        ID = auth.CurrentUser.UserId;
        userID = ID;
    }


    public void LoadTradesList(Transform tradesListContent)
    {
        StartCoroutine(LoadTradingList("Friends", tradesListContent, false));
    }
    int inc = 0;
    // function to load list of frnds with whome u can trade.
    private IEnumerator LoadTradingList(string childNode, Transform content, bool isRequest)
    {
        Task<DataSnapshot> DBTask = DataSaver.Instance.dbRef.Child("users").Child(ID).Child(childNode).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to load {childNode} with {DBTask.Exception}");
            yield break; // Exit if there's an error
        }

        DataSnapshot snapshot = DBTask.Result;

        // Clear existing UI elements
        ClearContent(content);

        foreach (DataSnapshot childSnapshot in snapshot.Children)
        {
            string id = childSnapshot.Key.ToString();
            string username = childSnapshot.Value.ToString();


            //string level = childSnapshot.Value.ToString();
            Debug.LogError("user ID " + id + " UserNAme: " + username);
            Task<DataSnapshot> frndLevelTask = DataSaver.Instance.dbRef.Child("users").Child(id).Child("IsOnline").GetValueAsync(); // Fetch online status
            yield return new WaitUntil(predicate: () => frndLevelTask.IsCompleted);

            if (frndLevelTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to load  with {frndLevelTask.Exception}");
                yield break; // Exit if there's an error
            }

            DataSnapshot frndLevelSnap = frndLevelTask.Result;

            //Debug.LogError("Level UP VALUE: " + frndLevelSnap.Value.ToString());
            // bool frndLevel = frndLevelTask.Result.Exists && (bool)frndLevelTask.Result.Value;
            bool isOnline = bool.Parse(frndLevelSnap.Value.ToString());
            // Debug.LogError("Level UP VALUE: " + level);

            if(isOnline)
            {

                Debug.LogError("User ID: " + id + " = " + isOnline + " is matched.");
                GameObject requestItem = Instantiate(isRequest ? tradeFrnd : tradeFrnd, content);
                FriendItem item = requestItem.GetComponent<FriendItem>();

                item.Initialize( username, isOnline);
               


            }
            else
            {
                Debug.LogError("User ID: " + id + " = " + isOnline + " did not match.");
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
}
