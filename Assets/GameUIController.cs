using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIController : Singleton<GameUIController> {
    
    public GameObject iceBG;
    public GameObject iceBGInactive;
    public GameObject icedirtBG;
    public GameObject icedirtBGInactive;
    public GameObject win;
    public GameObject lose;
    public GameObject replay;
    public Text notificationText;

    EventManager eventManager;
    GameController gameController;

    public enum State
    {
        Ice,
        IceInactive,
        Dirt,
        DirtInactive,
        Win,
        Lose,
        Inactive
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
        iceBG.SetActive(false);
        iceBGInactive.SetActive(false);
        icedirtBG.SetActive(false);
        icedirtBGInactive.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);
        replay.SetActive(false);
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
            case State.Dirt:
                icedirtBG.SetActive(true);
                break;
            case State.DirtInactive:
                icedirtBGInactive.SetActive(true);
                break;
            case State.Ice:
                iceBG.SetActive(true);
                break;
            case State.IceInactive:
                iceBGInactive.SetActive(true);
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

        iceBG.SetActive(false);
        iceBGInactive.SetActive(false);
        icedirtBG.SetActive(false);
        icedirtBGInactive.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);
        replay.SetActive(false);
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
