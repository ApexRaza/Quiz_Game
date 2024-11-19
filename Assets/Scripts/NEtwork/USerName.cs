
using Photon.Pun;
using Photon.Realtime;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class USerName : MonoBehaviourPunCallbacks
{

   public TextMeshProUGUI p1_Txt, p2_Txt;
   public NetworkQuizHandler nqh;
    
    private void Awake()
    {
        nqh = FindAnyObjectByType<NetworkQuizHandler>();
    }
    private void OnEnable()
    {


        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.LocalPlayer.NickName = DataBase.UserName;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = DataBase.UserName;
        }

        GetComponent<PhotonView>().RPC("SEtName", RpcTarget.All);
        nqh.setQuizData();
        //SEtName();
    }


    [PunRPC]
    void SEtName()
    {

        p1_Txt.text = PhotonNetwork.PlayerList[0].NickName;
        p2_Txt.text = PhotonNetwork.PlayerList[1].NickName;

       


    }


}
