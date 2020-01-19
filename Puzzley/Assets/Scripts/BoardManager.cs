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

    private GameObject currentLeft;
    private GameObject currentRight;

    public float matchDestroyDelay = 2f;
    private float matchDestroyTimer = 0;

    public MatchManager _mm;

    void Start()
    {
        instance = GetComponent<BoardManager>();

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);

        _mm = FindObjectOfType<MatchManager>();
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
                    int tileToUse = Random.Range(0, tiles.Length);

                    // this is where the tiles are generated. the vector3 is calculating coordinates of each tile by multiplying the size of the tile by the value of the x/y of the nested for loops
                    newTile = Instantiate(tiles[tileToUse], new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), Quaternion.identity);

                    while (MatchesAt(x, y, newTile))
                    {
                        Debug.Log("made it in here, hole.");
                        tileToUse = Random.Range(0, tiles.Length);
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
                    int tileToUse = Random.Range(0, tiles.Length);
                    // this is where the tiles are generated. the vector3 is calculating coordinates of each tile by multiplying the size of the tile by the value of the x/y of the nested for loops
                    GameObject newTile;
                    newTile = Instantiate(tiles[tileToUse], new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), Quaternion.identity);

                    while (MatchesAt(x, y, newTile))
                    {
                        Debug.Log("made it inside");
                        tileToUse = Random.Range(0, tiles.Length);
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

    void CheckMatches()
    {

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

    private void DestroyMatchesAt(int column, int row)
    {
        if (gameboard[column, row].GetComponent<PuzzleTile>()._Matched)
        {
            //matchDestroyTimer += Time.deltaTime;

            //if (matchDestroyTimer > matchDestroyDelay)
            //{
            Debug.Log(gameboard[column, row].GetComponent<PuzzleTile>().tag + ": " + _mm.TileInventory[gameboard[column, row].GetComponent<PuzzleTile>().tag]);


            Destroy(gameboard[column, row]);
            gameboard[column, row] = null;
                //matchDestroyTimer = 0;
            //}
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
                    DestroyMatchesAt(x, y);
                }
            }
        }
    }

    //public IEnumerator handleFalling()
    //{
    //    for (int x = 0; x < xSize; x++)
    //    {
    //        for (int y = 0; y < ySize; y++)
    //        {
    //            if (gameboard[x,y].GetComponent<PuzzleTile>()._Falling)
    //            {

    //            }
    //        }
    //    }
    //    yield return new WaitForSeconds(floatTime);
    //}

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
