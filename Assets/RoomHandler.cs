using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomHandler : MonoBehaviour {

    NetworkMenuManager networkMenuManager;
    public Text textObj;

    public void join()
    {
        networkMenuManager.join(textObj.text);
    }
    
    void Awake()
    {
        networkMenuManager = NetworkMenuManager.Instance;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
