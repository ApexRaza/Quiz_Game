using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleSheetsPublicReader : MonoBehaviour
{
    // Replace with your spreadsheet ID
    private string spreadsheetID = "1XXVCYrC35knVzHYS4q_APg4MwbzHeN5Ovi2MJfAkc4E";
    public string spreadSheetNb = "0";
    public GameObject imagePrefab;
    public Transform parentTransform;
    QuizManager quizManager;



    int id = 0;

    void Awake()
    {
        quizManager = Resources.Load<QuizManager>("Scriptables/QuizManager");

       
        

        id = 0;
        spreadSheetNb = quizManager.gridId[id].ToString();
        StartCoroutine(ReadSheetData());

    }

    IEnumerator ReadSheetData()
    {

        Debug.Log(spreadSheetNb + "    " + id);

        string url = $"https://docs.google.com/spreadsheets/d/{spreadsheetID}/export?format=csv&gid={spreadSheetNb}";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string csvData = www.downloadHandler.text;
            ProcessCSV(csvData);
            
        }
    }

    void ProcessCSV(string csvData)
    {
        string[] lines = csvData.Split('\n');
        //foreach (string line in lines.Skip(1))
        // quizManager.quizData[0].Data. = Data[lines.Length];
      //  int startNum=0;
      
       // if (quizManager.quizData[0].Data.Count < lines.Length)
        {
            
            quizManager.quizType[id].quizData.Clear();
            //startNum = quizManager.quizType[quizManager.type].quizData.Count;


            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                

                {
                    quizManager.quizType[id].quizData.Add(mydata((i - 1), values[0], values[1], values[2], bool.Parse(values[3]), values[4], values[5]));
                     
                }


                if (values.Length > 0)
                {

                    string imageUrl = values[4]; // Adjust the index based on the column where the URL is stored
                  
                    if (values[3] == "TRUE" )//||    !string.IsNullOrEmpty(imageUrl))
                    {
                      //  StartCoroutine(LoadImage(imageUrl));
                    }
                }
            }
        }


        if (id < 15)
        {
            id++;
            spreadSheetNb = quizManager.gridId[id].ToString();
            StartCoroutine(ReadSheetData());
        }
    }


    Data mydata(int index, string question, string right, string wrong,bool isImage,string imagelink,string tip)
    {
        Data data = new Data();

        data.question = question;
        data.rightAnswer = right;
        data.wrongAnswer= wrong;
        data.IsImage = isImage;
        data.imageLink = imagelink;
        data.tip = tip;

        return data;

        
    }


    public Image m;
    IEnumerator LoadImage(string imageUrl)
    {
       
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);

            foreach (QuizType q in Enum.GetValues(typeof(QuizType)))
            {
                Debug.Log(q);
            }
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            GameObject newImageObject = Instantiate(imagePrefab, parentTransform);

            
              
            m.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0,0));

           
            newImageObject.GetComponent<RawImage>().texture = texture;
        }
    }

}



















//        string url = "https://docs.google.com/spreadsheets/d/1HtoVqS2HvqJI9eqqQaNkvDzEob-83PfFOR38HAwhoIE/edit?usp=sharing";
