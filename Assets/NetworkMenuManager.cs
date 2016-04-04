using UnityEngine;
using System.Collections;

public class NetworkMenuManager : Singleton<NetworkMenuManager> {

    NetworkController networkController;
    public Canvas canvas;
    public GameObject connecting;

    public enum State
    {
        Connecting,
        Active,
        Inactive
    }

    public State state;
    
    public void setState(State _state)
    {
        state = _state;
        switch(state)
        {
            case State.Connecting:
                connecting.SetActive(true);
                canvas.gameObject.SetActive(false);
                break;
            case State.Active:
                canvas.gameObject.SetActive(true);
                connecting.SetActive(false);
                break;
            case State.Inactive:
                canvas.gameObject.SetActive(false);
                connecting.SetActive(false);
                break;
        }
    }
    
    public void randomJoin()
    {
        networkController.RandomJoin();
    }

    public void refresh()
    {
        scan();
    }

    public void scan()
    {
        networkController.Scan();
        foreach (RoomInfo game in networkController.rooms)
        {
            Debug.Log(game.name + " " + game.playerCount + "/" + game.maxPlayers);
        }
    }
    
    public void join(int i)
    {

    }

    public void host()
    {
        networkController.Host();
    }

    void Awake()
    {
        networkController = NetworkController.Instance;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
