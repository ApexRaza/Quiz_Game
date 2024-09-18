using Firebase.Storage;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FirebaseImageLoader : MonoBehaviour
{


    private string firebaseStoragePath = "gs://swipequiz-58284.appspot.com/1.HISTORYQ1.png"; // Firebase Storage path to the image
    public Image uiImage; // Reference to the UI Image in Unity where the image will be displayed

    private FirebaseStorage storage;

    void Start()
    {
        // Initialize Firebase Storage
        storage = FirebaseStorage.DefaultInstance;

        // Start the process of loading and displaying the image
        StartCoroutine(LoadImageFromFirebase());
    }

    IEnumerator LoadImageFromFirebase()
    {
        // Get reference to the image in Firebase Storage
        StorageReference storageRef = storage.GetReference(firebaseStoragePath);

        // Get the download URL
        var downloadUrlTask = storageRef.GetDownloadUrlAsync();

        yield return new WaitUntil(() => downloadUrlTask.IsCompleted);

        if (downloadUrlTask.IsFaulted || downloadUrlTask.IsCanceled)
        {
            Debug.LogError("Failed to get download URL: " + downloadUrlTask.Exception);
            yield break;
        }

        // Download URL of the image
        string imageUrl = downloadUrlTask.Result.ToString();
        Debug.Log("Image URL: " + imageUrl);

        // Use UnityWebRequest to download the image
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error downloading the image: " + request.error);
        }
        else
        {
            // Get the texture from the downloaded image
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // Convert texture to a sprite and apply it to the UI Image
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

            uiImage.sprite = sprite; // Set the sprite to the UI Image
        }
    }
}
