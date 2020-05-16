using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataHolder : MonoBehaviour
{
    public LevelData level;

    public int movesLimit;

    public SelectorController selector;
    public GameObject _bm;
    public GameObject _selector;

    void Start()
    {
        selector = FindObjectOfType<SelectorController>();
    }

    void Update()
    {

    }
}
