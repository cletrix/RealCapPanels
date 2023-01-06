using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Globe Settings", menuName = "Raffle Settings/Globe Settings")]
public class GlobeScriptable : ScriptableObject
{
    public string editionName;
    public string editionNumber;
    public int order;
    public string date;
    public string description;
    public float value;
    [Space]
    public int Winners;
    public int prizeValue;
    public int ballRaffledCount;
    public int possiblesWinnersCount;
    [Space]
    public List<string> numberBalls;
    public int indexBalls = 0;
    public void ResetRaffle()
    {
        Winners = 0;
        prizeValue = 0;
        ballRaffledCount = 0;
        possiblesWinnersCount = 0;
        numberBalls.Clear();
        indexBalls = 0;
    }


}

