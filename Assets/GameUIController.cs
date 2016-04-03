using UnityEngine;
using System.Collections;

public class GameUIController : Singleton<GameUIController> {

    public TextMesh centerText;
    public TextMesh subText;
    public GameObject iceBG;
    public GameObject icedirtBG;

    EventManager eventManager;

    public void endGame(int player)
    {
        centerText.gameObject.SetActive(true);
        centerText.text = "Player " + player.ToString() + " wins";
    }

    void Awake()
    {
        eventManager = EventManager.Instance;
    }

	// Use this for initialization
	void Start () {
        
	}

    public void countdown(int player)
    {
        showUI();
        icedirtBG.SetActive(false);
        centerText.text = "Player " + player.ToString() + "'s turn";
        subText.text = "";
        eventManager.addEvent(() => { subText.text = "3"; }, 1, true);
        eventManager.addEvent(() => { subText.text = "2"; }, 2, true);
        eventManager.addEvent(() => { subText.text = "1"; }, 3, true);
        eventManager.addEvent(hideUI, 4, true);
    }

    public void hideUI()
    {
        centerText.gameObject.SetActive(false);
        subText.gameObject.SetActive(false);
        iceBG.SetActive(false);
        icedirtBG.SetActive(false);        
    }

    public void showUI()
    {
        centerText.gameObject.SetActive(true);
        subText.gameObject.SetActive(true);
        iceBG.SetActive(true);
        icedirtBG.SetActive(true);
    }

    public void showStartMenu()
    {
        showUI();
        centerText.text = "Ice Age";
        subText.text = "Press Enter to begin";
        iceBG.SetActive(true);
        icedirtBG.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
