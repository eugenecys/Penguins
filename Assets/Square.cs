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
        EmptyHighlight,
    }

    public enum TileColor
    {
        Red,
        Blue,
        Neutral
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
    public GameObject blackSprite;
    public GameObject whiteSprite;
    public GameObject[] explosionSprite;
    public GameObject redLayer;
    public GameObject blueLayer;

    public Animator flame;

    public State state;
    public TileColor color;
    private int fadeNum;

    public int xIndex;
    public int yIndex;

    void Awake()
    {
        
    }

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

    public void setColor(TileColor _color)
    {
        blueLayer.SetActive(false);
        redLayer.SetActive(false);
        color = _color;
        switch(color)
        {
            case TileColor.Blue:
                blueLayer.SetActive(true);
                break;
            case TileColor.Red:
                redLayer.SetActive(true);
                break;
            case TileColor.Neutral:
                break;
        }
    }

    public void setState(State _state)
    {
        iceSprite.SetActive(false);
        dirtSprite.SetActive(false);
        blackSprite.SetActive(false);
        whiteSprite.SetActive(false);
        for (int i = 0; i < explosionSprite.Length; i++)
        {
            explosionSprite[i].SetActive(false);
        }
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
                dirtSprite.SetActive(true);
                blackSprite.SetActive(true);
                break;
            case State.IceInactive:
                state = State.IceInactive;
                iceSprite.SetActive(true);
                blackSprite.SetActive(true);
                break;
            case State.Fading:
                state = State.Fading;
                dirtSprite.SetActive(true);
                explosionSprite[fadeNum].SetActive(true);
                break;
            case State.FadingInactive:
                state = State.FadingInactive;
                dirtSprite.SetActive(true);
                explosionSprite[fadeNum].SetActive(true);
                blackSprite.SetActive(true);
                break;
            case State.EmptyHighlight:
                state = State.EmptyHighlight;
                dirtSprite.SetActive(true);
                whiteSprite.SetActive(true);
                break;
            case State.IceHighlight:
                state = State.IceHighlight;
                iceSprite.SetActive(true);
                whiteSprite.SetActive(true);
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
            if (fadeNum >= explosionSprite.Length)
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
            if (fadeNum >= explosionSprite.Length)
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
