using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    CollectionsSO collectionSO;
    QuizManager quizManager;
    //private Login login;


    // Start is called before the first frame update
    void Start()
    {
        collectionSO = Resources.Load<CollectionsSO>("Scriptables/Collection");
        quizManager = Resources.Load<QuizManager>("Scriptables/QuizManager");
        quizManager.QuizTypesInit();
    }
    public void SelectCategory(QuizType quizType)
    {
        quizManager.SetQuizType(quizType);
    }

    public void TestGoogleLogin() 
    {
        Login.Instance.AnomLinkGoogle();   
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
