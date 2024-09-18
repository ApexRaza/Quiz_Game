
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Web;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using static UnityEditor.Progress;

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
    


    // Start is called before the first frame update
    void Start()
    {
       
        


        collectionSO = Resources.Load<CollectionsSO>("Scriptables/Collection");
        quizManager = Resources.Load<QuizManager>("Scriptables/QuizManager");
        quizManager.QuizTypesInit();


        quizManager.SetQuizType(QuizType.Varia);

        UpdateCollectionPanel();
    }




    int cat1, cat2;
    public void ChooseQuestionCategory(int category1, int category2)
    {
        cat1 = category1;
        cat2 = category2;
        int i = UnityEngine. Random.Range(0, 2);
        if (i == 0)
            quizManager.type = category1;
        else
            quizManager.type = category2;

        Debug.Log(quizManager.type);

    }
    public void ChooseQuestionCategory()
    {
        
        int i = UnityEngine.Random.Range(0, 10);
        if (i > 5)
            quizManager.type = cat1;
        else
            quizManager.type = cat2;
    }




    public void ChooseCollectionType(int i)
    {
        collectionType = i;
        UpdateCollectionPanel();
    }
    
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



            item.questionImage.enabled = true;
            item.questionImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));


        }
    }


    public void OpenquestionPanel()
    {
      // Type quizType = quizManager.quizType[questionCategory];
      //  Button btn= item.rightTxt.gameObject.transform.parent.GetComponent<Button>();
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
                   // item.questionImage.gameObject.SetActive(true);
                    StartCoroutine(LoadImage(quizManager.quizType[quizManager.type].quizData[num].imageLink));
                   //.enabled = true;
                }
                else
                {
                    item.questionImage.enabled = false;
                }
                item.questionTxt.text = quizManager.quizType[quizManager.type].quizData[num].question.ToString();
                int i = UnityEngine.Random.Range(0, 2);
                if (i < 1)
                {
                    Vector3 pos = item.wrongBtn.transform.position;
                    item.wrongBtn.transform.position = item.rightBtn.transform.position;
                    item.rightBtn.transform.position = pos;
                }
                //item.rightTxt.text = quizManager.quizType[quizManager.type].quizData[num].rightAnswer.ToString();
                //item.wrongTxt.text = quizManager.quizType[quizManager.type].quizData[num].wrongAnswer.ToString();

                
            }
        }

    }

    public IEnumerator Next()
    {
        Debug.Log("run");

        item.wrongBtn.GetComponent<Image>().color = Color.red;
        item.rightBtn.GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(1);

        progressState.gameObject.SetActive(true);
        progressState.UpdateState(true);
        item.wrongBtn.GetComponent<Image>().color = Color.white;
        item.rightBtn.GetComponent<Image>().color = Color.white;
        DataBase.Keys += 20;

        int i = DataBase.GetQuiz(quizManager.type);
        i++;
        DataBase.SetQuiz(quizManager.type, i);

        item.questionImage.enabled = false;
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
        item.wrongBtn.GetComponent<Image>().color = Color.red;
        item.rightBtn.GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(1);

        progressState.gameObject.SetActive(true);
        progressState.UpdateState(false);
        item.wrongBtn.GetComponent<Image>().color = Color.white;
        item.rightBtn.GetComponent<Image>().color = Color.white;
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

[System. Serializable]
public class Items
{
  

    [Header("Collection Panel Area")]
    [Space(2)]
    public GameObject CollectionPanel;
    public GameObject Content;


    [Header("Question Panel Area")]
    [Space(2)]
    public GameObject QuestionPanel,rightBtn,wrongBtn;
    public TextMeshProUGUI questionTxt;
    public Image questionImage;



}
