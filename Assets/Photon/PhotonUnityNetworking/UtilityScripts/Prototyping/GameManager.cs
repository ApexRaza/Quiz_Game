using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using Photon.Pun;

public enum GameMode{ 

    Addition,
    Subtraction,
    Multiplication,
    Division

}
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public GameMode GameMode;
    public GameObject[] AllPlayersUI;
    public Sprite[] Modes;
    public Image ModeImage;
   


    void Start()
    {
        Instance = this;
        
    }
    public void StartGame()
    {
        for (int i = 0; i < FindObjectOfType<ConnectAndJoinRandom>().Battleplayers; i++)
        {
            AllPlayersUI[i].SetActive(true);
        }
        ModeImage.sprite = Modes[FindObjectOfType<ConnectAndJoinRandom>().mode];
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            StartCoroutine(CheckifPlayerLeft());
    }
    public GameObject GameplayPanel;
    public GameObject logo;
    public GameObject ModePanel;
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public GameObject WinnerPanel;
    public IEnumerator OnPlayerLeftGame()
    {
        WinnerPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        LeaveRoom();
    }
    public IEnumerator CheckifPlayerLeft()
    {
        yield return new WaitForSeconds(2);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            StartCoroutine(OnPlayerLeftGame());
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CheckifPlayerLeft());
    }
    public IEnumerator OnPlayerWinorLoseGame()
    {
       
        yield return new WaitForSeconds(4f);
        LeaveRoom();
    }
}