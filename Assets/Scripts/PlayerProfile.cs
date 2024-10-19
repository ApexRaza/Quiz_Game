using TMPro;
using UnityEngine;


public class PlayerProfile : MonoBehaviour
{
    public TextMeshProUGUI  questionAnswered, rightAnswer, wrongAnswer, winningPercentage;
    // Start is called before the first frame update
    private void OnEnable()
    {
      
        questionAnswered.text = DataBase.Questions.ToString();
        rightAnswer.text = DataBase.RightAnswer.ToString();
        wrongAnswer.text = DataBase.WrongAnswer.ToString();
        winningPercentage.text = (CalculatePercentage(DataBase.RightAnswer, DataBase.Questions).ToString() + " %");
    }

    public float CalculatePercentage(int value, float total)
    {
        float result = (value / total) * 100f;
        return Mathf.Round(result * 100f) / 100f;
    }
}
