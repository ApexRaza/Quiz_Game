
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
    
    [Header("Question Panel Area")]
    [Space(2)]
    public GameObject QuestionPanel,correctAns, incorrectAns, ansObjects;
    public TextMeshProUGUI questionTxt;
    public Image questionImage;



    public List<GameObject> contentItem = new List<GameObject>();

    public ProgressState progressState;
    string[] game_ticket_id;
    int quizCount = 0;



    // Start is called before the first frame update
    void Start()
    {




        collectionSO = Resources.Load<CollectionsSO>("Scriptables/Collection");
        quizManager = Resources.Load<QuizManager>("Scriptables/QuizManager");
        quizManager.QuizTypesInit();


        quizManager.SetQuizType(QuizType.Varia);

  
    }

    int cat1, cat2;
    // setting the two different categories selected by the user
    public void SetQuestionCategories(int category1, int category2)
    {
        cat1 = category1;
        cat2 = category2;

        ChooseQuestionCategory();

    }

    //randomly choosing between the two selected categories
    public void ChooseQuestionCategory()
    {

        int i = Random.Range(0, 10);
        if (i > 5)
            quizManager.type = cat1;
        else
            quizManager.type = cat2;
    }




    public IEnumerator Next()
    {
        Debug.Log("run");

        correctAns.SetActive(true);
        ansObjects.SetActive(false);
        yield return new WaitForSeconds(1);

        progressState.gameObject.SetActive(true);
        progressState.UpdateState(true);
        DataBase.Keys += 20;

        int i = DataBase.GetQuiz(quizManager.type);
        i++;
        DataBase.SetQuiz(quizManager.type, i);

        questionImage.enabled = false;
        quizCount++;

        ChooseQuestionCategory();
        if (quizCount > DataBase.QuestionsToTreasure)
        {
            quizCount = 0;

            Debug.Log("Level Cleared treasure Obtained");

        }
        else
            OpenquestionPanel();
    }



    public IEnumerator WrongAns()
    {
       
        yield return new WaitForSeconds(1);

        progressState.gameObject.SetActive(true);
        progressState.UpdateState(false);
     
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


        }
    }

    

    public void OpenquestionPanel()
    {
        // Type quizType = quizManager.quizType[questionCategory];
        //  Button btn= item.rightTxt.gameObject.transform.parent.GetComponent<Button>();
        int num = DataBase.GetQuiz(quizManager.type);

        Debug.Log(num + " " + DataBase.GetQuiz(quizManager.type));

        QuestionPanel.SetActive(true);
        if (quizCount > DataBase.QuestionsToTreasure)
        {
            quizCount = 0;
            num = DataBase.GetQuiz(quizManager.type);
            Debug.Log("Level Cleared treasure Obtained");
            TreasureSystem.Instance.CalculatePercentage(TreasureType.Low);
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
                //int i = Random.Range(0, 2);
                //if (i < 1)
                //{
                //    Vector3 pos = item.wrongBtn.transform.position;
                //    item.wrongBtn.transform.position = item.rightBtn.transform.position;
                //    item.rightBtn.transform.position = pos;
                //}
                //item.rightTxt.text = quizManager.quizType[quizManager.type].quizData[num].rightAnswer.ToString();
                //item.wrongTxt.text = quizManager.quizType[quizManager.type].quizData[num].wrongAnswer.ToString();


            }
        }

    }

}
