using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    public Text userName, questionAnswered, rightAnswer, wrongAnswer, winningPercentage;
    // Start is called before the first frame update
    private void OnEnable()
    {
        userName.text = DataBase.UserName;
        questionAnswered.text = DataBase.Questions.ToString();
        rightAnswer.text = DataBase.RightAnswer.ToString();
        wrongAnswer.text = DataBase.WrongAnswer.ToString();
        winningPercentage.text = (CalculatePercentage(DataBase.RightAnswer, DataBase.Questions).ToString() + " %");
    }

    public float CalculatePercentage(int value, float percentage)
    {
        float result = (value / percentage) * 100f;
        return Mathf.Round(result * 100f) / 100f;
    }
}
