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
        EmptyFading
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
        if (state.Equals(State.Ice) || state.Equals(State.IceInactive) || state.Equals(State.IceFading))
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
        if (state.Equals(State.Ice) || state.Equals(State.IceInactive) || state.Equals(State.IceFading))
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
                fadeNum = 0;
                iceSprite.SetActive(false);
                dirtSprite.SetActive(true);
                iceInactiveSprite.SetActive(false);
                dirtInactiveSprite.SetActive(false);
                for(int i = 0; i < dirtFadeSprites.Length;i++)
                {
                    dirtFadeSprites[i].SetActive(false);
                }
                for(int i = 0; i < iceFadeSprites.Length; i++)
                {
                    iceFadeSprites[i].SetActive(false);
                }
                break;
            case State.Ice:
                state = State.Ice;
                fadeNum = 0;
                iceSprite.SetActive(true);
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
                break;
            case State.EmptyInactive:
                state = State.EmptyInactive;
                iceSprite.SetActive(false);
                dirtSprite.SetActive(false);
                iceInactiveSprite.SetActive(false);
                dirtInactiveSprite.SetActive(true);
                for (int i = 0; i < dirtFadeSprites.Length; i++)
                {
                    dirtFadeSprites[i].SetActive(false);
                }
                for (int i = 0; i < iceFadeSprites.Length; i++)
                {
                    iceFadeSprites[i].SetActive(false);
                }
                break;
            case State.IceInactive:
                state = State.IceInactive;
                iceSprite.SetActive(false);
                dirtSprite.SetActive(false);
                iceInactiveSprite.SetActive(true);
                dirtInactiveSprite.SetActive(false);
                for (int i = 0; i < dirtFadeSprites.Length; i++)
                {
                    dirtFadeSprites[i].SetActive(false);
                }
                for (int i = 0; i < iceFadeSprites.Length; i++)
                {
                    iceFadeSprites[i].SetActive(false);
                }
                break;
            case State.EmptyFading:
                state = State.EmptyFading;
                iceSprite.SetActive(false);
                dirtSprite.SetActive(false);
                iceInactiveSprite.SetActive(false);
                dirtInactiveSprite.SetActive(false);
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
                iceSprite.SetActive(false);
                dirtSprite.SetActive(false);
                iceInactiveSprite.SetActive(false);
                dirtInactiveSprite.SetActive(false);
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
        }
    }
    
    public void fade()
    {
        if (state.Equals(State.Empty) || state.Equals(State.EmptyFading))
        {
            setState(State.EmptyFading);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
