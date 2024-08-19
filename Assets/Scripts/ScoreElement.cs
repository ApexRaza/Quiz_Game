using TMPro;
using UnityEngine;

public class ScoreElement : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text xpText;
    // Start is called before the first frame update
    public void NewScoreElement(string _username, int _xp)
    {
        usernameText.text = _username;
        xpText.text = _xp.ToString();
    }
}
