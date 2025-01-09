using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChallengeItem : MonoBehaviour
{
    public TextMeshProUGUI userNameText;
    public Image onlineStatusImage;
    public string id;
    public Button challengeButton;
    ChallengeFrnd challengeFrnd;

    public void Initialize(string userName, ChallengeFrnd cf,string fid, bool isOnline)
    {
        userNameText.text = userName;
        challengeFrnd = cf;
        id = fid;
        onlineStatusImage.color = isOnline ? Color.green : Color.red;
        challengeButton.onClick.AddListener(ChallengeButton);
    }




    public void ChallengeButton()
    {
        challengeFrnd.ChallengeFriend(id);
    }
}