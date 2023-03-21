using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Lottery Settings", menuName = "Raffle Settings/Lottery Settings")]
public class LotteryScriptable : ScriptableObject
{
    public string firstRaffle;
    public string secondRaffle;
    public string thirdRaffle;
    public string fourthRaffle;
    public string fifithRaffle;

    public string lotteryRaffleResult;

    public void Reset()
    {
        firstRaffle = string.Empty;
        secondRaffle = string.Empty;
        thirdRaffle = string.Empty;
        fourthRaffle = string.Empty;
        fifithRaffle = string.Empty;
    }
}
