using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;


[System.Serializable]
public class NetworkData
{
    public string question;
    public Image image;
    public string correctAns;
}
public class NetworkQuizHandler : MonoBehaviour
{
    CollectionsSO collectionSO;
    QuizManager nQuizManager;


    [Space(2)]
    public GameObject QuestionPanel, correctAns, incorrectAns, ansObjects, loadingQ, outOflivePanel;
    public TextMeshProUGUI questionTxt, correctTxt;
    public Image questionImage;


    public List<NetworkData> networkData;// = new List<NetworkData>();

    public ProgressState progressState;
    public TImer timer;
    string[] game_ticket_id;
    int quizCount = 0;
    int rightAnswerCount = 0;
    int wrongAnswerCount = 0;

    public AudioSource swipeAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        collectionSO = Resources.Load<CollectionsSO>("Scriptables/Collection");
        nQuizManager = Resources.Load<QuizManager>("Scriptables/QuizManager");
        nQuizManager.QuizTypesInit();

        nQuizManager.SetQuizType(QuizType.Varia);
    }

    public void QuizNo()
    {
        quizCount = 0;
    }
    public void setQuizType()

    {
        setQuizTypeRPC();
    }

    public void setQuizTypeRPC()
    {
        int num = Random.Range(0, 16);
        SetQuestionCategory(num);
        OpenquestionPanel();
    }


    // setting the two different categories selected by the user
    public void SetQuestionCategory(int category)
    {
        nQuizManager.type = category;
    }

    public void CheckAns(string ans)
    {
        if (ans == nQuizManager.quizType[nQuizManager.type].quizData[num].rightAnswer)
        {
            RightAns();
            StartCoroutine(nameof(Next));
        }
        else
        {
            WrongAnss();
            StartCoroutine(nameof(WrongAns));
        }
    }

    void RightAns()
    {

        swipeAudioSource.Play();
    }

    void WrongAnss()
    {
        swipeAudioSource.Play();
    }
    public IEnumerator Next()
    {
        //Debug.Log("run");
        rightAnswerCount++;
        correctTxt.gameObject.SetActive(true);
        correctAns.SetActive(true);
        ansObjects.SetActive(false);
        yield return new WaitForSeconds(2);
        correctTxt.gameObject.SetActive(false);
        correctAns.SetActive(false);
        quizCount++;
        if(quizCount < 3) 
        {
            networkData[quizCount].image.gameObject.SetActive(true);
            networkData[quizCount - 1].image.gameObject.SetActive(false);
            questionTxt.text = networkData[quizCount].question.ToString();
            questionTxt.gameObject.SetActive(true);
            ansObjects.SetActive(true);
            timer.ResetTimer();
        }
        else 
        {
            timer.ResetTimer();
            quizCount = 0;
            ResetState();
            QuestionPanel.SetActive(false);
        }
    }
    public IEnumerator WrongAns()
    {
        wrongAnswerCount++;
        correctTxt.gameObject.SetActive(true);
        incorrectAns.SetActive(true);
        ansObjects.SetActive(false);

        yield return new WaitForSeconds(2);
        correctTxt.gameObject.SetActive(false);
        incorrectAns.SetActive(false);
        quizCount++;
        if (quizCount < 3)
        {
            networkData[quizCount].image.gameObject.SetActive(true);
            networkData[quizCount - 1].image.gameObject.SetActive(false);
            questionTxt.text = networkData[quizCount].question.ToString();
            questionTxt.gameObject.SetActive(true);
            ansObjects.SetActive(true);
            timer.ResetTimer();
        }
        else
        {
            timer.ResetTimer();
            quizCount = 0;
            ResetState();
            QuestionPanel.SetActive(false);
        }
    }
    IEnumerator StartQuiz()
    {
        questionTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        ansObjects.SetActive(true);
        timer.ResetTimer();
    }

    void ResetState()
    {
        correctTxt.gameObject.SetActive(false);
        incorrectAns.gameObject.SetActive(false);
        questionTxt.gameObject.SetActive(false);
        ansObjects.SetActive(false);
        correctAns.SetActive(false);
    }

   
    IEnumerator LoadImage1(string imageUrl, Image img)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);


            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

            // StartCoroutine(StartQuiz());
        }
    }
    IEnumerator LoadImage(string imageUrl, Image img)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);

          
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
          
           // StartCoroutine(StartQuiz());
        }
    }

    IEnumerator LoadingWait()
    {
        loadingQ.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        progressState.gameObject.SetActive(false);
        QuestionPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        loadingQ.SetActive(false);
    }

    int num = 0;
    public void OpenquestionPanel()
    {
        ResetState();
        for (int i = 0; i < 3; i++) 
        {
            
            networkData[i].question = nQuizManager.quizType[nQuizManager.type].quizData[i].question.ToString();
            networkData[i].correctAns = nQuizManager.quizType[nQuizManager.type].quizData[i].correctAns.ToString();
            StartCoroutine(LoadImage(nQuizManager.quizType[nQuizManager.type].quizData[i].imageLink, networkData[i].image));
        }
        DisplayQuestion();
    }
    public void DisplayQuestion() 
    {
        networkData[0].image.gameObject.SetActive(true);
        questionTxt.text = networkData[0].question.ToString();
        questionTxt.gameObject.SetActive(true);
        ansObjects.SetActive(true);
        timer.ResetTimer();
    }
}
