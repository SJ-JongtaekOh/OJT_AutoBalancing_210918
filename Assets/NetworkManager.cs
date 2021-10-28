using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager :MonoBehaviourPunCallbacks 
{
    public InputField NickNameInput;
    public GameObject DisconnectPanel;
    public GameObject RespawnPanel;
   
    private void Awake()
    {
        Screen.SetResolution(1440, 2960, true);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30; 

    }


    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room" , new RoomOptions { MaxPlayers = 2}, null);
    }

    public override void OnJoinedRoom()
    {
        DisconnectPanel.SetActive(false);
        Spawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectPanel.SetActive(true );
        RespawnPanel.SetActive(false);
    }

    public void Spawn()
    {
        //PhotonNetwork.Instantiate()
        RespawnPanel.SetActive(false);  
         
    }

}
