using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Facebook.Unity;
using System;
using System.Collections;

public class Login : MonoBehaviour
{
    private static Login instance;
    public static Login Instance;
    public GameObject dbObj;
    public string webClientId = "841125010072-943riqkh4192cev49ptkugh59m1hbs8i.apps.googleusercontent.com";
    
    private GoogleSignInConfiguration configuration;
    public FirebaseAuth auth;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Instance = instance;
            DontDestroyOnLoad(gameObject);
        }
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            Debug.Log("Initialize the Facebook SDK");
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            Debug.Log("Already initialized, signal an app activation App Event");
            FB.ActivateApp();
        }
        auth = FirebaseAuth.DefaultInstance;
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("FirstTime")) 
        {
            GuestLogin();
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.Save();
        }
        else 
        {
            Debug.Log("Start else");
            CheckSignInStatus();
        }
    }

    ////..........................................Guest SignIn............................................................./////

    public async void GuestLogin()
    {
        await AnonymousLoginBtn();
    }

    async Task AnonymousLoginBtn()
    {
        await auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCompleted) 
            {
                AuthResult result = task.Result;
                
                if (PlayerPrefs.HasKey("FirstTimeAnom"))
                {
                    dbObj.SetActive(true);
                    DataSaver.Instance.SaveUsername();
                    DataSaver.Instance.SaveData();
                    Debug.Log("isAnom: " + result.User.IsAnonymous);
                    Debug.Log("FirstTimeAnom value: " + PlayerPrefs.GetInt("FirstTimeAnom"));
                    LoginSuccess(result.User.UserId);
                }
                else
                {
                    Debug.Log("FirstTimeAnom");
                    PlayerPrefs.Save();
                    PlayerPrefs.SetInt("FirstTimeAnom", 1);
                }
            }
        });

        //string userId = SystemInfo.deviceUniqueIdentifier;
        //Invoke(nameof(GuestLoginSuccess), 1f);
    }


    ////..........................................Google LogIn.............................................................////

    public void AnomLinkGoogle()
    {
        DataSaver.Instance.LoadData();
        DataSaver.Instance.dbRef.Child("users").Child(DataSaver.Instance.userID).RemoveValueAsync();
        GoogleLogin();
        DataSaver.Instance.SaveData();
    }

    public void GoogleLogin()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished, TaskScheduler.Default);
        Debug.Log("GoogleLogin() function UserID" + auth.CurrentUser.UserId);
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            dbObj.SetActive(true);
            DataBase.UserName = task.Result.DisplayName;
            CheckUserDataExists(task.Result.UserId);

            // Authenticate with Firebase
            //GoogleAuth(task.Result.IdToken);
        }
    }

    private void GoogleAuth(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Firebase sign-in failed: " + task.Exception);
                return;
            }

            FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                Debug.Log("isAnom: " + user.IsAnonymous);
                dbObj.SetActive(true);
                CheckUserDataExists(user.UserId);
            }
            else
            {
                // No user is signed in
                Debug.Log("No user is signed in.");
            }
            Debug.Log("Firebase user signed in successfully: " + user.DisplayName);
            //DataSaver.Instance.SaveData();
        });
    }

    public void GoogleSignOut()
    {
        Debug.Log("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    ////..........................................Facebook LogIn............................................................./////
    ///
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            Debug.Log("Signal an app activation App Event");
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Debug.Log("Pause the game - we will need to hide");
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Debug.Log("Resume the game - we're getting focus again");
            Time.timeScale = 1;
        }
    }

    public void AnomLinkFB()
    {
        //var perms = new List<string>() { "public_profile", "email" };
        /// FB.LogInWithPublishPermissions(perms, AnonFBAuthCallback);
        PlayerPrefs.SetInt("AnomLinkFB", 1);
        FBLogin();
    }

    public void FBLogin() 
    {
        var perms = new List<string>() { "public_profile", "email" };
        Debug.Log("FB Login perms" + perms);
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
            Debug.Log("Access Token" + aToken);
            if (PlayerPrefs.HasKey("AnomLinkFB")) 
            {
                PlayerPrefs.DeleteKey("AnomLinkFB");
                AnomLinkFBAuth(aToken);
            }
            else 
            {
                FBAuth(aToken);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    public async void AnomLinkFBAuth(string accessToken)
    {
        Credential credential = FacebookAuthProvider.GetCredential(accessToken);

        await auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWithOnMainThread(task => {

            try
            {
                AuthResult result = task.Result;
                FB.API("/me?fields=name", HttpMethod.GET, response =>
                {
                    string facebookName = response.ResultDictionary["name"] as string;
                    UserProfile profile = new UserProfile { DisplayName = facebookName };
                    result.User.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(profileUpdateTask => {
                        if (profileUpdateTask.IsCompleted)
                        {
                            Debug.Log("User profile updated with Facebook name: " + result.User.DisplayName);
                            DataBase.UserName = result.User.DisplayName;
                            DataSaver.Instance.SaveData();
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                Debug.Log("FB SignIn Failed. " + ex.Message);
            }
        });
    }

    private async void FBAuth(string accessToken) 
    {
        Credential credential = FacebookAuthProvider.GetCredential(accessToken);
        await auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
            try 
            {
                AuthResult result = task.Result;

                Debug.Log("FB SignIn. ");
                dbObj.SetActive(true);

                DataBase.UserName = result.User.DisplayName;
                CheckUserDataExists(result.User.UserId);
            }
            catch (Exception ex)
            {
                Debug.Log("FB SignIn Failed. " + ex.Message);
            }
        });
    }

    public void GetFacebookFriends()
    {
        FB.API("/me/friends", HttpMethod.GET, FriendsCallback);
    }

    private void FriendsCallback(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError("Error getting friends: " + result.Error);
            return;
        }

        var friends = result.ResultDictionary["data"] as List<object>;
        var friendList = new List<Dictionary<string, object>>();

        foreach (var friend in friends)
        {
            friendList.Add(friend as Dictionary<string, object>);
        }

        // Show these friends in your game's UI
        Friendlist.Instance.DisplayFBFriends(friendList);
    }


    ////..........................................Other.............................................................////

    private void CheckSignInStatus()
    {
        if(auth.CurrentUser != null) 
        {
            // User is signed in
            Debug.Log("User is already signed in: " + auth.CurrentUser.DisplayName);
            Debug.Log("Is User Anom: " + auth.CurrentUser.IsAnonymous);
            dbObj.SetActive(true);
            StartCoroutine(LoadDataAndChangeScene());
        }
        else 
        {
            // No user is signed in
            Debug.Log("No user is signed in.");
            dbObj.SetActive(false);
            return;
        }
    }

    private void CheckUserDataExists(string userId)
    {
        DataSaver.Instance.dbRef.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log("User data exists, loading data...");
                    StartCoroutine(LoadDataAndChangeScene());
                    //DataSaver.Instance.LoadData();
                    //LoginSuccess(userId);
                }
                else
                {
                    Debug.Log("User data does not exist, saving new data...");

                    DataSaver.Instance.SaveData();
                    LoginSuccess(userId);
                }
            }
            else
            {
                Debug.LogError("Failed to check user data existence.");
            }
        });
    }

    private IEnumerator LoadDataAndChangeScene()
    {
        // Start the LoadData coroutine and wait for it to finish
        Debug.Log("Start the LoadData coroutine and wait for it to finish");
        yield return StartCoroutine(DataSaver.Instance.LoadDataEnum());
        LoginSuccess(auth.CurrentUser.UserId);
    }

    private void LoginSuccess(string id)
    {
        Debug.Log("Login Successful");
        SceneChange();
    }

    public void SceneChange()
    {
        Debug.Log("SceneChange");
        SceneManager.LoadScene(1);
    }
}
