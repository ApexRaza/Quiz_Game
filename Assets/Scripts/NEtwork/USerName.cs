
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class USerName : MonoBehaviourPunCallbacks
{

   public TextMeshProUGUI p1_Txt,p1_Txt1,p2_Txt2, p2_Txt, p1_coin, p2_coin;
   public NetworkQuizHandler nqh;
    public int coin1, coin2;

    private void Awake()
    {
        nqh = FindAnyObjectByType<NetworkQuizHandler>();
    }
    private void OnEnable()
    {

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.LocalPlayer.NickName = DataBase.UserName;
           // UpdateCoins(DataBase.Dollars);
            GetComponent<PhotonView>().RPC(nameof(setCoin), RpcTarget.All, 1, DataBase.Dollars);
            nqh. setQuizData();
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = DataBase.UserName;
          //  UpdateCoins2(DataBase.Dollars);
            GetComponent<PhotonView>().RPC(nameof(setCoin), RpcTarget.All, 2, DataBase.Dollars);
        }

        GetComponent<PhotonView>().RPC("SetName", RpcTarget.All);
      
        //SEtName();
    }



    public void UpdateCoins(int coins)
    {
        Hashtable coinsP1 = new Hashtable();
        coinsP1["P1Coins"] = coins;
        PhotonNetwork.CurrentRoom.SetCustomProperties(coinsP1);
        Debug.Log("P1 " + coins);
    }

    public void UpdateCoins2(int coins)
    {
        Hashtable coinsP2 = new Hashtable();
        coinsP2["P2Coins"] = coins;
        PhotonNetwork.CurrentRoom.SetCustomProperties(coinsP2);
        Debug.Log("P2 " + coins);
    }



    [PunRPC]
    void SetName()
    {

        p1_Txt.text = PhotonNetwork.PlayerList[0].NickName;
        p2_Txt.text = PhotonNetwork.PlayerList[1].NickName;
        p1_Txt1.text = PhotonNetwork.PlayerList[0].NickName;
        p2_Txt2.text = PhotonNetwork.PlayerList[1].NickName;

        

        p1_coin.text = coin1.ToString();
        p2_coin.text = coin2.ToString();
    }

    [PunRPC]
    void setCoin(int num, int value)
    {
        switch (num)
        {
            case 1:
                coin1 = value; break;
            case 2:
                coin2 = value; break;
        }
        
    }

}
