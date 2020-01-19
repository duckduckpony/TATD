using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvCounter : MonoBehaviour
{
    private MatchManager matchManager;

    public TextMeshProUGUI counter;
    public string ItemName;



    void Start()
    {
        matchManager = FindObjectOfType<MatchManager>();
    }

    void Update()
    {
        counter.text = matchManager.TileInventory[ItemName].ToString();
    }
}
