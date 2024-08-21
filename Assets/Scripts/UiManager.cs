using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    CollectionsSO collectionSO;
    QuizManager quizManager;
    public Items item;

    [Space (10)]
    public int collectionType;
    public int questionCategory;


    public List<GameObject> contentItem = new List<GameObject>();
    
    public ProgressState progressState;
     string[] game_ticket_id;
    int quizCount=0;
    void TEstURL()
    {

        string url = "https://ourgamedomain.com?ref[]=E10DD63DBF44A3A&ref[]=4B039523023C5D3"; //Application.absoluteURL ;
        
        Uri uri = new Uri(url);

        // Extract the query string
        string query = uri.Query;
        NameValueCollection queryParams = HttpUtility.ParseQueryString(query);

        // Extract the 'ref[]' parameters (Note: you might need to use "ref%5B%5D" for encoded brackets)
        string[] refValues = queryParams.GetValues("ref[]");

        if (refValues != null)
        {
            game_ticket_id = new string[refValues.Length];

            for (int i = 0; i < refValues.Length; i++)
            {
                Debug.Log($"ref[{i}] = {refValues[i]}");

                game_ticket_id[i] = refValues[i];
            }
        }
        else
        {
            Debug.Log("No 'ref[]' parameters found.");
        }

    }


    // Start is called before the first frame update
    void Start()
    {
       
        


        collectionSO = Resources.Load<CollectionsSO>("Scriptables/Collection");
        quizManager = Resources.Load<QuizManager>("Scriptables/QuizManager");
        quizManager.QuizTypesInit();


        quizManager.SetQuizType(QuizType.Varia);

        UpdateCollectionPanel();
    }


    public void SelectCategory(QuizType quizType)
    {
        quizManager.SetQuizType(quizType);
    }


    public void ChooseQuestionCategory(int i)
    {
        questionCategory = i;
    }

    public void ChooseCollectionType(int i)
    {
        collectionType = i;
        UpdateCollectionPanel();
    }

    public void OpenquestionPanel()
    {
      // Type quizType = quizManager.quizType[questionCategory];
        Button btn= item.rightTxt.gameObject.transform.parent.GetComponent<Button>();
        int num = DataBase.GetQuiz(quizManager.type);

        Debug.Log(num + " " + DataBase.GetQuiz(quizManager.type));

        item.QuestionPanel.SetActive(true);
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
                    // item.questionImage.sprite = quizData.Data[num].imageQuestion;
                    item.questionImage.gameObject.SetActive(true);//.enabled = true;
                }
                else
                {
                    item.questionImage.gameObject.SetActive(false);
                }
                item.questionTxt.text = quizManager.quizType[quizManager.type].quizData[num].question.ToString();
                int i = UnityEngine.Random.Range(0, 2);
                if (i < 1)
                {
                    Vector3 pos = item.wrongTxt.transform.parent.position;
                    item.wrongTxt.transform.parent.position = item.rightTxt.transform.parent.position;
                    item.rightTxt.transform.parent.position = pos;
                }
                item.rightTxt.text = quizManager.quizType[quizManager.type].quizData[num].rightAnswer.ToString();
                item.wrongTxt.text = quizManager.quizType[quizManager.type].quizData[num].wrongAnswer.ToString();

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => Next());
            }
        }

    }

    public void Next()
    {
        Debug.Log("run");


        progressState.gameObject.SetActive(true);
        progressState.UpdateState();

        DataBase.Keys += 20;

        int i = DataBase.GetQuiz(quizManager.type);
        i++;
        DataBase.SetQuiz(quizManager.type, i);


        quizCount++;
        if (quizCount > DataBase.QuestionsToTreasure)
        {
            quizCount = 0;

            Debug.Log("Level Cleared treasure Obtained");
            
        }
        else
            OpenquestionPanel();
    }


    public void OpenCollectionPanel(int num)
    {
      

        item.CollectionPanel.SetActive(true);
        foreach (Transform t in item.Content.transform)
        {
            t.gameObject.SetActive(false);
            contentItem.Add(t.gameObject);
        }

      

        for (int i = 0; i < collectionSO.collectionData[collectionType].item.Length; i++)
        {
            Sprite icon = collectionSO.collectionData[collectionType].item[i].Icon;
            string s = collectionSO.collectionData[collectionType].item[i].collected.ToString() + " / " +
                collectionSO.collectionData[collectionType].item[i].total.ToString();



            contentItem[i].transform.GetChild(0).GetComponent<Image>().sprite = icon;
            contentItem[i].GetComponentInChildren<TextMeshProUGUI>().text = s;
            contentItem[i].SetActive(true);
        }
        

    }


    public void UpdateCollectionPanel()
    {
        contentItem.Clear();

       
        foreach (Transform t in item.Content.transform)
        {
            t.gameObject.SetActive(false);
            contentItem.Add(t.gameObject);
        }



        for (int i = 0; i < collectionSO.collectionData[collectionType].item.Length; i++)
        {
            Sprite icon = collectionSO.collectionData[collectionType].item[i].Icon;
            string s = collectionSO.collectionData[collectionType].item[i].collected.ToString() + " / " +
                collectionSO.collectionData[collectionType].item[i].total.ToString();



            contentItem[i].transform.GetChild(0).GetComponent<Image>().sprite = icon;
            contentItem[i].GetComponentInChildren<TextMeshProUGUI>().text = s;
            contentItem[i].SetActive(true);
        }


    }




    public void CloseCollectionPanel()
    {
        item.QuestionPanel.SetActive(false);
        item.CollectionPanel.SetActive (false);
        contentItem.Clear();
    }
}

[Serializable]
public class Items
{
  

    [Header("Collection Panel Area")]
    [Space(2)]
    public GameObject CollectionPanel;
    public GameObject Content;


    [Header("Question Panel Area")]
    [Space(2)]
    public GameObject QuestionPanel;
    public TextMeshProUGUI questionTxt,rightTxt,wrongTxt;
    public Image questionImage;



}
