using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveLimitText : MonoBehaviour
{
    private SelectorController _selector;
    private LevelDataHolder _levelData;

    public TextMeshProUGUI limitCounter;
    //public string movesText;

    // Start is called before the first frame update
    void Start()
    {
        _selector = FindObjectOfType<SelectorController>();
        _levelData = FindObjectOfType<LevelDataHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        limitCounter.text = "Moves Left: " + (_levelData.movesLimit - _selector.moves).ToString();
    }
}
