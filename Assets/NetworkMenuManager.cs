using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkMenuManager : Singleton<NetworkMenuManager> {

    NetworkController networkController;
    public Canvas canvas;
    public GameObject connecting;

    public RectTransform roomCanvas;
    public GameObject roomObject;

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
    
    public void join(string text)
    {
        Debug.Log("Joining: " + text);
        networkController.Join(text);
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
    }
    
    public void join(int i)
    {

    }

    public void host()
    {
        networkController.Host();
    }

    public void updateRoomList()
    {
        int height = 100;
        int width = 480;
        int space = 30;
        int viewportWidth = 720;
        foreach (Transform child in roomCanvas.transform)
        {
            Destroy(child.gameObject);
        }
        int totalRooms = networkController.rooms.Length;
        Debug.Log("Total Rooms: " + totalRooms);
        for (int i = 0; i < totalRooms; i++)
        {
            GameObject sObj = Object.Instantiate(roomObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            sObj.transform.parent = roomCanvas.transform;
            RectTransform rect = sObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(width, height);
            rect.localPosition = new Vector2(viewportWidth/2, -((space + height) * i + space + height / 2));
            sObj.GetComponentInChildren<Text>().text = networkController.rooms[i].name;
        }
        roomCanvas.sizeDelta = new Vector2(0, (space + height) * totalRooms + space);
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
