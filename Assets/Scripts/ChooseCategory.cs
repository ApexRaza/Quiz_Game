using UnityEngine;

public class ChooseCategory : MonoBehaviour
{
 
   public UiManager uiManager;
    public QuizHandler quizHandler;
    public ScrollRectSelection s1,s2;

    // Start is called before the first frame update
    void Awake()
    {
        uiManager = FindAnyObjectByType<UiManager>();
    }

    // Update is called once per frame
    public void updateCategory(int val)
    {
       
       quizHandler.SetQuestionCategory(val);
    }


    public void ReSpin()
    {
        if (DataBase.Gems >= 20)
        {
            s1.StartSlotSpin();
            s2.StartSlotSpin();

            DataBase.Gems -= 20;
            DataSaver.Instance.SaveData();
        }
        else
        {
            Debug.Log("Dont have Gems");
        }
    }


    public void StartQuiz()
    {
      
        quizHandler.OpenquestionPanel();
    }



}
