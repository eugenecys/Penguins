using UnityEngine;
using System.Collections;

public class Grid : Singleton<Grid> {

    public int length = 16;
    public int height = 24;

    public float playerX = -6f;
    public float playerY = 0f;

    public float otherX = 6f;
    public float otherY = 0f;

    public float screenWidth = 8;
    public float screenHeight = 12;

    public Square[,,] playerGrid;
    public Square[,,] otherGrid;

    public GameObject squarePrefab;

    private bool[,] traverseplayerGrid;

    public GameObject[] playerGridsObjects;

	// Use this for initialization
	void Start () {
       
	}

    public void InitializeGrid()
    {
        playerGrid = new Square[2, length, height];
        otherGrid = new Square[2, length, height];
        playerGridsObjects = new GameObject[2];
        PopulateSquares();
    }

    void PopulateSquares()
    {
        squarePrefab = Resources.Load("Square") as GameObject;
        float lStep = 1.0f * screenWidth / length;
        float hStep = 1.0f * screenHeight / height;
        playerGridsObjects[0] = new GameObject("Player 1 Grid");
        playerGridsObjects[1] = new GameObject("Player 2 Grid");
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float xPos = i * lStep - screenWidth / 2 + lStep / 2 + playerX;
                float yPos = j * hStep - screenHeight / 2 + hStep / 2 + playerY;
                createSquare(i, j, xPos, yPos, Square.Type.Player, 0);
                createSquare(i, j, xPos, yPos, Square.Type.Player, 1);
            }
        }
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float xPos = i * lStep - screenWidth / 2 + lStep / 2 + otherX;
                float yPos = j * hStep - screenHeight / 2 + hStep / 2 + otherY;
                createSquare(i, j, xPos, yPos, Square.Type.Other, 0);
                createSquare(i, j, xPos, yPos, Square.Type.Other, 1);
            }
        }
    }

    void createSquare(int xIndex, int yIndex, float x, float y, Square.Type type, int player)
    {
        GameObject sObj = Object.Instantiate(squarePrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        sObj.transform.parent = playerGridsObjects[player].transform;
        sObj.transform.localScale = new Vector3(1.0f/(length/screenWidth), 1.0f/(height/screenHeight), 1);
        Square square = sObj.GetComponent<Square>();
        square.xIndex = xIndex;
        square.yIndex = yIndex;
        square.init(type);
        if (type.Equals(Square.Type.Player))
        {
            playerGrid[player, xIndex, yIndex] = square;
        }
        else
        {
            otherGrid[player, xIndex, yIndex] = square;
        }
    }

    public void deactivate(int player)
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                otherGrid[player, i, j].deactivate();
            }
        }
    }

    public void hitEmpty(int player, int x, int y)
    {
        playerGrid[player, x, y].setEmpty();
        if (y > 0)
        {
            playerGrid[player, x, y - 1].setEmpty();
        }
        if (y < height - 1)
        {
            playerGrid[player, x, y + 1].setEmpty();
        }
        if (x > 0)
        {
            playerGrid[player, x - 1, y].setEmpty();
            if (y > 0)
            {
                playerGrid[player, x - 1, y - 1].setEmpty();
            }
            if (y < height - 1)
            {
                playerGrid[player, x - 1, y + 1].setEmpty();
            }
        }

        if (x < length - 1)
        {
            playerGrid[player, x + 1, y].setEmpty();
            if (y > 0)
            {
                playerGrid[player, x + 1, y - 1].setEmpty();
            }
            if (y < height - 1)
            {
                playerGrid[player, x + 1, y + 1].setEmpty();
            }
        }

    }

    public void hitIce(int player, int x, int y)
    {
        playerGrid[player, x, y].setIce();
        if (y > 0)
        {
            playerGrid[player, x, y - 1].setIce();
        }
        if (y < height - 1)
        {
            playerGrid[player, x, y + 1].setIce();
        }
        if (x > 0)
        {
            playerGrid[player, x - 1, y].setIce();
            if (y > 0)
            {
                playerGrid[player, x - 1, y - 1].setIce();
            }
            if (y < height - 1)
            {
                playerGrid[player, x - 1, y + 1].setIce();
            }
        }
        
        if (x < length - 1)
        {
            playerGrid[player, x + 1, y].setIce();
            if (y > 0)
            {
                playerGrid[player, x + 1, y - 1].setIce();
            }
            if (y < height - 1)
            {
                playerGrid[player, x + 1, y + 1].setIce();
            }
        }

        bool found = searchPath(player);
        Debug.Log(found);

        if (found)
        {
            GameController.Instance.endGame(player);
        }
    }

    public void refreshOther(int player)
    {
        int other = (player + 1) % 2;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (otherGrid[player, i, j].active)
                {
                    otherGrid[player, i, j].setState(playerGrid[other, i, j].state);
                }
            }
        }
    }

    public void reveal(int player, int x, int y)
    {
        int other = (player + 1) % 2;
        otherGrid[player, x, y].setState(playerGrid[other, x, y].state);
        if (y > 0)
        {
            otherGrid[player, x, y - 1].setState(playerGrid[other, x, y - 1].state);
        }
        if (y < height - 1)
        {
            otherGrid[player, x, y + 1].setState(playerGrid[other, x, y + 1].state);
        }
        if (x > 0)
        {
            otherGrid[player, x - 1, y].setState(playerGrid[other, x - 1, y].state);

            if (y > 0)
            {
                otherGrid[player, x - 1, y - 1].setState(playerGrid[other, x - 1, y - 1].state);
            }
            if (y < height - 1)
            {
                otherGrid[player, x - 1, y + 1].setState(playerGrid[other, x - 1, y + 1].state);
            }
        }

        if (x < length - 1)
        {
            otherGrid[player, x + 1, y].setState(playerGrid[other, x + 1, y].state);
            if (y > 0)
            {
                otherGrid[player, x + 1, y - 1].setState(playerGrid[other, x + 1, y - 1].state);
            }
            if (y < height - 1)
            {
                otherGrid[player, x + 1, y + 1].setState(playerGrid[other, x + 1, y + 1].state);
            }
        }
    }

    bool searchPath(int player)
    {
        for (int i = 0; i < length; i++)
        {
            if (playerGrid[player, i, 0].iced)
            {
                traverseplayerGrid = new bool[length, height];
                for (int j = 0; j < length; j++)
                {
                    for (int k = 0; k < height; k++)
                    {
                        traverseplayerGrid[j, k] = false;
                    }
                }
                if (DFS(player, i, 0))
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool DFS(int player, int x, int y)
    {
        if (x < 0 || x >= length || y < 0 || y >= height)
        {
            return false;
        }
        if (!playerGrid[player, x, y].iced)
        {
            return false;
        }
        if (y == height - 1)
        {
            return true;
        }
        if (!traverseplayerGrid[x, y])
        {
            traverseplayerGrid[x, y] = true;
            bool result = false;
            if (DFS(player, x - 1, y))
            {
                result = true;
            }
            if (DFS(player, x + 1, y))
            {
                result = true;
            }
            if (DFS(player, x, y - 1))
            {
                result = true;
            }
            if (DFS(player, x, y + 1))
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

    // Update is called once per frame
    void Update()
    {
	
	}
}
