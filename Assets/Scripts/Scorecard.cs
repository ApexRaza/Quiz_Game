using Firebase.Database;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Scorecard : MonoBehaviour
{
    private DatabaseReference db;
    [Header("UserData")]
    public GameObject scoreElement;
    public Transform scoreboardContent;

    private void Start()
    {
        db = DataSaver.Instance.dbRef;
    }

    public void ScoreboardButton()
    {
        StartCoroutine(LoadScoreboardData());
    }
    private IEnumerator LoadScoreboardData()
    {
        //Get all the users data ordered by kills amount
        Task<DataSnapshot> DBTask = db.Child("users").OrderByChild("Dollars").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("userName").Value.ToString();
                int xp = int.Parse(childSnapshot.Child("Dollars").Value.ToString());

                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, xp);
            }
        }
    }
}
