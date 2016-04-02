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

    public enum Type
    {
        Player,
        Other
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

    public GameObject ice;
    public GameObject dirt;
    public GameObject iceInactive;
    public GameObject dirtInactive;

    public State state;
    public Type type;

    public int xIndex;
    public int yIndex;

	// Use this for initialization
	void Start () 
    {
        grid = Grid.Instance;
	}

    public void hit(int player)
    {
        if (type.Equals(Type.Player))
        {
            hitIce(player);
        }
        else
        {
            reveal(player);
        }
    }

    public void init(Type _type)
    {
        type = _type;
        if (type.Equals(Type.Player))
        {
            setEmpty();
        }
        else
        {
            setEmptyInactive();
        }
    }

    public void reveal(int player)
    {
        grid.reveal(player, xIndex, yIndex);
    }

    public void deactivate()
    {
        if (state.Equals(State.Ice) || state.Equals(State.IceInactive))
        {
            setIceInactive();
        }
        else
        {
            setEmptyInactive();
        }
    }

    public void setState(State _state)
    {
        switch (_state)
        {
            case State.Empty:
                setEmpty();
                break;
            case State.Ice:
                setIce();
                break;
            case State.EmptyInactive:
                setEmptyInactive();
                break;
            case State.IceInactive:
                setIceInactive();
                break;
        }
    }

    public void activate()
    {
        if (state.Equals(State.Ice) || state.Equals(State.IceInactive))
        {
            setIce();
        }
        else
        {
            setEmpty();
        }
    }

    public void hitEmpty(int player)
    {
        grid.hitEmpty(player, xIndex, yIndex);
    }

    public void hitIce(int player)
    {
        grid.hitIce(player, xIndex, yIndex);
    }

    public void setEmptyInactive()
    {
        state = State.EmptyInactive;
        ice.SetActive(false);
        dirt.SetActive(false);
        iceInactive.SetActive(false);
        dirtInactive.SetActive(true);
    }

    public void setIceInactive()
    {
        state = State.IceInactive;
        ice.SetActive(false);
        dirt.SetActive(false);
        iceInactive.SetActive(true);
        dirtInactive.SetActive(false);
    }

    public void setEmpty()
    {
        state = State.Empty;
        ice.SetActive(false);
        dirt.SetActive(true);
        iceInactive.SetActive(false);
        dirtInactive.SetActive(false);
    }

    public void setIce()
    {
        state = State.Ice;
        ice.SetActive(true);
        dirt.SetActive(false);
        iceInactive.SetActive(false);
        dirtInactive.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
