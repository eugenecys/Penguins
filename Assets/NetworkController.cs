using UnityEngine;
using System.Collections;

public class NetworkController : Singleton<NetworkController> {

    bool connected;
    bool isHost;

    public void Init()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public void Scan()
    {
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
