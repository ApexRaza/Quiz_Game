using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public GameObject congo;
    public GameObject dbObj;
    public string webClientId = "243769013671-8otfe2at2sb2s5ch7fskgpohv5he8med.apps.googleusercontent.com";
    
    private GoogleSignInConfiguration configuration;
    private FirebaseAuth auth;
    public Text userNameText;
    /// <Test>
    public void sceneChange() 
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
        CheckSignInStatus();
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

            print("Login Success");

            AuthResult result = task.Result;
            print("Guest name: " + result.User.DisplayName);
            print("Guest Id: " + result.User.UserId);
            //can save user id in playerprefs
            GuestLoginSuccess(result.User.UserId);
            congo.SetActive(true);
            dbObj.SetActive(true);
            DataSaver.Instance.SaveData();
            // Update the Text component with the user's name
            if (userNameText != null)
            {
                userNameText.text = "Welcome, " + result.User.UserId + "!";
            }
        });

        //string userId = SystemInfo.deviceUniqueIdentifier;
        //Invoke(nameof(GuestLoginSuccess), 1f);
    }

    void GuestLoginSuccess(string id)
    {
        Debug.Log("Guest Login Successful");
    }

    ////..........................................Google SignIn.............................................................////
    public void AnomLinkGoogle() 
    {
        DataSaver.Instance.dbRef.Child("users").Child(DataSaver.Instance.userID).RemoveValueAsync();
        OnGoogleSignIn();
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
            congo.SetActive(true);
            dbObj.SetActive(true);
            DataSaver.Instance.SaveData();
            if (userNameText != null)
            {
                userNameText.text = "Welcome, " + task.Result.DisplayName + "!";
            }

            // Authenticate with Firebase
            AuthenticateWithFirebase(task.Result.IdToken);
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

            FirebaseUser user = task.Result;
            Debug.Log("Firebase user signed in successfully: " + user.DisplayName);
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
                congo.SetActive(true);
                dbObj.SetActive(true);
                DataSaver.Instance.SaveData();
                if (userNameText != null)
                {
                    userNameText.text = "Welcome, " + user.DisplayName + "!";
                }
            }
            else
            {
                // No user is signed in
                Debug.Log("No user is signed in.");
                congo.SetActive(false);
                dbObj.SetActive(false);
            }
        }
        else 
        {
            return;
        }
    }
}
