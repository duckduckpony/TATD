using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorController : MonoBehaviour
{
    public KeyCode kcUp;
    public KeyCode kcLeft;
    public KeyCode kcDown;
    public KeyCode kcRight;
    public KeyCode kcSwap;

    Vector2 cursorPos;

    private BoardManager boardManager;

    public GameObject cursLeft;
    public GameObject cursRight;

    public static Vector2 leftCoord;
    public static Vector2 rightCoord;

    public int moves = 0;

    public delegate void TilesSwapped();
    public static event TilesSwapped tilesSwappedEvent;

    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
    }


    void Update()
    {
        cursorPos = transform.position;

        // get directional input
        if (Input.GetKeyDown(kcUp) || Input.GetKeyDown(KeyCode.W))
        {
            // move up
            if (transform.position.y < boardManager.ySize)
            {
                transform.position = cursorPos + Vector2.up;
            }
        }
        if (Input.GetKeyDown(kcDown) || Input.GetKeyDown(KeyCode.S))
        {
            // move down
            if (transform.position.y > 0)
            {
                transform.position = cursorPos + Vector2.down;
            }
            
        }
        if (Input.GetKeyDown(kcLeft) || Input.GetKeyDown(KeyCode.A))
        {
            // move left
            if (transform.position.x > 0)
            {
                transform.position = cursorPos + Vector2.left;

            }
        }
        if(Input.GetKeyDown(kcRight) || Input.GetKeyDown(KeyCode.D))
        {
            // move right
            if (transform.position.x < (boardManager.xSize - 2))
            {
                transform.position = cursorPos + Vector2.right;
            }
        }

        leftCoord = cursLeft.transform.position;
        rightCoord = cursRight.transform.position;

        // get action input
        if (Input.GetKeyDown(kcSwap))
        {
            if (boardManager.gameboard[(int)transform.position.x, (int)transform.position.y] != null || boardManager.gameboard[((int)transform.position.x + 1), (int)transform.position.y])
            {
                if (tilesSwappedEvent != null) tilesSwappedEvent();

                moves = moves + 1;
                Debug.Log("Total # of moves is " + moves + ".");
                //Debug.Log("swap event called");
            }
        }

        

    }
}
