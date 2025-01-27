
using UnityEngine;
using TMPro;
using UnityEngine.UI;

         
public class TradeRequestItem : MonoBehaviour
{
    public TextMeshProUGUI userNameText;


    public Button tradeButton;
    

    public TradeUIManager TradeUIManager;
    public string userID;
    private string userName;

    private void OnEnable()
    {
        TradeUIManager = GetComponentInParent<TradeUIManager>();
    }



    public void Initialize(string senderID, string userName)
    {
        userID = senderID;
        userNameText.text = userName;
        this.userName = userName;
        tradeButton.onClick.AddListener(TradeRequest);
    }

    public void TradeRequest()
    {
        TradeUIManager.GetFriendData(userID);
        TradeUIManager.userName = userName;
        Debug.Log("I am Clicked! ");

    }


  

}
