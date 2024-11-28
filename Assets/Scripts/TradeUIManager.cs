using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Firebase.Extensions;
using System.Collections;


public class TradeUIManager : MonoBehaviour
{
    public GameObject[] bgButtons, frontEndButtons;
    private string ID, index;
    private DatabaseReference dbRef;
    private FirebaseAuth auth;
    private string userID;
    // Start is called before the first frame update
    void Start()
    {
      
        auth = Login.Instance.auth;
       
        ID = auth.CurrentUser.UserId;
    }

    private void OnEnable()
    {
        ButtonEffect(0);
    }

    public void ButtonEffect(int num)
    {
        for (int i = 0; i < 2; i++)
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
    public void LoadTradeList()
    {
        StartCoroutine(LoadTradeData());
    }

    private IEnumerator LoadTradeData()
    {
        Task<DataSnapshot> DBTask = DataSaver.Instance.dbRef.Child("users").Child(ID).Child("Trade").GetValueAsync();
       
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to load  with {DBTask.Exception}");
            yield break; // Exit if there's an error
        }

        DataSnapshot snapshot = DBTask.Result;
        index = snapshot.Value.ToString();


        Task<DataSnapshot> DBTask_1 = DataSaver.Instance.dbRef.Child("users").Child(ID).Child("Coins").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask_1.IsCompleted);

        if (DBTask_1.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to load  with {DBTask_1.Exception}");
            yield break; // Exit if there's an error
        }

        DataSnapshot snapshot_1 = DBTask_1.Result;

        Debug.LogWarning(message: $"dummy Failed to load  with {DBTask_1 .Exception}");

        string s = snapshot_1.Children.ElementAt(int.Parse(index)).Key.ToString(); 
       
        Debug.LogError("Value on " + index + " = " + s);

      

        
        //foreach (DataSnapshot childSnapshot in snapshot.Children)
        //{

        //    index = childSnapshot.Key.ToString();
        //    Debug.Log("Index " + index);
        //}
    }


    public void LoadTradesList(Transform tradesListContent)
    {
        StartCoroutine(LoadTradingData("Friends", tradesListContent, false));
    }

    private IEnumerator LoadTradingData(string childNode, Transform content, bool isRequest)
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
            Task<DataSnapshot> frndLevelTask = DataSaver.Instance.dbRef.Child("users").Child(id).Child("LevelUp").GetValueAsync(); // Fetch online status
            yield return new WaitUntil(predicate: () => frndLevelTask.IsCompleted);

            if (frndLevelTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to load  with {frndLevelTask.Exception}");
                yield break; // Exit if there's an error
            }

            DataSnapshot frndLevelSnap = frndLevelTask.Result;

            //Debug.LogError("Level UP VALUE: " + frndLevelSnap.Value.ToString());
            // bool frndLevel = frndLevelTask.Result.Exists && (bool)frndLevelTask.Result.Value;
            int level = int.Parse(frndLevelSnap.Value.ToString());
           // Debug.LogError("Level UP VALUE: " + level);
            GameObject requestItem = Instantiate(isRequest ? FriendUIManager.Instance.friendRequestItemPrefab : FriendUIManager.Instance.friendItemPrefab, content);
            if (level == DataBase.LevelUp)
            {

                Debug.LogError("User ID: " + id + " = " + level + " is matched.");
            }
            else
            {
                Debug.LogError("User ID: " + id + " = " + level + " did not match.");
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
