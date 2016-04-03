using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {

    Grid grid;

    public enum State
    {
        Ice, 
        Empty,
        IceInactive,
        EmptyInactive
    }

    public bool iced
    {
        get
        {
            return state.Equals(State.Ice);
        }
    }

    public bool active
    {
        get
        {
            return (state.Equals(State.Ice) || state.Equals(State.Empty));
        }
    }

    public GameObject iceSprite;
    public GameObject dirtSprite;
    public GameObject iceInactiveSprite;
    public GameObject dirtInactiveSprite;

    public State state;

    public int xIndex;
    public int yIndex;

	// Use this for initialization
	void Start () 
    {
        grid = Grid.Instance;
	}
    
    public void deactivate()
    {
        if (state.Equals(State.Ice) || state.Equals(State.IceInactive))
        {
            setState(State.IceInactive);
        }
        else
        {
            setState(State.EmptyInactive);
        }
    }

    public void activate()
    {
        if (state.Equals(State.Ice) || state.Equals(State.IceInactive))
        {
            setState(State.Ice);
        }
        else
        {
            setState(State.Empty);
        }
    }

    public void setState(State _state)
    {
        switch (_state)
        {
            case State.Empty:
                state = State.Empty;
                iceSprite.SetActive(false);
                dirtSprite.SetActive(true);
                iceInactiveSprite.SetActive(false);
                dirtInactiveSprite.SetActive(false);
                break;
            case State.Ice:
                state = State.Ice;
                iceSprite.SetActive(true);
                dirtSprite.SetActive(false);
                iceInactiveSprite.SetActive(false);
                dirtInactiveSprite.SetActive(false);
                break;
            case State.EmptyInactive:
                state = State.EmptyInactive;
                iceSprite.SetActive(false);
                dirtSprite.SetActive(false);
                iceInactiveSprite.SetActive(false);
                dirtInactiveSprite.SetActive(true);
                break;
            case State.IceInactive:
                state = State.IceInactive;
                iceSprite.SetActive(false);
                dirtSprite.SetActive(false);
                iceInactiveSprite.SetActive(true);
                dirtInactiveSprite.SetActive(false);
                break;
        }
    }
    
	// Update is called once per frame
	void Update () {
	
	}
}
