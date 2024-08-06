using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(menuName ="QuizGame/QuizManager", fileName ="QuizManager")]
public class QuizManager : ScriptableObject
{

    public QuizType quiz;

    public Type[] quizType;

    public int type;

    public int[] gridId = new int[16];

    // Start is called before the first frame update
    void Start()
    {
        type = 0;
        QuizTypesInit();
    }

    public int SetQuizType(QuizType quiz)
    {
        int num = 0;
        num = quiz switch
        {
            QuizType.Varia => num =0,
            QuizType.Arts => num = 1,
            QuizType.Celebrities => num = 2,
            QuizType.Dictionary => num = 3,
            QuizType.Geography => num = 4,
            QuizType.Movies => num = 5,
            QuizType.Music10s => num = 6,
            QuizType.Music00s => num = 7,
            QuizType.Music90s => num = 8,
            QuizType.Music80s => num = 9,
            QuizType.Music70s => num = 10,
            QuizType.Nature => num = 11,
            QuizType.Spelling => num = 12,
            QuizType.Sports => num = 13,
            QuizType.Television => num = 14,
            QuizType.History => num = 15,
            _ => num = 0,
        };



        return type = num;

    }


    public void QuizTypesInit()
    {
        for (int i = 0; i < Enum.GetValues(typeof(QuizType)).Length; i++)
        {
            quizType[i].Name = Enum.GetNames(typeof(QuizType))[i].ToString();
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }


    //public void RightAnswerGiven(QuizType quizType)
    //{
    //    switch (quizType)
    //    {
    //        case QuizType.Animals:
    //            {
    //                DataBase.AnimalQuiz++;
    //            }
    //            break;
    //        case QuizType.Sports:
    //            {
    //                DataBase.SportsQuiz++;
    //            }
    //            break;
    //        case QuizType.Vehicles:
    //            {
    //                DataBase.VehicleQuiz++;
    //            }
    //            break;
    //        case QuizType.General:
    //            {
    //                DataBase.GeneralQuiz++;
    //            }

    //            break;
    //    }
    //}





}





[Serializable]
public class Type
{
   
    public string Name;
    public int ID;
    public List<Data> quizData;
}


[Serializable]
public class Data
{
    public string question;
    public string rightAnswer;
    public string wrongAnswer;
    public bool IsImage;
    public string imageLink;
    public string tip;
}

public enum QuizType
{
    Varia,
    Arts,
    Celebrities,
    Dictionary,
    Geography,
    Movies,
    Music10s,
    Music00s,
    Music90s,
    Music80s,
    Music70s,
    Nature,
    Spelling,
    Sports,
    Television,
    History

}

