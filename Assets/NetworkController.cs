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

    public RoomInfo currentRoom;

    public override void OnJoinedLobby()
    {
        networkMenuManager.setState(NetworkMenuManager.State.Active);
        Debug.Log("Joined Lobby");
        Scan();
    }

    public override void OnJoinedRoom()
    {
        networkMenuManager.setState(NetworkMenuManager.State.Inactive);
        currentRoom = PhotonNetwork.room;
        Debug.Log("Joined Room: " + currentRoom.name);
        gameController.readyToStart();
        if (PhotonNetwork.room.playerCount == 2)
        {
            otherPlayer = PhotonNetwork.otherPlayers[0];
            gameController.ready();
            gameController.photonView.RPC("ready", otherPlayer);
        }
    }


    public override void OnReceivedRoomListUpdate()
    {
        Debug.Log("Received updates");
        networkMenuManager.updateRoomList();
    }

    public override void OnLeftRoom()
    {
        updateState();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("Player dropped");
        gameController.dropGame();
    }

    public void Connect()
    {
        if (!PhotonNetwork.connected)
        {
            networkMenuManager.setState(NetworkMenuManager.State.Connecting);
            PhotonNetwork.ConnectUsingSettings("0.4");
        }
    }
    
    public void updateState()
    {
        if (PhotonNetwork.inRoom)
        {
            networkMenuManager.setState(NetworkMenuManager.State.Inactive);
        }
        else if (PhotonNetwork.insideLobby && !PhotonNetwork.inRoom)
        {
            networkMenuManager.setState(NetworkMenuManager.State.Active);
        }
        else if (PhotonNetwork.connected && !PhotonNetwork.insideLobby)
        {
            networkMenuManager.setState(NetworkMenuManager.State.Connecting);
            PhotonNetwork.JoinLobby();
        }
        else if (!PhotonNetwork.connected)
        {
            Connect();
        }
    }

    public void disconnect()
    {
        networkMenuManager.setState(NetworkMenuManager.State.Inactive);
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    public void quitRoom()
    {
        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public void RandomJoin()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Scan()
    {
        rooms = PhotonNetwork.GetRoomList();
        networkMenuManager.updateRoomList();
        isHost = false;
    }

    public void Join(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        isHost = false;
    }

    public void Host()
    {
        isHost = true;
        RoomOptions roomOptions = new RoomOptions()
        {
            maxPlayers = 2
        };
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
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
