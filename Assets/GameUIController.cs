using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIController : Singleton<GameUIController> {
    
    public GameObject win;
    public GameObject lose;
    public GameObject replay;
    public GameObject waitScreen;
    public Text notificationText;

    EventManager eventManager;
    GameController gameController;

    public enum State
    {
        Win,
        Lose,
        Inactive,
        Waiting
    }

    public State state;

    public void showWin()
    {
        setState(State.Win);
    }

    public void showLose()
    {
        setState(State.Lose);
    }

    public void setState(State _state)
    {
        win.SetActive(false);
        lose.SetActive(false);
        replay.SetActive(false);
        waitScreen.SetActive(false);
        notificationText.gameObject.SetActive(false);
        state = _state;
        switch(state)
        {
            case State.Lose:
                lose.SetActive(true);
                eventManager.addEvent(() => replay.SetActive(true), 1, true);
                break;
            case State.Win:
                win.SetActive(true);
                eventManager.addEvent(() => replay.SetActive(true), 1, true);
                break;
            case State.Waiting:
                waitScreen.SetActive(true);
                break;
            case State.Inactive:
                break;

        }
    }
    
    public void Notify(string message)
    {
        notificationText.gameObject.SetActive(true);
        notificationText.text = message;
    }

    public void EndNotify()
    {
        notificationText.gameObject.SetActive(false);
    }

    void Awake()
    {
        eventManager = EventManager.Instance;
        gameController = GameController.Instance;
        
        win.SetActive(false);
        lose.SetActive(false);
        replay.SetActive(false);
        waitScreen.SetActive(false);
        notificationText.gameObject.SetActive(false);
    }

    public void hideAll()
    {
        setState(State.Inactive);
    }

	// Use this for initialization
	void Start () {
        
	}
    
	// Update is called once per frame
	void Update () {
	
	}
}
