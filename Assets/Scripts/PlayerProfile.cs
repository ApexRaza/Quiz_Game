using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerProfile : MonoBehaviour
{
    public TextMeshProUGUI  questionAnswered, rightAnswer, wrongAnswer, winningPercentage,badgeName;
    public Image badge, badge1;
    public Sprite[] badgeicons;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
      
        questionAnswered.text = DataBase.Questions.ToString();
        rightAnswer.text = DataBase.RightAnswer.ToString();
        wrongAnswer.text = DataBase.WrongAnswer.ToString();
        winningPercentage.text = (CalculatePercentage(DataBase.RightAnswer, DataBase.Questions).ToString() + " %");
        badgeName.text = DataBase.GradeName.ToString();
        UpdateBadge();  
    }

  public  void UpdateBadge()
    {
        for (int i = 1; i < 42; i++)
        {
            if (i == DataBase.LevelUp)
            {
                badge.sprite = badgeicons[i-1];
                badge1.sprite = badgeicons[i - 1];
            }
        }
    }
    void BadgeName()
    {
        
    }


    public float CalculatePercentage(int value, float total)
    {
        float result = (value / total) * 100f;
        return Mathf.Round(result * 100f) / 100f;
    }
}
