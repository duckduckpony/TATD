using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataHolder : MonoBehaviour
{
    public LevelData level;

    public int movesLimit;

    public bool win = false;
    public bool lose = false;

    public SelectorController selector;
    public GameObject _bm;

    void Start()
    {
        selector = FindObjectOfType<SelectorController>();
    }

    void Update() // TODO: future optimizations; this might not need to be in an update function. This could be just listening for the tilesSwappedEvent to check if over moves limit
    {
        if (selector.moves > movesLimit && !lose)
        {
            lose = true;
        }

        if (_bm.transform.childCount == 0 && !lose && !win)
        {
            win = true;
        }

    }
}
