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
        if (PhotonNetwork.room.playerCount == 2)
        {
            PhotonPlayer[] players = new PhotonPlayer[2];
            players[0] = PhotonNetwork.player;
            players[1] = PhotonNetwork.otherPlayers[0];
            otherPlayer = players[1];

            int startPlayer = Random.Range(0, 2);
            gameController.photonView.RPC("ready", players[startPlayer], true);
            gameController.photonView.RPC("ready", players[(startPlayer + 1) % 2], false);
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
