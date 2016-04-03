using UnityEngine;
using System.Collections;

public class NetworkMenuManager : Singleton<NetworkMenuManager> {

    NetworkController networkController;
    public Canvas canvas;

    public void enable()
    {
        canvas.gameObject.SetActive(true);
    }

    public void disable()
    {
        canvas.gameObject.SetActive(false);
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
