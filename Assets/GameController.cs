using UnityEngine;
using System.Collections;

public class GameController : Singleton<GameController> {

    public enum State
    {
        Menu,
        Network,
        Begin,
        Ice,
        Attack,
        Transition,
        Next,
        End
    }

    public State state;
    public int currentPlayer;

    Grid grid;
    UIController uiController;
    EventManager eventManager;
    NetworkController networkController;

    MainMenuManager mainMenuManager;
    NetworkMenuManager networkMenuManager;

    void Awake()
    {
        grid = Grid.Instance;
        uiController = UIController.Instance;
        eventManager = EventManager.Instance;
        networkController = NetworkController.Instance;

        mainMenuManager = MainMenuManager.Instance;
        networkMenuManager = NetworkMenuManager.Instance;

    }

    public void endGame(int player)
    {
        state = State.End;
        uiController.endGame(player);
    }

	// Use this for initialization
	void Start () {
        gotoState(State.Menu);
        //uiController.showStartMenu();
	}

    public void gotoState(State _state)
    {
        switch(_state)
        {
            case State.Network:
                networkController.Init();
                networkController.Scan();
                networkMenuManager.enable();
                mainMenuManager.disable();
                state = State.Network;
                break;
            case State.Menu:
                mainMenuManager.enable();
                networkMenuManager.disable();
                state = State.Menu;
                break;
            case State.Ice:
                state = State.Ice;
                break;
            case State.Begin:
                state = State.Begin;
                break;
            case State.Transition:
                state = State.Transition;
                break;
            case State.Next:
                state = State.Next;
                break;
            case State.End:
                state = State.End;
                break;
            case State.Attack:
                state = State.Attack;
                break;
        }
    }

    void startGame()
    {
        grid.InitializeGrid();
        currentPlayer = 0;
        startRound();
    }

	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case State.Attack:
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        Square square = hit.collider.gameObject.GetComponent<Square>();
                        if (square.type.Equals(Square.Type.Other))
                        {
                            square.reveal(currentPlayer);
                            int otherPlayer = (currentPlayer + 1) % 2;
                            square.hitEmpty(otherPlayer);
                            eventManager.addEvent(() => square.reveal(currentPlayer), 1, true);
                            if (!state.Equals(State.End))
                            {
                                state = State.Next;
                            }
                        }
                    }
                }
                break;
            case State.Ice:
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        Square square = hit.collider.gameObject.GetComponent<Square>();
                        if (square.type.Equals(Square.Type.Player))
                        {
                            square.hitIce(currentPlayer);
                            if (!state.Equals(State.End))
                            {
                                state = State.Attack;
                            }
                        }
                    }
                }
                break;
            case State.Menu:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    uiController.hideUI();
                    startGame();
                }
                break;
            case State.Next:
                startNextRound();
                break;
            case State.Transition:
                break;
            case State.End:
                break;

        }
	}

    void startRound()
    {
        state = State.Transition;
        grid.playerGridsObjects[currentPlayer].SetActive(true);
        int otherPlayer = (currentPlayer + 1) % 2;
        grid.playerGridsObjects[otherPlayer].SetActive(false);
        grid.deactivate(otherPlayer);
        state = State.Ice;
        uiController.countdown(currentPlayer + 1);

    }

    void startNextRound()
    {
        state = State.Transition;
        eventManager.addEvent(nextRound, 4, true);
        int nextPlayer = (currentPlayer + 1) % 2;
        eventManager.addEvent(() => uiController.countdown(nextPlayer + 1), 2, true);
    }

    void refreshGrid()
    {
        grid.refreshOther(currentPlayer);
    }

    void nextRound()
    {
        grid.deactivate(currentPlayer);
        grid.playerGridsObjects[currentPlayer].SetActive(false);
        int otherPlayer = (currentPlayer + 1) % 2;
        grid.playerGridsObjects[otherPlayer].SetActive(true);
        currentPlayer = otherPlayer;
        state = State.Ice;
    }

}
