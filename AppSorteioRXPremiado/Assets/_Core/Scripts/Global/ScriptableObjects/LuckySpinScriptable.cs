using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lucky Spin Settings", menuName = "Raffle Settings/Lucky Spin Settings")]

public class LuckySpinScriptable : ScriptableObject
{
    public int currentSpinID;
    public string currentResult;
    public string prizeDescription;
    public float prizeValue;
    public List<string> allSpinsResult;


    public void Reset()
    {
        currentSpinID = 1;
        currentResult = string.Empty;
        allSpinsResult.Clear();
    }
}
