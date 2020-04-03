using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    [System.Serializable]
    public struct rowData
    {
        public int[] row;
    }

    public rowData[] rows = new rowData[7];
}
