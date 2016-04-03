using UnityEngine;
using System.Collections;

public class NetworkController : PUNSingleton<NetworkController>
{
    GameController gameController;

    bool connected;
    bool isHost;

    public PhotonPlayer otherPlayer;

    public RoomInfo[] rooms;

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.room.name);
        gameController.readyToStart();
        if (PhotonNetwork.room.playerCount == 2)
        {
            otherPlayer = PhotonNetwork.otherPlayers[0];
            gameController.photonView.RPC("ready", otherPlayer);
        }
    }
    
    public void Init()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public void RandomJoin()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Scan()
    {
        rooms = PhotonNetwork.GetRoomList();
        isHost = false;
    }

    public void Join()
    {
        isHost = false;
    }

    public void Host()
    {
        isHost = true;
        PhotonNetwork.CreateRoom("Hello");
        Debug.Log("Created Room");
    }

    public void retrieveOtherPlayer()
    {
        if (PhotonNetwork.room.playerCount > 1)
        {
            otherPlayer = PhotonNetwork.otherPlayers[0];
        }
    }

    void Awake()
    {
        gameController = GameController.Instance;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
