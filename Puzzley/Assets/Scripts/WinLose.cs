using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLose : MonoBehaviour
{
    public GameObject selector;
    public GameObject boardManager;
    public GameObject matchManager;
    public GameObject levelDataHolder;

    public BoardManager _bm;

    public bool waiting = false;
    bool checking = true;
    public bool won = false;
    public bool lost = false;
    bool levelOver = false;

    public float matchTimer;
    public float mainTime;

    // Start is called before the first frame update
    void Start()
    {
        //selector = FindObjectOfType<SelectorController>();
        //boardManager = FindObjectOfType<BoardManager>();
        //matchManager = FindObjectOfType<MatchManager>();
        //levelDataHolder = FindObjectOfType<LevelDataHolder>();

        _bm = boardManager.GetComponent<BoardManager>();
        checking = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (selector.GetComponent<SelectorController>().moves - levelDataHolder.GetComponent<LevelDataHolder>().movesLimit >= 0 && checking)
        {
            selector.SetActive(false);
            checking = true;

            matchTimer = matchTimer - Time.deltaTime;

            if (_bm.transform.childCount == 0) won = true;
        }

        if (matchTimer <= 0 && _bm.transform.GetChildCount() > 0)
        {
            lost = true;
            checking = false;
            levelOver = true;

            Debug.Log("LOOOOOSE.");
        }

        if (won && !levelOver)
        {
            checking = false;
            levelOver = true;
            Debug.Log("Ya dun won.");
            //selector.SetActive(false);
            boardManager.SetActive(false);
            matchManager.SetActive(false);
        }

        
    }
}
