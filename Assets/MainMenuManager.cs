using UnityEngine;
using System.Collections;

public class MainMenuManager : Singleton<MainMenuManager> {

    GameController gameController;
    public Canvas canvas;

    public void enable()
    {
        canvas.gameObject.SetActive(true);
    }
    public void disable()
    {
        canvas.gameObject.SetActive(false);
    }

    public void Play()
    {
        gameController.gotoState(GameController.State.Network);
    }

    public void Quit()
    {
        Application.Quit();
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
}
