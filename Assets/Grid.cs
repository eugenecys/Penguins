using UnityEngine;
using System.Collections;

public class Grid : Singleton<Grid> {

    GameController gameController;

    public enum Type
    {
        Player = 0,
        Other = 1
    }

    public int length = 10;
    public int height = 18;
    
    public float screenWidth = 5;
    public float screenHeight = 9;

    public Square[,,] grids;
    
    public GameObject squarePrefab;

    private bool[,] traversePlayerGrid;

    public GameObject[] playerGridsObjects;

    void Awake()
    {
        gameController = GameController.Instance;
    }

	// Use this for initialization
	void Start () {
       
	}

    // Update is called once per frame
    void Update()
    {

    }
    
    public void InitializeGrid()
    {
        grids = new Square[2, length, height];
        playerGridsObjects = new GameObject[2];
        PopulateSquares();
        initializeSquares();
    }

    void PopulateSquares()
    {
        squarePrefab = Resources.Load("Square") as GameObject;
        float lStep = 1.0f * screenWidth / length;
        float hStep = 1.0f * screenHeight / height;
        playerGridsObjects[0] = new GameObject("Player Grid");
        playerGridsObjects[1] = new GameObject("Other Grid");
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float xPos = i * lStep - screenWidth / 2 + lStep / 2;
                float yPos = j * hStep - screenHeight / 2 + hStep / 2;
                createSquare(i, j, xPos, yPos, Type.Player);
                createSquare(i, j, xPos, yPos, Type.Other);
            }
        }
    }

    void createSquare(int xIndex, int yIndex, float x, float y, Type type)
    {
        GameObject sObj = Object.Instantiate(squarePrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        sObj.transform.parent = playerGridsObjects[(int)type].transform;
        sObj.transform.localScale = new Vector3(1.0f/(length/screenWidth), 1.0f/(height/screenHeight), 1);
        Square square = sObj.GetComponent<Square>();
        square.xIndex = xIndex;
        square.yIndex = yIndex;
        grids[(int) type, xIndex, yIndex] = square; 
    }

    void initializeSquares()
    {

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grids[0, i, j].setState(Square.State.Empty);
                grids[1, i, j].setState(Square.State.EmptyInactive);
            }
        }
    }

    public void showGrid(Type type)
    {
        switch(type)
        {
            case Type.Player:
                hide(playerGridsObjects[1]);
                show(playerGridsObjects[0]);
                break;
            case Type.Other:
                hide(playerGridsObjects[0]);
                show(playerGridsObjects[1]);
                break;
        }
    }

    public void hide(GameObject grid)
    {
        grid.transform.position = new Vector3(0, 0, 1);
        grid.transform.localScale = new Vector3(0, 0, 0);
    }

    public void show(GameObject grid)
    {
        grid.transform.position = new Vector3(0, 0, -1);
        grid.transform.localScale = new Vector3(1, 1, 1);
    }

    public void superice(Type playerType, int x, int y)
    {
        int type = (int)playerType;
        grids[type, x, y].setState(Square.State.Ice);
        if (y > 0)
        {
            grids[type, x, y - 1].setState(Square.State.Ice);
        }
        if (y > 1)
        {
            grids[type, x, y - 2].setState(Square.State.Ice);
        }
        if (y < height - 1)
        {
            grids[type, x, y + 1].setState(Square.State.Ice);
        }
        if (y < height - 2)
        {
            grids[type, x, y + 2].setState(Square.State.Ice);
        }
        if (x > 0)
        {
            grids[type, x - 1, y].setState(Square.State.Ice);
            if (y > 0)
            {
                grids[type, x - 1, y - 1].setState(Square.State.Ice);
            }
            if (y < height - 1)
            {
                grids[type, x - 1, y + 1].setState(Square.State.Ice);
            }
            if (y > 1)
            {
                grids[type, x - 1, y - 2].setState(Square.State.Ice);
            }
            if (y < height - 2)
            {
                grids[type, x - 1, y + 2].setState(Square.State.Ice);
            }
        }

        if (x < length - 1)
        {
            grids[type, x + 1, y].setState(Square.State.Ice);
            if (y > 0)
            {
                grids[type, x + 1, y - 1].setState(Square.State.Ice);
            }
            if (y < height - 1)
            {
                grids[type, x + 1, y + 1].setState(Square.State.Ice);
            }
            if (y > 1)
            {
                grids[type, x + 1, y - 2].setState(Square.State.Ice);
            }
            if (y < height - 2)
            {
                grids[type, x + 1, y + 2].setState(Square.State.Ice);
            }
        }

        if (x > 1)
        {
            grids[type, x - 2, y].setState(Square.State.Ice);
            if (y > 0)
            {
                grids[type, x - 2, y - 1].setState(Square.State.Ice);
            }
            if (y < height - 1)
            {
                grids[type, x - 2, y + 1].setState(Square.State.Ice);
            }
        }
        if (x < length - 1)
        {
            grids[type, x + 2, y].setState(Square.State.Ice);
            if (y > 0)
            {
                grids[type, x + 2, y - 1].setState(Square.State.Ice);
            }
            if (y < height - 1)
            {
                grids[type, x + 2, y + 1].setState(Square.State.Ice);
            }
        }

        bool found = searchPath();
        Debug.Log(found);

        if (found)
        {
            gameController.winGame();
        }
    }
    
    public void ice(Type playerType, int x, int y)
    {
        int type = (int)playerType;
        grids[type, x, y].setState(Square.State.Ice);
        if (y > 0)
        {
            grids[type, x, y - 1].setState(Square.State.Ice);
        }
        if (y < height - 1)
        {
            grids[type, x, y + 1].setState(Square.State.Ice);
        }
        if (x > 0)
        {
            grids[type, x - 1, y].setState(Square.State.Ice);
            if (y > 0)
            {
                grids[type, x - 1, y - 1].setState(Square.State.Ice);
            }
            if (y < height - 1)
            {
                grids[type, x - 1, y + 1].setState(Square.State.Ice);
            }
        }

        if (x < length - 1)
        {
            grids[type, x + 1, y].setState(Square.State.Ice);
            if (y > 0)
            {
                grids[type, x + 1, y - 1].setState(Square.State.Ice);
            }
            if (y < height - 1)
            {
                grids[type, x + 1, y + 1].setState(Square.State.Ice);
            }
        }

        bool found = searchPath();
        Debug.Log(found);

        if (found)
        {
            gameController.winGame();
        }
    }

    public void destroy(Type playerType, int x, int y)
    {
        int type = (int)playerType;
        
        grids[type, x, y].setState(Square.State.Empty);
        if (y > 0)
        {
            grids[type, x, y - 1].setState(Square.State.Empty);
        }
        if (y < height - 1)
        {
            grids[type, x, y + 1].setState(Square.State.Empty);
        }
        if (x > 0)
        {
            grids[type, x - 1, y].setState(Square.State.Empty);
            if (y > 0)
            {
                grids[type, x - 1, y - 1].setState(Square.State.Empty);
            }
            if (y < height - 1)
            {
                grids[type, x - 1, y + 1].setState(Square.State.Empty);
            }
        }

        if (x < length - 1)
        {
            grids[type, x + 1, y].setState(Square.State.Empty);
            if (y > 0)
            {
                grids[type, x + 1, y - 1].setState(Square.State.Empty);
            }
            if (y < height - 1)
            {
                grids[type, x + 1, y + 1].setState(Square.State.Empty);
            }
        }

    }

    public Square getSquare(Type playerType, int x, int y)
    {
        int type = (int)playerType;
        return grids[type, x, y];
    }

    public void setSquare(Type playerType, Square.State _state, int x, int y)
    {
        int type = (int)playerType;
        grids[type, x, y].setState(_state);
    }

    public void superreveal (Type playerType, int x, int y)
    {
        int type = (int)playerType;
        grids[type, x, y].activate();
        gameController.updateOther(grids[type, x, y]);
        if (y > 0)
        {
            grids[type, x, y - 1].activate();
            gameController.updateOther(grids[type, x, y - 1]);
        }
        if (y > 1)
        {
            grids[type, x, y - 2].activate();
            gameController.updateOther(grids[type, x, y - 2]);
        }
        if (y < height - 1)
        {
            grids[type, x, y + 1].activate();
            gameController.updateOther(grids[type, x, y + 1]);
        }
        if (y < height - 2)
        {
            grids[type, x, y + 2].activate();
            gameController.updateOther(grids[type, x, y + 2]);
        }
        if (x > 0)
        {
            grids[type, x - 1, y].activate();
            gameController.updateOther(grids[type, x - 1, y]);
            if (y > 0)
            {
                grids[type, x - 1, y - 1].activate();
                gameController.updateOther(grids[type, x - 1, y - 1]);
            }
            if (y < height - 1)
            {
                grids[type, x - 1, y + 1].activate();
                gameController.updateOther(grids[type, x - 1, y + 1]);
            }
            if (y > 1)
            {
                grids[type, x - 1, y - 2].activate();
                gameController.updateOther(grids[type, x - 1, y - 2]);
            }
            if (y < height - 2)
            {
                grids[type, x - 1, y + 2].activate();
                gameController.updateOther(grids[type, x - 1, y + 2]);
            }
        }

        if (x < length - 1)
        {
            grids[type, x + 1, y].activate();
            gameController.updateOther(grids[type, x + 1, y]);
            if (y > 0)
            {
                grids[type, x + 1, y - 1].activate();
                gameController.updateOther(grids[type, x + 1, y - 1]);
            }
            if (y < height - 1)
            {
                grids[type, x + 1, y + 1].activate();
                gameController.updateOther(grids[type, x + 1, y + 1]);
            }
            if (y > 1)
            {
                grids[type, x + 1, y - 2].activate();
                gameController.updateOther(grids[type, x + 1, y - 2]);
            }
            if (y < height - 2)
            {
                grids[type, x + 1, y + 2].activate();
                gameController.updateOther(grids[type, x + 1, y + 2]);
            }
        }

        if (x > 1)
        {
            grids[type, x - 2, y].activate();
            gameController.updateOther(grids[type, x - 2, y]);
            if (y > 0)
            {
                grids[type, x - 2, y - 1].activate();
                gameController.updateOther(grids[type, x - 2, y - 1]);
            }
            if (y < height - 1)
            {
                grids[type, x - 2, y + 1].activate();
                gameController.updateOther(grids[type, x - 2, y + 1]);
            }
        }
        if (x < length - 1)
        {
            grids[type, x + 2, y].activate();
            gameController.updateOther(grids[type, x + 2, y]);
            if (y > 0)
            {
                grids[type, x + 2, y - 1].activate();
                gameController.updateOther(grids[type, x + 2, y - 1]);
            }
            if (y < height - 1)
            {
                grids[type, x + 2, y + 1].activate();
                gameController.updateOther(grids[type, x + 2, y + 1]);
            }
        }

        bool found = searchPath();
        Debug.Log(found);

        if (found)
        {
            gameController.winGame();
        }
    }

    public void reveal(Type playerType, int x, int y)
    {
        int type = (int)playerType;
        grids[type, x, y].activate();
        gameController.updateOther(grids[type, x, y]);
        if (y > 0)
        {
            grids[type, x, y - 1].activate();
            gameController.updateOther(grids[type, x, y - 1]);
        }
        if (y < height - 1)
        {
            grids[type, x, y + 1].activate();
            gameController.updateOther(grids[type, x, y + 1]);
        }
        if (x > 0)
        {
            grids[type, x - 1, y].activate();
            gameController.updateOther(grids[type, x - 1, y]);

            if (y > 0)
            {
                grids[type, x - 1, y - 1].activate();
                gameController.updateOther(grids[type, x - 1, y - 1]);
            }
            if (y < height - 1)
            {
                grids[type, x - 1, y + 1].activate();
                gameController.updateOther(grids[type, x - 1, y + 1]);
            }
        }

        if (x < length - 1)
        {
            grids[type, x + 1, y].activate();
            gameController.updateOther(grids[type, x + 1, y]);
            if (y > 0)
            {
                grids[type, x + 1, y - 1].activate();
                gameController.updateOther(grids[type, x + 1, y - 1]);
            }
            if (y < height - 1)
            {
                grids[type, x + 1, y + 1].activate();
                gameController.updateOther(grids[type, x + 1, y + 1]);
            }
        }
    }
    
    bool searchPath()
    {
        for (int i = 0; i < length; i++)
        {
            if (grids[0, i, 0].iced)
            {
                traversePlayerGrid = new bool[length, height];
                for (int j = 0; j < length; j++)
                {
                    for (int k = 0; k < height; k++)
                    {
                        traversePlayerGrid[j, k] = false;
                    }
                }
                if (DFS(i, 0))
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool DFS(int x, int y)
    {
        if (x < 0 || x >= length || y < 0 || y >= height)
        {
            return false;
        }
        if (!grids[0, x, y].iced)
        {
            return false;
        }
        if (y == height - 1)
        {
            return true;
        }
        if (!traversePlayerGrid[x, y])
        {
            traversePlayerGrid[x, y] = true;
            bool result = false;
            if (DFS(x - 1, y))
            {
                result = true;
            }
            if (DFS(x + 1, y))
            {
                result = true;
            }
            if (DFS(x, y - 1))
            {
                result = true;
            }
            if (DFS(x, y + 1))
            {
                result = true;
            }
            return result;
        }
        else
        {
            return false;
        }
    }
    
    public void deactivate(Type playerType)
    {
        int type = (int) playerType;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grids[type, i, j].deactivate();
            }
        }
    }

    public void fade(Type playerType)
    {
        int type = (int)playerType;

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grids[type, i, j].fade();
            }
        }
    }

    //Old methods, need refactor
    /*
    
    public void refreshOther(int player)
    {
        int other = (player + 1) % 2;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grids[type,player, i, j].active)
                {
                    grids[type,player, i, j].setState(grids[type,other, i, j].state);
                }
            }
        }
    }


    */
}
