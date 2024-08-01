using System;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        collectionSO = Resources.Load<CollectionsSO>("Scriptables/Collection");
        quizManager = Resources.Load<QuizManager>("Scriptables/QuizManager");
       UpdateCollectionPanel();
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

    public void OpenquestionPanel(int num)
    {
        QuizData quizData = quizManager.quizData[questionCategory];
        Button btn= item.rightTxt.gameObject.transform.parent.GetComponent<Button>();
      
        item.QuestionPanel.SetActive(true);
        if (num >= quizData.Quiz.Length)
        {
            nextValue = 0;
            num=nextValue;
        }


        //if (next)
        //{
        //    num += nextValue;
        //}
        if (num < quizData.Quiz.Length)
        {
            if (quizData.Quiz[num].IsImage)
            {
                item.questionImage.sprite = quizData.Quiz[num].imageQuestion;
                item.questionImage.enabled = true;
            }
            else
            {
                item.questionImage.enabled = false;
            }
            item.questionTxt.text = quizData.Quiz[num].question.ToString();
            int i = UnityEngine.Random.Range(0, 2);
            if (i < 1)
            {
                Vector3 pos = item.wrongTxt.transform.parent.position;
                item.wrongTxt.transform.parent.position = item.rightTxt.transform.parent.position;
                item.rightTxt.transform.parent.position = pos;
            }
            item.rightTxt.text = quizData.Quiz[num].rightAnswer.ToString();
            item.wrongTxt.text = quizData.Quiz[num].wrongAnswer.ToString();

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => Next());
        }
      

    }

    bool next;
    int nextValue = 0;
    public void Next()
    {
        next = true;
        nextValue++;
      
        OpenquestionPanel(nextValue);
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
