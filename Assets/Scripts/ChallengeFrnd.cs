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
using Photon.Pun.UtilityScripts;
using System.Globalization;


public class ChallengeFrnd : MonoBehaviour
{
    
    private DatabaseReference dbRef;
    public string ID, index;
    private FirebaseAuth auth;
    private string userID, frndID;
    public GameObject challengeFrnd;
    public RectTransform scrollContent;
    public GameObject challengerPopup;
    public ConnectAndJoinRandom connectAndJoin;
    public GameObject waitingPanel, challengertimerScript;

    // Start is called before the first frame update
    void Awake()
    {

        auth = Login.Instance.auth;
        dbRef = DataSaver.Instance.dbRef;
        ID = auth.CurrentUser.UserId;
        userID = ID;
     
    }
    private void Start()
    {
        UpdateChallengers();
    }


    public void LoadFrndsList(Transform tradesListContent)
    {
        StartCoroutine(LoadFrnds_List("Friends", tradesListContent, false));
    }
    int inc = 0;
    // function to load list of frnds which are online.
    private IEnumerator LoadFrnds_List(string childNode, Transform content, bool isRequest)
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
                GameObject requestItem = Instantiate(isRequest ? challengeFrnd : challengeFrnd, content);
                ChallengeItem item = requestItem.GetComponent<ChallengeItem>();

                item.Initialize( username, this, id,isOnline);
               


            }
            else
            {
                Debug.LogError("User ID: " + id + " = " + isOnline + " did not match.");
            }

        }
    }


    public void ChallengeFriend(string frndID)
    {
        //StartCoroutine(Challenge(frndID));
        Challenge(frndID);
        this.frndID = frndID;
    }
    public async void Challenge(string frndID)
    {


        var challengeData = new Dictionary<string, object>
        {
        { "senderId", userID }, // Store sender ID in the trade data
            { "userName", DataBase.UserName },
            
        };


        try
        {
            // Generate a unique key for each trade request
            string uniqueTradeKey = dbRef.Child("users").Child(frndID).Child("Challenger").Push().Key;

            // Use the unique key to store the trade request
            string tradePath = $"users/{frndID}/Challenger/{userID}";

            await dbRef.Child(tradePath).SetValueAsync(challengeData);
            Debug.Log("Trade request sent successfully with unique key: " + uniqueTradeKey);
            
           
            connectAndJoin.CreateRoom(userID);
            waitingPanel.SetActive(true);
            challengertimerScript.SetActive(true);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error sending trade request: " + ex.Message);
        }


    }




    public  void UpdateChallengers()
    {

        dbRef.Child("users").Child(userID).Child("Challenger").ValueChanged += (object sender, ValueChangedEventArgs e) =>
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError($"Database Error: {e.DatabaseError.Message}");
                return;
            }

            if (e.Snapshot.Exists && e.Snapshot.Value != null)
            {

                // challengerPopup.SetActive(true);
                StartCoroutine(CheckforAnyChange());
                Debug.Log($"Coins updated: ");
               

            }
            else
            {
                Debug.Log("Coins data does not exist.");
            }
        };


        dbRef.Child("users").Child(userID).Child("Challenger").ValueChanged -= (object sender, ValueChangedEventArgs e) =>
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError($"Database Error: {e.DatabaseError.Message}");
                return;
            }

            if (e.Snapshot.Exists && e.Snapshot.Value != null)
            {

                challengerPopup.SetActive(false);
              //  StartCoroutine(CheckforAnyChange());
                Debug.Log($"Coins updated: ");


            }
            else
            {
                Debug.Log("Coins data does not exist.");
            }
        };








    }

    IEnumerator CheckforAnyChange()
    {
        Task<DataSnapshot> DBTask = dbRef.Child("users").Child(userID).Child("Challenger").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        DataSnapshot dataSnapshot = DBTask.Result;
        if (dataSnapshot.ChildrenCount > 0)
        {
            foreach (DataSnapshot child in dataSnapshot.Children)
            {
                challengerPopup.gameObject.SetActive(true);

                Debug.Log(child.Value + "  ----------  " + child.Key);
                challengerPopup.GetComponent<ChallengerInfo>().Initialize((child.Child("userName").Value.ToString()), this, child.Key);

            }
        }
      
    }

    


    public void AcceptChallenge(string id)
    {
        connectAndJoin.JoinRoombyID(id);
    }
    public async void RejectChallenge(string id)
    {
       await  dbRef.Child("users").Child(userID).Child("Challenger").Child(id).RemoveValueAsync();
    }

    public async void RejectChallengeRequest()
    {
        await dbRef.Child("users").Child(frndID).Child("Challenger").Child(userID).RemoveValueAsync();

    }



    private void ClearContent(Transform content)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
