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


public class TradeUIManager : MonoBehaviour
{
    public GameObject[] bgButtons, frontEndButtons;
    public string ID, index;
    private DatabaseReference dbRef;
    private FirebaseAuth auth;
    private string userID, frndID;
    public GameObject tradeFrnd,viewTradeReqst, proposedPanel, demandedPanel;

    public string[] coinCount = new string[5];
    public string[] indexCount = new string[5];
    public string userName;


    public CoinData[] proposedData = new CoinData[5];
    public CoinData[] demandedData = new CoinData[5];






    // Start is called before the first frame update
    void Awake()
    {
        
        auth = Login.Instance.auth;
        dbRef = DataSaver.Instance.dbRef;
        ID = auth.CurrentUser.UserId;
        userID = ID;
    }

    private void OnEnable()
    {
        ButtonEffect(0);
        // LoadTradesList();
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

        Debug.LogWarning(message: $"dummy Failed to load  with {DBTask_1.Exception}");

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

            if (level == DataBase.LevelUp)
            {

                Debug.LogError("User ID: " + id + " = " + level + " is matched.");
                GameObject requestItem = Instantiate(isRequest ? tradeFrnd : tradeFrnd, content);
                TradeRequestItem item = requestItem.GetComponent<TradeRequestItem>();

                item.Initialize(id, username);
                userName = username;


            }
            else
            {
                Debug.LogError("User ID: " + id + " = " + level + " did not match.");
            }

        }
    }



    public void LoadTradeRequests(Transform requestListContent)
    {
        StartCoroutine(LoadingTradeRequest("TradeRequest",requestListContent, false ));
    }
    //function to load the list of trade request sent from the frnds
    private IEnumerator LoadingTradeRequest(string childNode, Transform content, bool isRequest)
    {
        Task<DataSnapshot> DBTask = DataSaver.Instance.dbRef.Child("users").Child(userID).Child(childNode).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to load {childNode} with {DBTask.Exception}");
            yield break; // Exit if there's an error
        }

        DataSnapshot snapshot = DBTask.Result;


        Debug.Log("yeah i am in! " + snapshot);
        // Clear existing UI elements
        ClearContent(content);

        foreach (DataSnapshot childSnapshot in snapshot.Children)
        {
            string id = childSnapshot.Key.ToString();
            string username;
           

            //string level = childSnapshot.Value.ToString();
           
            Task<DataSnapshot> getUserName = DataSaver.Instance.dbRef.Child("users").Child(id).Child("userName").GetValueAsync(); // Fetch online status
            yield return new WaitUntil(predicate: () => getUserName.IsCompleted);

            if (getUserName.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to load  with {getUserName.Exception}");
                yield break; // Exit if there's an error
            }
            
            DataSnapshot frndUserName = getUserName.Result;
            username = frndUserName.Value.ToString();

            Debug.LogError("-----------user ID " + id + " UserNAme: " + username);

            // int level = int.Parse(frndUserName.Value.ToString());
            // Debug.LogError("Level UP VALUE: " + level);

            //if (level == DataBase.LevelUp)
            //{
            //    Debug.LogError("User ID: " + id + " = " + level + " is matched.");
            GameObject requestItem = Instantiate(isRequest ? viewTradeReqst : viewTradeReqst, content);
            ViewTradeRequest item = requestItem.GetComponent<ViewTradeRequest>();

            item.Initialize(id, username);
            userName = username;
            //}
            //else
            //{
            //    Debug.LogError("User ID: " + id + " = " + level + " did not match.");
            //}

        }
    }


    public void GetFriendData(string tri)
    {
        StartCoroutine(GetFrndData(tri));
        frndID = tri;
    }
    //called on the trade button when u find a frnd and wish to trade with him
    public IEnumerator GetFrndData(string tri)
    {
       
        for (int i = 1; i <= 5; i++)
        {


            Task<DataSnapshot> frndLevelTask = DataSaver.Instance.dbRef.Child("users").Child(tri).Child("Coins").Child(GetIndex(DataBase.LevelUp, i)).GetValueAsync();
            yield return new WaitUntil(predicate: () => frndLevelTask.IsCompleted);

            if (frndLevelTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to load  with {frndLevelTask.Exception}");
                yield break; // Exit if there's an error
            }
            else
            {
                Debug.Log("TAsk Result for frnd coins:  " + frndLevelTask.Result);
                coinCount[i - 1] = frndLevelTask.Result.Value.ToString();
                indexCount[i - 1] = frndLevelTask.Result.Key;
            }

        }

        proposedPanel.SetActive(true);

    }

    public void ViewTradeRequestData(string id)
    {
        StartCoroutine(GetTradeRequestData(id));
        frndID = id;
    }

    // view the request of trade if any present there
    public IEnumerator GetTradeRequestData(string id)
    {

      

        // checking the traderequest in the user database
        Task<DataSnapshot> tradeRequestData = DataSaver.Instance.dbRef.Child("users").Child(userID).Child("TradeRequest").Child(id).GetValueAsync();

        yield return new WaitUntil(predicate: () => tradeRequestData.IsCompleted);



        if (tradeRequestData.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to load  with {tradeRequestData.Exception}");
            yield break; // Exit if there's an error
        }

        else if (tradeRequestData.IsFaulted)
        {
            Debug.Log(tradeRequestData.Exception);
        }
        else if (tradeRequestData.IsCompleted)
        {

            DataSnapshot r_Snap = tradeRequestData.Result;
          
            Debug.Log(r_Snap);

            bool indexValueofCoin = false;
            // getting the result of trade request and store them in the demanded and proposed array respectivly
            // loop of 5 items because we have only 5 items per level
            for (int i = 0; i < 5; i++)
            {
                // a bool check for differentiate the index and value on each iteration because database data structure is saved in this order
                indexValueofCoin = false;
                foreach (DataSnapshot snap in r_Snap.Child("demandedItems").Child(i.ToString()).Children)
                {

                    if (!indexValueofCoin)
                    {
                        demandedData[i].coinIndex = int.Parse(snap.Value.ToString()); 
                        Debug.Log(" -----  " + snap.Value); 
                    }
                    else
                    {
                        demandedData[i].value = int.Parse( snap.Value.ToString());
                        Debug.Log( " -----  " + snap.Value); 
                    }

                    indexValueofCoin = true;

                }
                indexValueofCoin = false;
                foreach (DataSnapshot snap in r_Snap.Child("proposedItems").Child(i.ToString()).Children)
                {

                    if (!indexValueofCoin)
                    {
                        proposedData[i].coinIndex = int.Parse(snap.Value.ToString());
                        Debug.Log(" -----  " + snap.Value);
                    }
                    else
                    {
                        proposedData [i].value = int.Parse(snap.Value.ToString());
                        Debug.Log(" -----  " + snap.Value);
                    }


                    indexValueofCoin = true;
                }


                
            }
            Debug.Log("out !" + r_Snap.Child("demandedItems"));

        }

        // getting the other user coins total value he has
        for (int i = 1; i <= 5; i++)
        {
            Task<DataSnapshot> frndLevelTask = DataSaver.Instance.dbRef.Child("users").Child(id).Child("Coins").Child(GetIndex(DataBase.LevelUp, i)).GetValueAsync();
            yield return new WaitUntil(predicate: () => frndLevelTask.IsCompleted);

            if (frndLevelTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to load  with {frndLevelTask.Exception}");
                yield break; // Exit if there's an error
            }
            else
            {
                Debug.Log("TAsk Result for frnd coins:  " + frndLevelTask.Result);
                coinCount[i - 1] = frndLevelTask.Result.Value.ToString();
               
            }


        }


        demandedPanel.SetActive(true);



    }




  //-------------------- trade request sending logic --------------------

    public void TradeSent()
    {
        SendTradeRequest(userID, frndID, proposedData, demandedData);
    
    }



    //called to send the trade request to the other frnd data base.
    public async void SendTradeRequest(string senderId, string receiverId, CoinData[] proposedItems, CoinData[] demandedItems)
    {
        if (dbRef == null)
        {
            Debug.LogError("Database reference (dbRef) is null.");
            return;
        }

        if (proposedItems == null || proposedItems.Length != 5 || demandedItems == null || demandedItems.Length != 5)
        {
            Debug.LogError("Proposed or demanded items array is null or does not contain exactly 5 items.");
            return;
        }

        var tradeData = new Dictionary<string, object>
    {
        { "senderId", senderId }, // Store sender ID in the trade data
        { "proposedItems", ConvertCoinDataArrayToList(proposedItems) },
        { "demandedItems", ConvertCoinDataArrayToList(demandedItems) },
       
    };

        try
        {
            // Generate a unique key for each trade request
            string uniqueTradeKey = dbRef.Child("users").Child(receiverId).Child("TradeRequest").Push().Key;

            // Use the unique key to store the trade request
            string tradePath = $"users/{receiverId}/TradeRequest/{senderId}";

            await dbRef.Child(tradePath).SetValueAsync(tradeData);
            Debug.Log("Trade request sent successfully with unique key: " + uniqueTradeKey);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error sending trade request: " + ex.Message);
        }
    }

    //list of dictionary to convert the coin data array to list 
    private List<Dictionary<string, object>> ConvertCoinDataArrayToList(CoinData[] coinDataArray)
    {
        var list = new List<Dictionary<string, object>>();

        foreach (var coin in coinDataArray)
        {
            list.Add(new Dictionary<string, object>
        {
            { "coinIndex", coin.coinIndex },
            { "value", coin.value }
        });
        }

        return list;
    }

    //called the read the trade request of proposed and demanded coins
    public async void ReadTradeRequests(string receiverId)
    {
        if (dbRef == null)
        {
            Debug.LogError("Database reference (dbRef) is null.");
            return;
        }

        try
        {
            // Reference to the receiver's trade requests
            var tradeRequestsRef = dbRef.Child("users").Child(receiverId).Child("TradeRequest");

            // Get the trade requests data
            var snapshot = await tradeRequestsRef.GetValueAsync();

            if (snapshot.Exists)
            {
                foreach (var childSnapshot in snapshot.Children)
                {
                    string senderId = childSnapshot.Key; // Sender ID as the key

                    // Retrieve trade details
                    var tradeData = childSnapshot.Value as Dictionary<string, object>;
                    if (tradeData != null)
                    {
                        Debug.Log($"Trade Request from Sender: {senderId}");

                        // Retrieve proposed items
                        if (tradeData.TryGetValue("proposedItems", out var proposedItemsData))
                        {
                            var proposedItems = ConvertToCoinDataList(proposedItemsData as List<object>);
                            Debug.Log("Proposed Items:");
                            foreach (var item in proposedItems)
                            {
                                Debug.Log($"Index: {item.coinIndex}, Value: {item.value}");
                            }
                        }

                        // Retrieve demanded items
                        if (tradeData.TryGetValue("demandedItems", out var demandedItemsData))
                        {
                            var demandedItems = ConvertToCoinDataList(demandedItemsData as List<object>);
                            Debug.Log("Demanded Items:");
                            foreach (var item in demandedItems)
                            {
                                Debug.Log($"Index: {item.coinIndex}, Value: {item.value}");
                            }
                        }

                        // Additional fields like timestamp can also be logged
                        if (tradeData.TryGetValue("timestamp", out var timestamp))
                        {
                            Debug.Log($"Timestamp: {timestamp}");
                        }
                    }
                }
            }
            else
            {
                Debug.Log("No trade requests found for this receiver.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error reading trade requests: " + ex.Message);
        }
    }

    // convert the coin data list to value 
    private List<CoinData> ConvertToCoinDataList(List<object> dataList)
    {
        var coinDataList = new List<CoinData>();

        foreach (var item in dataList)
        {
            if (item is Dictionary<string, object> coinDataDict &&
                coinDataDict.TryGetValue("coinIndex", out var coinIndex) &&
                coinDataDict.TryGetValue("value", out var value))
            {
                coinDataList.Add(new CoinData(Convert.ToInt32(coinIndex), Convert.ToInt32(value)));
            }
        }

        return coinDataList;
    }


    //------------------------ swap or reject logic --------------------------------

    public  void SwapAction()
    {
        for (int i = 0; i < 5; i++)
        {
            DataBase.SetCoins(proposedData[i].coinIndex, proposedData[i].value);
            DataBase.SetCoins(demandedData[i].coinIndex, -demandedData[i].value);
           
        }
        StartCoroutine(SwapFrndData(frndID));
        //  await dbRef.Child("users").Child(userID).Child("TradeRequest").Child(frndID).RemoveValueAsync();
    }

    public async void Reject()
    {
        await dbRef.Child("users").Child(userID).Child("TradeRequest").Child(frndID).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error removing friend request: " + task.Exception);
                // Handle error (e.g., show a message to the user)
            }
            else
            {
                Debug.Log("Friend request from " + " removed successfully.");
                // Optionally, you can notify the UI or perform additional actions here
            }
        });
        
    }

    public IEnumerator SwapFrndData(string id)
    {
       // dbRef.Child("users").Child(userID).Child("TradeRequest").Child(id).RemoveValueAsync(); 
        int[] currentCoin = new int[5];
      

        for (int i = 0; i < 5; i++)
        {
            Task<DataSnapshot> CoinTask = dbRef.Child("users").Child(id).Child("Coins").Child(proposedData[i].coinIndex.ToString()).GetValueAsync();
            yield return new WaitUntil(predicate: () => CoinTask.IsCompleted);

            DataSnapshot coin = CoinTask.Result;

            currentCoin[i] = int.Parse(coin.Value.ToString());

            currentCoin[i] = currentCoin[i] + demandedData[i].value;

            currentCoin[i] = currentCoin[i] - proposedData [i].value;


           
        }

        var updates = new Dictionary<string, object>
        {
            { proposedData[0].coinIndex.ToString() , currentCoin[0] },
            { proposedData[1].coinIndex.ToString() , currentCoin[1] },
            { proposedData[2].coinIndex.ToString() , currentCoin[2] },
            { proposedData[3].coinIndex.ToString() , currentCoin[3] },
            { proposedData[4].coinIndex.ToString() , currentCoin[4] },
        };


        dbRef.Child("users").Child(id).Child("Coins").UpdateChildrenAsync(updates);
        dbRef.Child("users").Child(userID).Child("TradeRequest").Child(id).RemoveValueAsync();

        //UpdateCoin(id, updates);
        

    }

     public async void  UpdateCoin(string id, Dictionary<string , object> updates)
    {
       await dbRef.Child("users").Child(id).Child("Coins").UpdateChildrenAsync(updates);

        await dbRef.Child("users").Child(userID).Child("TradeRequest").Child(id).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error removing friend request: " + task.Exception);
                // Handle error (e.g., show a message to the user)
            }
            else
            {
                Debug.Log("Friend request from " +  " removed successfully.");
                // Optionally, you can notify the UI or perform additional actions here
            }
        });
    }




    // get the index of the coins from the data base
    public string GetIndex(int R, int C)
    {
        int num1;
        num1 = ((R - 1) * 5) + (C - 1);
        return num1.ToString();
    }

    // reverse value and get the row and column nb from the index
    public (int Row, int Column) GetRowColumn(int index)
    {
        int row, col;
        row = ((index - 1) / 5) + 1; // Correct row calculation
        col = (index % 5 == 0) ? 5 : (index % 5); // Handle boundary condition
        return (row, col);
    }

    // identify the coin type by the row nb based on database . level up
    public int GetCollectionType(int row)
    {
        return ((row - 1) / 14 + 1);
    }

    public void test(int num)
    {


        Debug.Log("Grid # of Selected Index: " + GetRowColumn(num) + "  Collection type of selected Row: " + GetCollectionType(GetRowColumn(num).Row));
    }

    //accept the trade request and remove from the database
    public void AcceptTrade()
    { 

    }

    //reject the trade request and remove form the database
    public void RejectTrade()
    {

    }


    private void ClearContent(Transform content)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }


}



[System.Serializable]
public class CoinData
{
    public int coinIndex; // e.g., index on which coin is stored"
    public int value;       // e.g., value of the coin

    public CoinData(int coinIndex, int value)
    {
        this.coinIndex = coinIndex;
        this.value = value;
    }
}

[System.Serializable]
public class TradeRequest
{
   
    public CoinData[] coins; // Array of different coins

    public TradeRequest( CoinData[] coins)
    {
       
        this.coins = coins;
    }
}


