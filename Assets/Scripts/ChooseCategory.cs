using DanielLochner.Assets.SimpleScrollSnap;

using UnityEngine;

public class ChooseCategory : MonoBehaviour
{
    int category1, category2;
    public SimpleScrollSnap snap1,snap2;
   public UiManager uiManager;
    // Start is called before the first frame update
    void Awake()
    {
        uiManager = FindAnyObjectByType<UiManager>();
    }

    // Update is called once per frame
    public void updateCategory(bool rightSide)
    {
        if(rightSide)
            category1 = snap1.CenteredPanel;
        else
            category2 = snap2.CenteredPanel;


    }

    public void StartQuiz()
    {
        uiManager.ChooseQuestionCategory(category1,category2);
        uiManager.OpenquestionPanel();
    }



}
