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
        Fading,
        FadingInactive,
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
            return (state.Equals(State.Ice) || state.Equals(State.Empty) || state.Equals(State.Fading));
        }
    }

    public bool isIce
    {
        get
        {
            return state.Equals(State.Ice) || state.Equals(State.IceHighlight)|| state.Equals(State.IceInactive);
        }
    }

    public bool isEmpty
    {
        get
        {
            return state.Equals(State.Empty) || state.Equals(State.EmptyHighlight) || state.Equals(State.Fading) || state.Equals(State.EmptyInactive) || state.Equals(State.FadingInactive);
        }
    }

    public bool isFading
    {
        get
        {
            return state.Equals(State.Fading) || state.Equals(State.FadingInactive);
        }
    }

    public GameObject iceSprite;
    public GameObject dirtSprite;
    public GameObject iceInactiveSprite;
    public GameObject dirtInactiveSprite;

    public GameObject dirtHighlightSprite;
    public GameObject iceHighlightSprite;

    public GameObject[] dirtFadeSprites;
    public GameObject[] dirtFadeInactiveSprites;

    public Animator flame;

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
        else if (isFading)
        {
            setState(State.FadingInactive);
        }
        else {
            setState(State.EmptyInactive);
        }
    }

    public void activate()
    {
        if (isIce)
        {
            setState(State.Ice);
        }
        else if (isFading)
        {
            setState(State.Fading);
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
        for (int i = 0; i < dirtFadeInactiveSprites.Length; i++)
        {
            dirtFadeInactiveSprites[i].SetActive(false);
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
            case State.Fading:
                state = State.Fading;
                dirtFadeSprites[fadeNum].SetActive(true);
                break;
            case State.FadingInactive:
                state = State.FadingInactive;
                dirtFadeInactiveSprites[fadeNum].SetActive(true);
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

    public void explode()
    {
        flame.SetTrigger("explode");
        fadeNum = 0;
        setState(State.Fading);
    }
    
    public void fade()
    {
        if (state.Equals(State.Fading))
        {
            fadeNum++;
            if (fadeNum >= dirtFadeSprites.Length)
            {
                setState(State.Empty);
                fadeNum = 0;
            }
            else
            {
                setState(State.Fading);
            }
        }
        else if (state.Equals(State.FadingInactive))
        {
            fadeNum++;
            if (fadeNum >= dirtFadeInactiveSprites.Length)
            {
                setState(State.EmptyInactive);
                fadeNum = 0;
            }
            else
            {
                setState(State.FadingInactive);
            }
        }
    }
    
    public void ice()
    {
        if (!state.Equals(State.Fading) && !state.Equals(State.FadingInactive))
        {
            if (active)
            {
                setState(State.Ice);
            }
            else
            {
                setState(State.IceInactive);
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
