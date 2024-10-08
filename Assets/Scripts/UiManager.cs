
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Web;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;

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
    
   


   

   


  


    //public void OpenCollectionPanel(int num)
    //{
      

    //    item.CollectionPanel.SetActive(true);
    //    foreach (Transform t in item.Content.transform)
    //    {
    //        t.gameObject.SetActive(false);
    //        contentItem.Add(t.gameObject);
    //    }

      

    //    for (int i = 0; i < collectionSO.collectionData[collectionType].item.Length; i++)
    //    {
    //        Sprite icon = collectionSO.collectionData[collectionType].item[i].Icon;
    //        string s = collectionSO.collectionData[collectionType].item[i].collected.ToString() + " / " +
    //            collectionSO.collectionData[collectionType].item[i].total.ToString();



    //        contentItem[i].transform.GetChild(0).GetComponent<Image>().sprite = icon;
    //        contentItem[i].GetComponentInChildren<TextMeshProUGUI>().text = s;
    //        contentItem[i].SetActive(true);
    //    }
        

    //}


    public void UpdateCollectionPanel()
    {
        contentItem.Clear();
        

        
        foreach (Transform t in item.Content.transform)
        {
            t.gameObject.SetActive(false);
            contentItem.Add(t.gameObject);
        }

        int num = 0;
        for (int i = 0; i < collectionSO.collectionData.Length; i++)
        {
            for (int j = 0; j < 70; j++)
            {
               
                collectionSO.collectionData[i].item[j].collected = DataBase.GetCoins(num);
                if (num <= (DataBase.LevelUp * 5))
                {

                }

                num++;
            }
        }


        for (int i = 0; i < collectionSO.collectionData[collectionType].item.Length; i++)
        {
            Sprite icon = collectionSO.collectionData[collectionType].item[i].Icon;
            string s = collectionSO.collectionData[collectionType].item[i].collected.ToString() + " / " +
                collectionSO.collectionData[collectionType].item[i].total.ToString();
            string n = collectionSO.collectionData[collectionType].item[i].Name;



           contentItem[i].transform.GetChild(0).GetComponent<Image>().sprite = icon;
            contentItem[i].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = s;
            contentItem[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = n;
            contentItem[i].SetActive(true);


            if (collectionType == 0)
            {
                contentItem[i].transform.GetComponent<Image>().sprite = item.redBg;
                contentItem[i].transform.GetChild(2).GetComponent<Image>().sprite = item.redTxt;
            }
            if (collectionType == 1)
            {
                contentItem[i].transform.GetComponent<Image>().sprite = item.bluBg;
                contentItem[i].transform.GetChild(2).GetComponent<Image>().sprite = item.bluTxt;
            }
            if (collectionType == 2)
            {
                contentItem[i].transform.GetComponent<Image>().sprite = item.yellowBg;
                contentItem[i].transform.GetChild(2).GetComponent<Image>().sprite = item.yellowTxt;
            }


        }
        item.Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        item.CollectionPanel.SetActive(true);
    }




    public void CloseCollectionPanel()
    {
       
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

    public Sprite redBg, redTxt, bluBg, bluTxt, yellowBg, yellowTxt;



}
