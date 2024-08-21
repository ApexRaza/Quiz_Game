using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using Firebase.Database;

public class Login : MonoBehaviour
{
    public GameObject dbObj;
    public string webClientId = "841125010072-943riqkh4192cev49ptkugh59m1hbs8i.apps.googleusercontent.com";
    
    private GoogleSignInConfiguration configuration;
    private FirebaseAuth auth;

    /// <Test>
    public void SceneChange() 
    {
        SceneManager.LoadScene(1);
    }
    /// </Test>
    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };
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

    void LoginSuccess(string id)
    {
        Debug.Log("Login Successful");
        SceneChange();
    }

    ////..........................................Google SignIn.............................................................////
    public void AnomLinkGoogle() 
    {
        DataSaver.Instance.LoadData();
        DataSaver.Instance.dbRef.Child("users").Child(DataSaver.Instance.userID).RemoveValueAsync();
        OnGoogleSignIn();
        DataSaver.Instance.SaveData();
    }
    public void OnGoogleSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished, TaskScheduler.Default);
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
            //if (DataSaver.Instance.dbRef.Child("users").Child(task.Result.UserId) == null) 
            //{
            //    DataSaver.Instance.SaveData();
            //}
            //else 
            //{
            //    DataSaver.Instance.LoadData();
            //}
            CheckUserDataExists(task.Result.UserId);
            //LoginSuccess(task.Result.UserId);
            //DataSaver.Instance.SaveData();
            // Authenticate with Firebase
            //AuthenticateWithFirebase(task.Result.IdToken);
        }
    }
    private void AuthenticateWithFirebase(string idToken)
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
                    DataSaver.Instance.LoadData();
                    LoginSuccess(userId);
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

    public void OnGoogleSignOut()
    {
        Debug.Log("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    private void CheckSignInStatus()
    {

        if(auth.CurrentUser != null) 
        {
            FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                // User is signed in
                Debug.Log("User is already signed in: " + user.DisplayName);
                dbObj.SetActive(true);
                DataSaver.Instance.LoadData();
                SceneChange();
            }
            else
            {
                // No user is signed in
                Debug.Log("No user is signed in.");
                dbObj.SetActive(false);
            }
        }
        else 
        {
            return;
        }
    }
}
