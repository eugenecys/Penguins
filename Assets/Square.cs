using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {

    Grid grid;

    public enum State
    {
        Ice,
        Empty,
        IceInactive,
        EmptyInactive,
        IceFading,
        EmptyFading,
        IceHighlight,
        EmptyHighlight
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

    public bool isIce
    {
        get
        {
            return state.Equals(State.Ice) || state.Equals(State.IceHighlight) || state.Equals(State.IceFading) || state.Equals(State.IceInactive);
        }
    }

    public bool isEmpty
    {
        get
        {
            return state.Equals(State.Empty) || state.Equals(State.EmptyHighlight) || state.Equals(State.EmptyFading) || state.Equals(State.EmptyInactive);
        }
    }

    public GameObject iceSprite;
    public GameObject dirtSprite;
    public GameObject iceInactiveSprite;
    public GameObject dirtInactiveSprite;

    public GameObject dirtHighlightSprite;
    public GameObject iceHighlightSprite;

    public GameObject[] dirtFadeSprites;
    public GameObject[] iceFadeSprites;

    public State state;
    private int fadeNum;

    public int xIndex;
    public int yIndex;

    // Use this for initialization
    void Start()
    {
        grid = Grid.Instance;
        fadeNum = 0;
    }

    public void deactivate()
    {
        if (isIce)
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
        if (isIce)
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
        iceSprite.SetActive(false);
        dirtSprite.SetActive(false);
        iceInactiveSprite.SetActive(false);
        dirtInactiveSprite.SetActive(false);
        for (int i = 0; i < dirtFadeSprites.Length; i++)
        {
            dirtFadeSprites[i].SetActive(false);
        }
        for (int i = 0; i < iceFadeSprites.Length; i++)
        {
            iceFadeSprites[i].SetActive(false);
        }
        dirtHighlightSprite.SetActive(false);
        iceHighlightSprite.SetActive(false);
        switch (_state)
        {
            case State.Empty:
                state = State.Empty;
                dirtSprite.SetActive(true);
                fadeNum = 0;
                break;
            case State.Ice:
                state = State.Ice;
                iceSprite.SetActive(true);
                fadeNum = 0;
                break;
            case State.EmptyInactive:
                state = State.EmptyInactive;
                dirtInactiveSprite.SetActive(true);
                break;
            case State.IceInactive:
                state = State.IceInactive;
                iceInactiveSprite.SetActive(true);
                break;
            case State.EmptyFading:
                state = State.EmptyFading;
                if (fadeNum >= dirtFadeSprites.Length)
                {
                    deactivate();
                }
                else
                {
                    for(int i = 0; i < dirtFadeSprites.Length; i++)
                    {
                        if (i == fadeNum)
                        {
                            dirtFadeSprites[i].SetActive(true);
                        }
                        else
                        {
                            dirtFadeSprites[i].SetActive(false);
                        }
                    }
                    fadeNum++;
                }
                break;
            case State.IceFading:
                state = State.IceFading;
                if (fadeNum >= iceFadeSprites.Length)
                {
                    deactivate();
                }
                else
                {
                    for (int i = 0; i < iceFadeSprites.Length; i++)
                    {
                        if (i == fadeNum)
                        {
                            iceFadeSprites[i].SetActive(true);
                        }
                        else
                        {
                            iceFadeSprites[i].SetActive(false);
                        }
                        fadeNum++;
                    }
                }
                break;
            case State.EmptyHighlight:
                state = State.EmptyHighlight;
                dirtHighlightSprite.SetActive(true);
                break;
            case State.IceHighlight:
                state = State.IceHighlight;
                iceHighlightSprite.SetActive(true);
                break;
        }
    }
    
    public void unhighlight()
    {
        if (state.Equals(State.IceHighlight))
        {
            setState(State.Ice);
        }
        else if (state.Equals(State.EmptyHighlight))
        {
            setState(State.Empty);
        }
    }

    public void highlight()
    {
        if (isIce)
        {
            setState(State.IceHighlight);
        } else
        {
            setState(State.EmptyHighlight);
        }
    }

    public void fade()
    {
        if (state.Equals(State.Empty) || state.Equals(State.EmptyFading))
        {
            setState(State.EmptyFading);
        }
        else if (state.Equals(State.Ice) || state.Equals(State.IceFading)) 
        {
            setState(State.IceFading);
        }
    }
    
	// Update is called once per frame
	void Update () {
	
	}
}
