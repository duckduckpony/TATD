using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    public GameObject tile;
    public int xSize, ySize; // size of board, or at least the portion that will be filled

    public GameObject[] tiles; // houses tile prefabs

    public GameObject[,] gameboard; // multidimensional array that holds the game tiles
    public int[,] gameboardRead;

    private GameObject currentLeft;
    private GameObject currentRight;

    public float matchDestroyDelay = 2f;
    private float matchDestroyTimer = 0;

    public MatchManager _mm;
    public LevelDataHolder _ld;
    public SelectorController _sc;

    bool puzzleMode = true;

    void Start()
    {
        instance = GetComponent<BoardManager>();

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;

        _mm = FindObjectOfType<MatchManager>();
        _ld = FindObjectOfType<LevelDataHolder>();
        _sc = FindObjectOfType<SelectorController>();

        if (!puzzleMode)
        {
            CreateBoard(offset.x, offset.y);
        }
        else
        {
            CreateBoard_Puzzle(offset.x, offset.y);
        }

        
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    //void Update()
    //{

    //}

    private void CreateBoard(float xOffset, float yOffset)
    {
        gameboard = new GameObject[xSize, ySize];

        // this is where the game starts to fill the board in. bottom left corner?
        float startX = transform.position.x;
        float startY = transform.position.y;

        int hole = Random.Range(0, ySize);

        /* this iterates through every 'coordinate' of the gameboard. At the first x coordinate, essentially 0,
         * it'll then iterate through each y coordinate, instantiating a new tile in each row until the column is filled
         * then it moves on to the next column, or x coordinate.
        */
        for (int x = 0; x < xSize; x++)
        {
            if (x == hole)
            {
                for (int y = 0; y < Random.Range(2,3); y++)
                {
                    GameObject newTile;
                    int tileToUse = Random.Range(1, tiles.Length);

                    // this is where the tiles are generated. the vector3 is calculating coordinates of each tile by multiplying the size of the tile by the value of the x/y of the nested for loops
                    newTile = Instantiate(tiles[tileToUse], new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), Quaternion.identity);

                    while (MatchesAt(x, y, newTile))
                    {
                        Debug.Log("made it in here, hole.");
                        tileToUse = Random.Range(1, tiles.Length);
                        Destroy(newTile);
                        newTile = Instantiate(tiles[tileToUse], new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), Quaternion.identity);
                    }

                    newTile.name = "(" + x + ", " + y + ")";
                    gameboard[x, y] = newTile; //above line creates the tile, this line records its place in the array

                    newTile.transform.parent = transform; // parents the tiles to the board manager
                    //Sprite newSprite = tileSprites[Random.Range(0, tileSprites.Count)]; // chooses a random sprite for the new tile
                    //newTile.GetComponent<SpriteRenderer>().sprite = newSprite;

                }
            }
            else
            {
                for (int y = 0; y < ySize - Random.Range(0, 4); y++)
                {
                    int tileToUse = Random.Range(1, tiles.Length);
                    // this is where the tiles are generated. the vector3 is calculating coordinates of each tile by multiplying the size of the tile by the value of the x/y of the nested for loops
                    GameObject newTile;
                    newTile = Instantiate(tiles[tileToUse], new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), Quaternion.identity);

                    while (MatchesAt(x, y, newTile))
                    {
                        Debug.Log("made it inside");
                        tileToUse = Random.Range(1, tiles.Length);
                        Destroy(newTile);
                        newTile = Instantiate(tiles[tileToUse], new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), Quaternion.identity);
                    }

                    newTile.name = "(" + x + ", " + y + ")";
                    gameboard[x, y] = newTile;

                    newTile.transform.parent = transform; // parents the tiles to the board manager
                    //Sprite newSprite = tileSprites[Random.Range(0, tileSprites.Count)]; // chooses a random sprite for the new tile
                    //newTile.GetComponent<SpriteRenderer>().sprite = newSprite;

                }
            }
            
        }
    }

    private void CreateBoard_Puzzle(float xOffset, float yOffset)
    {
        gameboard = new GameObject[xSize, ySize];

        // this is where the game starts to fill the board in. bottom left corner?
        float startX = transform.position.x;
        float startY = transform.position.y;

        /* this iterates through every 'coordinate' of the gameboard. At the first x coordinate, essentially 0,
         * it'll then iterate through each y coordinate, instantiating a new tile if the level data matrix says a tile should be there
         * then it moves on to the next column, or x coordinate.
        */
        for (int x = 0; x < xSize; x++)
        {
            for (int y = ySize - 1; y > 0; y--)
            {
                int tileToUse = _ld.level.rows[y].row[x]; // get data from level data holder. each cell has a value in it which corresponds to a certain type of tile

                if (tileToUse > 0) // for puzzle levels, when reading matrix, if that cell holds a 0, don't bother making any tiles. only create a tile if the matrix cell holds a number
                {
                    // this is where the tiles are generated. the vector3 is calculating coordinates of each tile by multiplying the size of the tile by the value of the x/y of the nested for loops
                    GameObject newTile;
                    newTile = Instantiate(tiles[tileToUse], new Vector3(startX + (xOffset * x), startY + (yOffset * -y) + (ySize - 1), 0), Quaternion.identity); // yoffset multiplied by negative y to invert data from leveldataholder matrix

                    newTile.name = "(" + x + ", " + (-y + (ySize -1)) + ")";
                    gameboard[x, -y + (ySize - 1)] = newTile; // y is negative to invert the way tiles show up after reading data in matrix.

                    newTile.transform.parent = transform; // parents the tiles to the board manager
                }
            }
        }
    }

    // this method is used to make sure board generates without any matches to start
    private bool MatchesAt(int column, int row, GameObject currentTile)
    {
        if(column > 1 && row > 1)
        {
            if (gameboard[column - 1, row] != null && gameboard[column - 2, row] != null)
            {
                if (gameboard[column - 1, row].tag == currentTile.tag && gameboard[column - 2, row].tag == currentTile.tag)
                {
                    return true;
                }
            }
            if (gameboard[column, row - 1].tag == currentTile.tag && gameboard[column, row - 2].tag == currentTile.tag)
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if(gameboard[column, row - 1].tag == currentTile.tag && gameboard[column, row - 2].tag == currentTile.tag)
                {
                    return true;
                }
            }

            if (column > 1)
            {
                if(gameboard[column - 1, row].tag == currentTile.tag && gameboard[column - 2, row].tag == currentTile.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator DestroyMatchesAt(int column, int row)
    {
        if (gameboard[column, row].GetComponent<PuzzleTile>()._Matched)
        {
            yield return new WaitForSeconds(0.5f);

            Destroy(gameboard[column, row]);
            gameboard[column, row] = null;


            // TODO: iterate through board after last move to see if any tiles are matched. if they are, keep going. keep iterating until there's either only unmatched tiles or none at all? IDK.
        }

        
    }

    public void DestroyAllMatches()
    {
        // iterate through the entire board. at each place, if a piece exists there, check if it's matched. If so, destroy it
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (gameboard[x, y] != null)
                {
                    StartCoroutine(DestroyMatchesAt(x, y));
                    
                }
            }
        }
    }

    
    //void TilesSwappedListener()
    //{

    //    currentLeft = gameboard[(int)SelectorController.leftCoord.x, (int)SelectorController.leftCoord.y];
    //    currentRight = gameboard[(int)SelectorController.leftCoord.x + 1, (int)SelectorController.leftCoord.y];

    //    Vector2 prevLeft = currentLeft.transform.position;
    //    Vector2 prevRight = currentRight.transform.position;

    //    SwapTiles(currentLeft, currentRight);

    //    gameboard[(int)prevLeft.x, (int)prevLeft.y] = currentRight;
    //    gameboard[(int)prevRight.x, (int)prevRight.y] = currentLeft;

    //    //Debug.Log("left tile is " + currentLeft + " and right is " + currentRight);
    //}

    //void SwapTiles(GameObject left, GameObject right)
    //{
    //    left.transform.position = left.transform.position + Vector3.right;
    //    right.transform.position = right.transform.position + Vector3.left;
    //}
}

// lots of comments in here explaining how everything works to myself since I'm learning a lot of this from tutorials.
