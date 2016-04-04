using UnityEngine;
using System.Collections;

public class NetworkController : PUNSingleton<NetworkController>
{
    GameController gameController;
    NetworkMenuManager networkMenuManager;

    bool connected;
    bool isHost;

    public PhotonPlayer otherPlayer;

    public RoomInfo[] rooms;

    public override void OnJoinedLobby()
    {
        networkMenuManager.setState(NetworkMenuManager.State.Active);
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        networkMenuManager.setState(NetworkMenuManager.State.Inactive);
        Debug.Log("Joined Room: " + PhotonNetwork.room.name);
        gameController.readyToStart();
        if (PhotonNetwork.room.playerCount == 2)
        {
            otherPlayer = PhotonNetwork.otherPlayers[0];
            gameController.ready();
            gameController.photonView.RPC("ready", otherPlayer);
            Debug.Log("Broadcasted Ready");
        }
    }
    
    public void Init()
    {
        networkMenuManager.setState(NetworkMenuManager.State.Connecting);
        PhotonNetwork.ConnectUsingSettings("0.3");
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
        networkMenuManager = NetworkMenuManager.Instance;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
