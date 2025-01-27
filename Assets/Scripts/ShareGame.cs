using Photon.Pun.UtilityScripts;
using UnityEditor;
using UnityEngine;

public class ShareGame : MonoBehaviour
{
    public string gameLink;
    public string roomID;
    public ConnectAndJoinRandom connectPun;
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void ShareTextWithURL(string message, string url);
#endif

    public void ShareGameLink()
    {
        roomID = DataSaver.Instance.userID;
        string shareMessage = "Check out this game: " + gameLink;
        string url = gameLink + "?roomID=" + roomID; 
        connectPun.CreateRoomByID(roomID);
        Debug.Log(roomID);

#if UNITY_ANDROID
        StartAndroidShare(url, shareMessage);
#elif UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                ShareTextWithURL(shareMessage, gameLink);
            }
#else
        Debug.Log("Sharing not supported on this platform");
#endif
        
    }

    void StartAndroidShare(string link, string message) 
    {
        // Create a new intent for sharing
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message + " " + link);
        
        // Create chooser
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share via");
        AndroidJavaObject currentActivity = GetCurrentActivity();
        currentActivity.Call("startActivity", chooser);
    }
    private AndroidJavaObject GetCurrentActivity()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }



    public void Test(string id)
    {
        connectPun.JoinRoombyID(id);
    }

}
