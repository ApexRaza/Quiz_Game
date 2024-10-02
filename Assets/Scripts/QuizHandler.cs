
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;


public class QuizHandler : MonoBehaviour
{
    CollectionsSO collectionSO;
    QuizManager quizManager;
    
    
    [Space(2)]
    public GameObject QuestionPanel,correctAns, incorrectAns, ansObjects, loadingQ, outOflivePanel;
    public TextMeshProUGUI questionTxt,correctTxt;
    public Image questionImage;



    public List<GameObject> contentItem = new List<GameObject>();

    public ProgressState progressState;
    public TImer timer;
    string[] game_ticket_id;
    int quizCount = 0;

    public AudioSource swipeAudioSource;

    // Start is called before the first frame update
    void Start()
    {




        collectionSO = Resources.Load<CollectionsSO>("Scriptables/Collection");
        quizManager = Resources.Load<QuizManager>("Scriptables/QuizManager");
        quizManager.QuizTypesInit();


        quizManager.SetQuizType(QuizType.Varia);

  
    }

    
    // setting the two different categories selected by the user
    public void SetQuestionCategory(int category)
    {

        quizManager.type = category;
      

    }

    public void CheckAns(string ans)
    {
        if (ans == quizManager.quizType[quizManager.type].quizData[num].rightAnswer)
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
        DataBase.Questions += 1;
        DataBase.RightAnswer += 1;
        DataSaver.Instance.SaveData();
        swipeAudioSource.Play();

    }

    void WrongAnss()
    {
        DataBase.Keys += 10;
        DataBase.Lives -= 1;
        DataBase.Questions += 1;
        DataBase.WrongAnswer += 1;
        DataSaver.Instance.SaveData();
        swipeAudioSource.Play();
    }
    public IEnumerator Next()
    {
        Debug.Log("run");
        correctTxt.gameObject.SetActive(true);
        correctAns.SetActive(true);
        ansObjects.SetActive(false);
        yield return new WaitForSeconds(2);

        progressState.gameObject.SetActive(true);
        progressState.UpdateState(true);
        DataBase.Keys += 20;

        int i = DataBase.GetQuiz(quizManager.type);
        i++;
        DataBase.SetQuiz(quizManager.type, i);

        questionImage.enabled = false;
        quizCount++;

      
        if (quizCount > DataBase.QuestionsToTreasure)
        {
            quizCount = 0;

            Debug.Log("Level Cleared treasure Obtained");

        }
        //else
        //    OpenquestionPanel();
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

    public IEnumerator WrongAns()
    {
        correctTxt.gameObject.SetActive(true);
        incorrectAns.SetActive(true);
        ansObjects.SetActive(false);

        yield return new WaitForSeconds(2);
        outOflivePanel.gameObject.SetActive(true);
        progressState.gameObject.SetActive(true);
        QuestionPanel.gameObject.SetActive(false);
        progressState.SetStateFirstTime();

     
    }

    IEnumerator LoadImage(string imageUrl)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();



        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);

            //foreach (QuizType q in Enum.GetValues(typeof(QuizType)))
            //{
            //    Debug.Log(q);
            //}
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);



            questionImage.enabled = true;
            questionImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

            StartCoroutine(StartQuiz());
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

    int num;
    public void OpenquestionPanel()
    {
        ResetState();
        // Type quizType = quizManager.quizType[questionCategory];
        //  Button btn= item.rightTxt.gameObject.transform.parent.GetComponent<Button>();
         num= DataBase.GetQuiz(quizManager.type);

        Debug.Log(num + " " + DataBase.GetQuiz(quizManager.type));

        StartCoroutine(LoadingWait());

        if (quizCount > DataBase.QuestionsToTreasure)
        {
            quizCount = 0;
            //num = DataBase.GetQuiz(quizManager.type);
            Debug.Log("Level Cleared treasure Obtained");
            num = 0;
        }
        else
        {
            Debug.Log(quizManager.quizType.Length);

            if (num < quizManager.quizType.Length)
            {
                if (quizManager.quizType[quizManager.type].quizData[num].IsImage)
                {
                    // item.questionImage.gameObject.SetActive(true);
                    StartCoroutine(LoadImage(quizManager.quizType[quizManager.type].quizData[num].imageLink));
                    //.enabled = true;
                }
                else
                {
                    questionImage.enabled = false;
                }
                questionTxt.text = quizManager.quizType[quizManager.type].quizData[num].question.ToString();
                correctTxt.text = quizManager.quizType[quizManager.type].quizData[num].correctAns.ToString();




            }
        }

    }

}
