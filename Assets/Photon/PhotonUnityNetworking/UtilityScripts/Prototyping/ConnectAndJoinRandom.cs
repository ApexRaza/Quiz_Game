// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectAndJoinRandom.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities, 
// </copyright>
// <summary>
//  Simple component to call ConnectUsingSettings and to get into a PUN room easily.
// </summary>
// <remarks>
//  A custom inspector provides a button to connect in PlayMode, should AutoConnect be false.
//  </remarks>                                                                                               
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;

namespace Photon.Pun.UtilityScripts
{

    /// <summary>Simple component to call ConnectUsingSettings and to get into a PUN room easily.</summary>
    /// <remarks>A custom inspector provides a button to connect in PlayMode, should AutoConnect be false.</remarks>
    public class ConnectAndJoinRandom : MonoBehaviourPunCallbacks
    {
        /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
        public bool AutoConnect = true;

        /// <summary>Used as PhotonNetwork.GameVersion.</summary>
        public byte Version = 1;

        /// <summary>Max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.</summary>
        [Tooltip("The max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.")]
        public byte MaxPlayers = 4;
        //  public GameObject GamePlay;
        public GameObject twoPlayers, onePlayers;

        public GameObject USerObj,questionPanel, loading, glassObj,vsObj;
        [HideInInspector]
        public string Roomname;
        public int playerTTL = -1;
        public int mode;
        public int Battleplayers;
        public bool isJoinedbyID;

        public void Start()
        {
            if (this.AutoConnect)
            {
                this.ConnectNow();
            }
        }

        public void ConnectNow()
        {
           // Debug.Log("ConnectAndJoinRandom.ConnectNow() will now call: PhotonNetwork.ConnectUsingSettings().");


            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;

        }


        // below, we implement some callbacks of the Photon Realtime API.
        // Being a MonoBehaviourPunCallbacks means, we can override the few methods which are needed here.


        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" + PhotonNetwork.CloudRegion +
                "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");

        }
        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion + "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
            print("OK");
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }

        /// ================================= Custom Create Room =========================

        public void CreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = Battleplayers, // Set the maximum number of players for the room
                CustomRoomProperties = new Hashtable
                {
                    { "GameMode", mode }, // Set the game mode as a custom property
                    { "NumberOfPlayers", Battleplayers }, // Set the number of players as a custom property
                },
                CustomRoomPropertiesForLobby = new string[] { "GameMode", "NumberOfPlayers" }, // Make the custom properties visible in the lobby
            };
            if (playerTTL >= 0)
                roomOptions.PlayerTtl = playerTTL;
            PhotonNetwork.CreateRoom(Roomname, roomOptions, null);
        }

        public void JoinRoombyID(string ID)
        {
            PhotonNetwork.JoinRoom(ID);
            
        }


        /// ================================= Create Random Room ==========================
        //=================================================================================
        //=================================================================================


        public void JoinRoom()
        {
            Hashtable expectedCustomRoomProperties = new Hashtable();
            expectedCustomRoomProperties.Add("Players", Battleplayers);
          
            //PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);

            PhotonNetwork.JoinRandomRoom();
        }
        void CreateRoom(int MaxPlayers)
        {
            // Define room options with custom properties.
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = (byte)Battleplayers;
            roomOptions.CustomRoomProperties = new Hashtable() { { "Players", MaxPlayers },{ "GameType",mode } };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "Players", "GameType" };

            // Create a room with the specified room options.
            PhotonNetwork.CreateRoom(null, roomOptions);
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // Get the selected mode from your UI or logic (e.g., "Addition" or "Subtraction").

            CreateRoom(Battleplayers);

        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom(); 
        }


        public override void OnJoinedRoom()
        {
            if (isJoinedbyID)
            {
                Hashtable customRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

                // Check if the custom properties exist and retrieve their values
                if (customRoomProperties.ContainsKey("GameMode"))
                {
                  int  gameMode = (int)customRoomProperties["GameMode"];
                    mode = gameMode;
                   
                }

                if (customRoomProperties.ContainsKey("NumberOfPlayers"))
                {
                    int numberOfPlayers = (int)customRoomProperties["NumberOfPlayers"];
                    Battleplayers = numberOfPlayers;

                }
                
                GetComponent<PhotonView>().RPC("StartGame", RpcTarget.All, true);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == Battleplayers)
            {
                Debug.Log(PhotonNetwork.LocalPlayer.NickName +"_________" + PhotonNetwork.PlayerList[1].ActorNumber);
                if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
                {
                    PhotonNetwork.LocalPlayer.NickName =    "Player 1";
                }
                else
                {
                    PhotonNetwork.LocalPlayer.NickName = "Player 2";
                }


                GetComponent<PhotonView>().RPC("StartGame", RpcTarget.All, true);
            }
        }
        [PunRPC]
        void StartGame(bool start)
        {


            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                onePlayers.SetActive(true);  
            }
            else
            {
               twoPlayers.SetActive(true);
            }

            glassObj.SetActive(false);
            vsObj.SetActive(true);
            USerObj.SetActive(true);
            StartCoroutine(delay());
            Debug.Log("GameStarted ___" + PhotonNetwork.LocalPlayer.NickName);
            //FindObjectOfType<GameManager>().StartGame();
             
        }


        IEnumerator delay()
        {
            yield return new WaitForSeconds(1f);
            loading.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            questionPanel.SetActive(true);
            
        }
    }
}
