using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class GameController : Singleton<GameController> {

    RuntimePlatform platform = Application.platform;

    public enum State
    {
        Menu,
        Network,
        Begin,
        Ice,
        Attack,
        OtherIce,
        OtherAttack,
        Pause,
        End,
        Wait
    }

    public State state;
    public int currentPlayer;

    public Grid grid;
    public PhotonView photonView;

    GameUIController gameUIController;
    EventManager eventManager;
    NetworkController networkController;

    MainMenuManager mainMenuManager;
    NetworkMenuManager networkMenuManager;

    private bool playerIce;
    private bool playerAttack;
    private bool otherIce;
    private bool otherAttack;
    private bool playerReady;
    private bool otherReady;

    public GameObject waitScreen;

    public void readyToStart()
    {
        gotoState(State.Begin);
    }

    [PunRPC]
    public void ready()
    {
        otherReady = true;
    }
    
    void Awake()
    {
        grid = Grid.Instance;
        gameUIController = GameUIController.Instance;
        eventManager = EventManager.Instance;
        networkController = NetworkController.Instance;

        mainMenuManager = MainMenuManager.Instance;
        networkMenuManager = NetworkMenuManager.Instance;

        waitScreen.SetActive(false);
    }

    // Use this for initialization
    void Start() {
        gotoState(State.Menu);
        PhotonNetwork.autoJoinLobby = true;

        playerIce = false;
        playerAttack = false;
        otherIce = false;
        otherAttack = false;

        //uiController.showStartMenu();
    }

    public void back()
    {
        switch (state)
        {
            case State.Network:
                gotoState(State.Menu);
                break;
            case State.Menu:
                break;
            case State.Ice:
            case State.Attack:
            case State.OtherAttack:
            case State.OtherIce:
            case State.Begin:
            case State.Wait:
                break;
            case State.End:
                break;
            case State.Pause:
                break;
        }
    }

    public void gotoState(State _state)
    {
        switch (_state)
        {
            case State.Network:
                //Hosting, Joining a game
                state = State.Network;
                networkController.updateState();
                mainMenuManager.disable();
                break;
            case State.Menu:
                //Main Menu
                state = State.Menu;
                mainMenuManager.enable();
                networkController.disconnect();
                break;
            case State.Ice:
                //Icing your own area
                state = State.Ice;
                grid.showGrid(Grid.Type.Player);
                break;
            case State.Begin:
                //Tutorial?
                state = State.Begin;
                waitScreen.SetActive(true);
                gameUIController.Notify(networkController.currentRoom.name);
                initGame();
                mainMenuManager.disable();
                playerReady = true;
                break;
            case State.OtherIce:
                //Waiting for other player to ice
                waitScreen.SetActive(true);
                playerIce = true;
                state = State.OtherIce;
                break;
            case State.OtherAttack:
                //Waiting for other player to attack;
                waitScreen.SetActive(true);
                playerAttack = true;
                state = State.OtherAttack;
                break;
            case State.Pause:
                //Game paused
                state = State.Pause;
                break;
            case State.End:
                //Game has ended
                state = State.End;
                break;
            case State.Attack:
                //Attacking other player
                state = State.Attack;
                grid.showGrid(Grid.Type.Other);
                break;
            case State.Wait:
                //Waiting for other player to be ready
                state = State.Wait;
                waitScreen.SetActive(true);
                playerReady = true;
                photonView.RPC("ready", networkController.otherPlayer);
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        detectBack();
        switch (state)
        {
            case State.Ice:
                detectHit();
                break;
            case State.Attack:
                detectHit();
                break;
            case State.Menu:
                break;
            case State.Begin:
                if (otherReady && playerReady)
                {
                    networkController.retrieveOtherPlayer();
                    otherReady = false;
                    playerReady = false;
                    waitScreen.SetActive(false);
                    gameUIController.EndNotify();
                    beginRound();
                }
                break;
            case State.Pause:
                break;
            case State.OtherIce:
                if (otherIce && playerIce)
                {
                    waitScreen.SetActive(false);
                    gotoState(State.Attack);
                    otherIce = false;
                    playerIce = false;
                }
                break;
            case State.OtherAttack:
                if (otherAttack && playerAttack)
                {
                    waitScreen.SetActive(false);
                    gotoState(State.Wait);
                    otherAttack = false;
                    playerAttack = false;
                }
                break;
            case State.End:
                break;
            case State.Wait:
                if (otherReady && playerReady)
                {
                    waitScreen.SetActive(false);
                    beginRound();
                    otherReady = false;
                    playerReady = false;
                }
                break;
            default:
                break;
        }
    }

    void detectBack()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            back();
        }
    }

    void detectHit()
    {
        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    hit(Input.GetTouch(0).position);
                }
            }
        }
        else if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsWebPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hit(Input.mousePosition);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                superhit(Input.mousePosition);
            }
        }
    }

    void hit(Vector3 position)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out hit))
        {
            Square square = hit.collider.gameObject.GetComponent<Square>();
            if (state.Equals(State.Attack) && !playerAttack)
            {
                playerAttack = true;
                photonView.RPC("getAttacked", networkController.otherPlayer, square.xIndex, square.yIndex);
                eventManager.addEvent(() => grid.attack(Grid.Type.Other, square.xIndex, square.yIndex), 1, true);
                eventManager.addEvent(() => gotoState(State.OtherAttack), 2, true);
            }
            else if (state.Equals(State.Ice) && !playerIce)
            {
                playerIce = true;
                grid.ice(Grid.Type.Player, square.xIndex, square.yIndex);
                photonView.RPC("ice", networkController.otherPlayer);
                eventManager.addEvent(() => gotoState(State.OtherIce), 1, true);
            }
        }
    }

    void superhit(Vector3 position)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out hit))
        {
            Square square = hit.collider.gameObject.GetComponent<Square>();
            if (state.Equals(State.Attack) && !playerAttack)
            {
                playerAttack = true;
                photonView.RPC("getAttacked", networkController.otherPlayer, square.xIndex, square.yIndex);
                eventManager.addEvent(() => grid.attack(Grid.Type.Other, square.xIndex, square.yIndex), 1, true);
                eventManager.addEvent(() => gotoState(State.OtherAttack), 2, true);
            }
            else if (state.Equals(State.Ice) && !playerIce)
            {
                playerIce = true;
                grid.superice(Grid.Type.Player, square.xIndex, square.yIndex);
                photonView.RPC("ice", networkController.otherPlayer);
                eventManager.addEvent(() => gotoState(State.OtherIce), 1, true);
            }
        }
    }

    [PunRPC]
    public void ice()
    {
        otherIce = true;
    }

    [PunRPC]
    public void getAttacked(int x, int y)
    {
        otherAttack = true;
        grid.superreveal(Grid.Type.Player, x, y);
        grid.attack(Grid.Type.Player, x, y);
    }

    [PunRPC]
    public void revealSquare(Square.State state, int x, int y) 
    {
        grid.setSquare(Grid.Type.Other, state, x, y);
    }

    public void updateOther(Square square)
    {
        photonView.RPC("revealSquare", networkController.otherPlayer, square.state, square.xIndex, square.yIndex);
    }

    public void initGame()
    {
        grid.InitializeGrid();
    }

    public void dropGame()
    {
        networkController.quitRoom();
        resetGame();
        gotoState(State.Network);
    }

    public void winGame()
    {
        gotoState(State.End);
        playerReady = false;
        otherReady = false;
        photonView.RPC("loseGame", networkController.otherPlayer);
        gameUIController.showWin();
    }

    [PunRPC]
    public void loseGame()
    {
        gotoState(State.End);
        playerReady = false;
        otherReady = false;
        gameUIController.showLose();
    }

    void beginRound()
    {
        grid.fade(Grid.Type.Other);
        gotoState(State.Ice);
    }

    public void replay()
    {
        resetGame();
        initGame();
        gotoState(State.Wait);
    }

    public void resetGame()
    {
        grid.Destroy();
        playerIce = false;
        playerAttack = false;
        otherIce = false;
        otherAttack = false;
        playerReady = false;
        otherReady = false;
    }

    void OnGUI()
    {
        GUILayout.Label(state.ToString());
    }
}
