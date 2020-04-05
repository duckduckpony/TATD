using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLose : MonoBehaviour
{
    public GameObject selector;
    public GameObject boardManager;
    public GameObject matchManager;
    public GameObject levelDataHolder;

    // Start is called before the first frame update
    void Start()
    {
        //selector = FindObjectOfType<SelectorController>();
        //boardManager = FindObjectOfType<BoardManager>();
        //matchManager = FindObjectOfType<MatchManager>();
        //levelDataHolder = FindObjectOfType<LevelDataHolder>();

    }

    // Update is called once per frame
    void Update()
    {
        if (levelDataHolder.GetComponent<LevelDataHolder>().lose || levelDataHolder.GetComponent<LevelDataHolder>().win)
        {
            selector.SetActive(false);
            boardManager.SetActive(false);
            matchManager.SetActive(false);
        }
    }
}
