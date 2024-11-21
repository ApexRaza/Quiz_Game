using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class TradeUIManager : MonoBehaviour
{
    public GameObject[] bgButtons, frontEndButtons;
    public string ID, index;

    // Start is called before the first frame update
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
    public void LoadFriendsList()
    {
        StartCoroutine(LoadFriendData());
    }

    private IEnumerator LoadFriendData()
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
}
