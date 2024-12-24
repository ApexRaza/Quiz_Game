
using UnityEngine;
using TMPro;
using UnityEngine.UI;

         
public class TradeRequestItem : MonoBehaviour
{
    public TextMeshProUGUI userNameText;


    public Button tradeButton;
    

    public TradeUIManager TradeUIManager;
    public string userID;
    private string userUsername;

    private void OnEnable()
    {
        TradeUIManager = GetComponentInParent<TradeUIManager>();
    }



    public void Initialize(string senderID, string userName)
    {
        userID = senderID;
        userNameText.text = userName;

        tradeButton.onClick.AddListener(TradeRequest);
    }

    public void TradeRequest()
    {
        TradeUIManager.GetFriendData(userID);
        Debug.Log("I am Clicked! ");

    }


  

}
