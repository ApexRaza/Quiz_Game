using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(menuName ="QuizGame/QuizManager", fileName ="QuizManager")]
public class QuizManager : ScriptableObject
{

    public Quiz quiz;

    public QuizData[] quizData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RightAnswerGiven(Quiz quizType)
    {
        switch (quizType)
        {
            case Quiz.Animals:
                {
                    DataBase.AnimalQuiz++;
                }
                break;
            case Quiz.Sports:
                {
                    DataBase.SportsQuiz++;
                }
                break;
            case Quiz.Vehicles:
                {
                    DataBase.VehicleQuiz++;
                }
                break;
            case Quiz.General:
                {
                    DataBase.GeneralQuiz++;
                }

                break;
        }
    }





}

[Serializable]
public class QuizData
{
   
    public string Name;
    public int ID;
    public Data[] Quiz;
}


[Serializable]
public class Data
{
    public string question;
    public string rightAnswer;
    public string wrongAnswer;
    public bool IsImage;
    public Sprite imageQuestion;
    public string tip;
}

public enum Quiz
{
    Sports,
    Animals,
    Vehicles,
    General

}

