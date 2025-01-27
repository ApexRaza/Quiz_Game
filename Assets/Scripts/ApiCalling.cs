
using System.Collections.Specialized;
using UnityEngine;

using System;
using System.Web;
using Photon.Pun.UtilityScripts;



public class ApiCalling : MonoBehaviour
{
    
    public string[] roomID;
    public ConnectAndJoinRandom connectPun;

    void Start()
    {
        
        GetIDFromURL();
    }
    void GetIDFromURL()
    {

        string url = "https://quizapex.itch.io/quizwebgl?roomID=Kandal2341231231jkadfadfajkfa";//Application.absoluteURL;

        Uri uri = new Uri(url);

        // Extract the query string
        string query = uri.Query;
        NameValueCollection queryParams = HttpUtility.ParseQueryString(query);

        // Extract the  parameters 
        string[] refValues = queryParams.GetValues("roomID");


        if (refValues != null)
        {
            roomID = new string[refValues.Length];

            for (int i = 0; i < refValues.Length; i++)
            {
                

                roomID[i] = refValues[i].ToString();
            }
        }
        else
        {
            Debug.Log("No parameters found.");
        }


        connectPun.JoinRoombyID(roomID[0]);

    }



  
}
