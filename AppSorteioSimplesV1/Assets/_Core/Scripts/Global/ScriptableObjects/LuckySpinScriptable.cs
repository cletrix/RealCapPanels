using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lucky Spin Settings", menuName = "Raffle Settings/Lucky Spin Settings")]

public class LuckySpinScriptable : ScriptableObject
{
    public string editionName;
    public int nextDraw;
    public int currentDrawIndex;
    public string currentResult;
    public string prizeDescription;
    public float prizeValue;
    public List<string> allSpinsResult;

    public List<int> AllSpinsOder;

    public void Reset()
    {
        nextDraw = 1;
        currentDrawIndex = -1;
        currentResult = string.Empty;
        allSpinsResult.Clear();
        AllSpinsOder.Clear();
    }

    public void AddOrder(int _order)
    {
        if(!AllSpinsOder.Contains(_order))
        {
            AllSpinsOder.Add(_order);
        }
    }
}
