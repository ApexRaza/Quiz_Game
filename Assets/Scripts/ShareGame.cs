using UnityEngine;

public class ShareGame : MonoBehaviour
{
    public string gameLink;
    public void ShareGameLink()
    {
        string shareMessage = "Check out this game: " + gameLink;
        string url = gameLink;

        // Create a new intent for sharing
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage + " " + url);
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");

        // Create chooser
        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share via");
        AndroidJavaObject currentActivity = GetCurrentActivity();
        currentActivity.Call("startActivity", chooser);
    }

    private AndroidJavaObject GetCurrentActivity()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }
}
