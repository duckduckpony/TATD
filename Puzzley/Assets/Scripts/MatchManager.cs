using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    private BoardManager board;
    public List<GameObject> currentMatches = new List<GameObject>();

    public Dictionary<string, int> TileInventory = new Dictionary<string, int>();
    public Text invText;

    public Sprite matchedSprite;

    void Start()
    {
        board = FindObjectOfType<BoardManager>();

        // add different tiles to inventory list that the inventory counter will keep track of. TODO: refactor this to use variables instead of hard coding tile names
        TileInventory.Add("Red Tile", 0);
        TileInventory.Add("Pink Tile", 0);
        TileInventory.Add("Green Tile", 0);
        TileInventory.Add("Black Tile", 0);
        TileInventory.Add("Orange Tile", 0);
        TileInventory.Add("Blue Tile", 0);

    }

    public void FindAllMatchesCall()
    {
        StartCoroutine(FindAllMatches());
    }

    private IEnumerator FindAllMatches()
    {
        yield return new WaitForSeconds(.3f);
        for (int x = 0; x < board.xSize; x++)
        {
            for (int y = 0; y < board.ySize; y++)
            {
                GameObject currentTile = board.gameboard[x, y];
                if (currentTile != null)
                {
                    if (x > 0 && x < board.xSize - 1)
                    {
                        GameObject leftTile = board.gameboard[x - 1, y];
                        GameObject rightTile = board.gameboard[x + 1, y];

                        if (leftTile != null && rightTile != null)
                        {
                            if (leftTile.tag == currentTile.tag && rightTile.tag == currentTile.tag)
                            {
                                leftTile.GetComponent<PuzzleTile>()._Matched = true;
                                rightTile.GetComponent<PuzzleTile>()._Matched = true;
                                currentTile.GetComponent<PuzzleTile>()._Matched = true;

                                leftTile.GetComponent<SpriteRenderer>().sprite = matchedSprite;
                                rightTile.GetComponent<SpriteRenderer>().sprite = matchedSprite;
                                currentTile.GetComponent<SpriteRenderer>().sprite = matchedSprite;

                                if (!leftTile.GetComponent<PuzzleTile>()._Counted)
                                {
                                    CountTileToInventory(leftTile);
                                }
                                if (!currentTile.GetComponent<PuzzleTile>()._Counted)
                                {
                                    CountTileToInventory(currentTile);
                                }
                                if (!rightTile.GetComponent<PuzzleTile>()._Counted)
                                {
                                    CountTileToInventory(rightTile);
                                }
                            }
                        }
                    }

                    if (y > 0 && y < board.ySize - 1)
                    {
                        GameObject downTile = board.gameboard[x, y - 1];
                        GameObject upTile = board.gameboard[x, y + 1];

                        if (downTile != null && upTile != null)
                        {
                            if (downTile.tag == currentTile.tag && upTile.tag == currentTile.tag)
                            {
                                downTile.GetComponent<PuzzleTile>()._Matched = true;
                                upTile.GetComponent<PuzzleTile>()._Matched = true;
                                currentTile.GetComponent<PuzzleTile>()._Matched = true;

                                downTile.GetComponent<SpriteRenderer>().sprite = matchedSprite;
                                upTile.GetComponent<SpriteRenderer>().sprite = matchedSprite;
                                currentTile.GetComponent<SpriteRenderer>().sprite = matchedSprite;

                                if (!downTile.GetComponent<PuzzleTile>()._Counted)
                                {
                                    CountTileToInventory(downTile);
                                }
                                if (!currentTile.GetComponent<PuzzleTile>()._Counted)
                                {
                                    CountTileToInventory(currentTile);
                                }
                                if (!upTile.GetComponent<PuzzleTile>()._Counted)
                                {
                                    CountTileToInventory(upTile);
                                }

                                //downTile.tag = "Matched";
                                //upTile.tag = "Matched";
                                //currentTile.tag = "Matched";
                            }
                        }
                    }
                }
            }
        }
    }

    void CountTileToInventory(GameObject tileToUse)
    {
        tileToUse.GetComponent<PuzzleTile>()._Counted = true;
        TileInventory[tileToUse.tag] = TileInventory[tileToUse.tag] + 1;
        //invText.text = tileToUse.tag + ": " + TileInventory[tileToUse.tag].ToString();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
