using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class lobbymanager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    public Text connectionInfoText;
    public Button JoinButton;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    public void OnJoinButton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        JoinButton.interactable = false;
        connectionInfoText.text = "Connect Master Server...";

        if(PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Connecting To Random Room";

        }

    }

    public override void OnConnectedToMaster()
    {
        JoinButton.interactable = true;
        connectionInfoText.text = "Online : Connected To Master Server...";

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        JoinButton.interactable = false;
        connectionInfoText.text = $"Offline : Connection Disabled {cause.ToString()}";

        PhotonNetwork.ConnectUsingSettings();

    }

}
