using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PuzzleTile : MonoBehaviour
{
    [Header("Positional Variables")]
    public int row;
    public int column;
    public int targetX;
    public int targetY;
    public int spacesUnder;
    public float fallDelay;
    public float fallSpeed;
    private float fallTimer;

    private int rowHolder;
    
    private Vector2 tempPos;
    private BoardManager boardManager;
    private MatchManager matchManager;
    private GameObject otherTile;
    private GameObject selector;

    public Sprite matchedSprite;
    public SpriteRenderer mySpriteRenderer;
    public Rigidbody2D body;

    [Header("Logic")]
    public bool _Matched = false;
    public bool _Falling = false;
    public bool _ReadyToFall = false;
    public bool _Floating = false;
    public bool _Normal = true;
    public bool _Counted = false;
    public bool _Switched = false;

    //private GameObject leftTile1;
    //private GameObject rightTile1;
    //private GameObject upTile1;
    //private GameObject downTile1;

    //int spaceToFall = 0;


    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
        matchManager = FindObjectOfType<MatchManager>();
        // start with the target coords as its initial position. these will change whenever it is swapped and then it will move to the new target coord
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;

        selector = GameObject.Find("Selector");
        
    }

    private void OnEnable()
    {
        SelectorController.tilesSwappedEvent += SwapMove;
    }

    private void OnDisable()
    {
        SelectorController.tilesSwappedEvent -= SwapMove;

    }

    void Update()
    {

        // this allows columns of blocks to fall together. checks if the tile beneath it is ready to fall. if so, it becomes ready to fall as well.
        try
        {
            if (row > 0)
            {
                if (boardManager.gameboard[column, row - 1] != null)
                {
                    GameObject belowTile = boardManager.gameboard[column, row - 1];
                    if (belowTile.GetComponent<PuzzleTile>()._ReadyToFall)
                    {
                        _ReadyToFall = true;
                    }
                }
            }
            
        }
        catch (Exception e)
        {
            Debug.Log("Whoops.");
        }
        
        // check if the block has any empty space beneath it
        if (row > 0 && boardManager.gameboard[column, row - 1] == null && !_Falling && !_ReadyToFall)
        {
            _ReadyToFall = true;
            _Normal = false;
        }

        if (_ReadyToFall)
        {
            fallTimer += Time.deltaTime;
        }

        if (fallTimer > fallDelay && _ReadyToFall)
        {
            _Falling = true;
            _ReadyToFall = false;

            // see how many empty spaces are beneath the tile
            spacesUnder = 0;
            int i = 1;

            // checks if tile is 
            if (row > 1)
            {
                while ((row - i) > -1 && boardManager.gameboard[column, row - i] == null)
                {
                    i++;
                    spacesUnder++;
                }
                boardManager.gameboard[column, row] = null;
                row = row - spacesUnder;
                boardManager.gameboard[column, row] = this.gameObject;
            }
            else
            {
                boardManager.gameboard[column, row] = null;
                row = 0;
                boardManager.gameboard[column, row] = this.gameObject;
                spacesUnder = 1;
            }
        }

        if (_Falling)
        {
            fallTimer = 0;
            _ReadyToFall = false;
            targetY = row;
            if (Mathf.Abs(targetY - transform.position.y) > .1)
            {
                //Move Towards the target
                tempPos = new Vector2(transform.position.x, targetY);
                transform.position = Vector2.Lerp(transform.position, tempPos, fallSpeed);
                if (boardManager.gameboard[column, targetY] != this.gameObject)
                {
                    boardManager.gameboard[column, targetY] = this.gameObject;
                    matchManager.FindAllMatchesCall();
                }

            }
            else
            {
                //Directly set the position
                //Debug.Log("Made it to where falling should be false");
                tempPos = new Vector2(transform.position.x, targetY);
                transform.position = tempPos;
                _Falling = false;
                _Normal = true;
                matchManager.FindAllMatchesCall();
            }
        }


        if (_Matched && !_Switched)
        {
            this.mySpriteRenderer.sprite = matchedSprite;
            boardManager.DestroyAllMatches();
            _Switched = true;
        }

        // checking where we want to be. column/row get changed in swapmove function, so when that happens, the target will be updated here
        targetX = column;
        targetY = row;
        // check if piece needs to be moved; move piece;
        if (Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, 0.5f);
            if (boardManager.gameboard[column, row] != this.gameObject)
            {
                boardManager.gameboard[column, row] = this.gameObject;
            }

            if (_Normal) matchManager.FindAllMatchesCall();
            
        }
        else
        {
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
        }
        //CheckMatches();

        //if(spaceToFall != 0)
        //{
        //    Invoke("Fall", 2.0f);
        //}

    }

    

    void SwapMove()
    {
        if (_Normal && !_Floating && !_Falling && !_ReadyToFall)
        {
            if (selector.transform.position.y != row)
            {
                return;
            }
            else if (selector.transform.position.x == column)
            {
                // move left piece right
                if (boardManager.gameboard[column + 1, row] != null)
                {
                    otherTile = boardManager.gameboard[column + 1, row];
                    otherTile.GetComponent<PuzzleTile>().column -= 1;
                    column += 1;
                }
                else
                {
                    // if empty space beside tile, move tile over, and update gameboard to let it know that tile is now empty
                    column += 1;
                    boardManager.gameboard[column - 1, row] = null;
                }


            }
            else if (selector.transform.position.x + 1 == column)
            {
                // move right piece left
                if (boardManager.gameboard[column - 1, row] != null)
                {
                    otherTile = boardManager.gameboard[column - 1, row];
                    otherTile.GetComponent<PuzzleTile>().column += 1;
                    column -= 1;
                }
                else
                {
                    // if empty space beside tile, move tile over, and update gameboard to let it know that tile is now empty
                    column -= 1;
                    boardManager.gameboard[column + 1, row] = null;
                }


            }
            else
            {
                return;
            }
        }           
    }

    //IEnumerator Flash()
    //{
    //    for (int n = 0; n < 2; n++)
    //    {
    //        SetSpriteColor(Color.white);
    //        yield return new WaitForSeconds(0.1f);
    //        SetSpriteColor(spriteColor);
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    //void SetSpriteColor(Color color)
    //{
    //    Color originalCol = GetComponent<SpriteRenderer>().color;

    //}
}

//private IEnumerator Fall()
//{
//    boardManager.gameboard[column, row] = null;

//    yield return new WaitForSeconds(0.6f);

//    targetY = row;
//    if (Mathf.Abs(targetY - transform.position.y) > .1 && _Falling)
//    {
//        //Move Towards the target
//        tempPos = new Vector2(transform.position.x, targetY);
//        transform.position = Vector2.Lerp(transform.position, tempPos, .2f);
//        if (boardManager.gameboard[column, targetY] != this.gameObject)
//        {
//            boardManager.gameboard[column, targetY] = this.gameObject;
//            matchManager.FindAllMatchesCall();
//        }

//    }
//    else
//    {
//        //Directly set the position
//        tempPos = new Vector2(transform.position.x, targetY);
//        transform.position = tempPos;
//        if (boardManager.gameboard[column, targetY] != this.gameObject)
//        {
//            boardManager.gameboard[column, targetY] = this.gameObject;
//            _Falling = false;
//            matchManager.FindAllMatchesCall();
//        }
//    }



//}

//void CheckFalling()
//{
//    // check if tile is on bottom row
//    if (row != 0)
//    {
//        // if no empty space below tile, don't do anything
//        if (boardManager.gameboard[column, row-1] != null)
//        {
//            return;
//        }
//        else
//        {
//            // check 1 by 1 for empty spaces beneath tile. once there are no more empty spaces, stop checking and then change 

//            bool noMoreRoom = false;
//            while (!noMoreRoom)
//            {
//                if (boardManager.gameboard[column, row - (1 + spaceToFall)] == null)
//                {
//                    spaceToFall++;
//                }
//                else
//                {
//                    Debug.Log(spaceToFall);
//                    noMoreRoom = true;
//                }
//            }

//            row = row - spaceToFall;
//            Debug.Log(row);
//            boardManager.gameboard[column, row + spaceToFall] = null;
//        }
//    }
//}

//// check if the block has any empty space beneath it
//if (row > 0 && boardManager.gameboard[column, row - 1] == null && !_Floating)
//{
//    //if (!_Falling)
//    //{
//        _Floating = true;
//    //}
//}

//if (_Floating)
//{
//    // see how many empty spaces are beneath the tile
//    spacesUnder = 0;
//    int i = 1;
//    //_Floating = true;
//    while (boardManager.gameboard[column, row - i] == null && i<row + 1) // TODO: right now, tiles won't drop to the very first row. FIX DIS.
//    {
//        i++;
//        spacesUnder++;
//    }
//    row = row - spacesUnder;
//    _Falling = true;
//    _Floating = false;
//}

////targetY = row;

//if (_Falling)
//{
//    StartCoroutine(Fall());
//}
