using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ViewTradeRequest : MonoBehaviour
{
    public TextMeshProUGUI userNameText;


    public Button viewButton;


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

        viewButton.onClick.AddListener(ViewRequest);
    }

    public void ViewRequest()
    {
        TradeUIManager.ViewTradeRequestData(userID);
        Debug.Log("I am Clicked! ");

    }



}
